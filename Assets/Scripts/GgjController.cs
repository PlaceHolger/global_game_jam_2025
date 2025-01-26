using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;

public class GgjController : MonoBehaviour
{
    public enum eInputDevice
    {
        KeyboardWASD,
        KeyboardArrows,
        ControllerOne,
        ControllerTwo
    }

    [FormerlySerializedAs("player")] public eInputDevice controlScheme = eInputDevice.KeyboardWASD;
    public float speed = 5.0f;
    public float dashSpeed = 20.0f;
    public float dashTime = 0.1f;
    public float dashCooldown = 1.0f;

    public TeamSettings teamSettings;
    public UnityEvent OnDashStart;
    public UnityEvent OnDashEnd;
    public UnityEvent OnHit;

    public List<GameObject> alignToDash;

    public GameObject positionIndicator;

    private Rigidbody rb;
    private bool isDashing;
    private float dashEndTime;
    private float lastDashTime;

    public Vector3 dashPushBoxSize = new(1.0f, 1.0f, 1.0f);
    public float dashPushForceBall = 8.0f;
    public float dashPushForcePlayer = 30.0f;

    private Vector3 lastDashDirection;
    private Quaternion lastDashOrientation;
    private bool didDashPushHit;

    public LayerMask dashPushLayerMask;

    private float lastHitTime;
    private float knockOutEndTime;
    public float knockHitTime = 0.5f;
    public float hitBallAutoAimAngle = 38.0f;
    public GameObject ball;

    private float initialLightIntensity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateColors();
        ball = GameObject.FindGameObjectWithTag("Ball");

        if (positionIndicator.TryGetComponent(out Light pointLight))
        {
            initialLightIntensity = pointLight.intensity;
        }

        //check if control scheme is set to controller and if so, check if a controller is connected
        if (controlScheme == eInputDevice.ControllerOne && Gamepad.all.Count < 1)
        {
            Debug.LogWarning("ControllerOne selected but no controller connected, disable player");
            gameObject.SetActive(false);
        }

        else if (controlScheme == eInputDevice.ControllerTwo && Gamepad.all.Count < 2)
        {
            Debug.LogWarning("ControllerTwo selected but no controller connected, disable player");
            gameObject.SetActive(false);
        }
    }

    public void OnBeingHit(Vector3 hitDir, float hitStrength, float knockOutMulti = 1.0f)
    {
        knockOutEndTime = Time.time + knockHitTime * knockOutMulti;
        if (hitStrength > 0.0f)
        {
            rb.AddForce(hitDir.normalized * hitStrength, ForceMode.Impulse);
            OnHit.Invoke();
        }
    }

    void UpdateColors()
    {
        var feetRender = GetComponentInChildren<SkinnedMeshRenderer>();
        if (feetRender)
            feetRender.material.color = teamSettings.teamColor;

        var lightComp = GetComponentInChildren<Light>();
        if (lightComp)
            lightComp.color = teamSettings.teamColor;

        var trailComp = GetComponentInChildren<TrailRenderer>();
        if (trailComp)
            trailComp.startColor = teamSettings.teamColor;

        var particles = GetComponentsInChildren<ParticleSystem>();
        foreach (var particle in particles)
        {
            if(particle.name.Contains("Hit"))
                continue;

            var main = particle.main;
            main.startColor = teamSettings.teamColor;
            // var colorOverLifetime = particle.colorOverLifetime;
            // colorOverLifetime.color = teamSettings.teamColor;
        }
    }

    void Update()
    {
        if (Time.time < knockOutEndTime)
            return;

        Vector3 move = Vector3.zero;
        bool dashPressed = false;

        if (controlScheme == eInputDevice.KeyboardWASD)
        {
            move.x = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
            move.z = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
            dashPressed = Input.GetKeyDown(KeyCode.LeftShift);
        }
        else if (controlScheme == eInputDevice.KeyboardArrows)
        {
            move.x = Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
            move.z = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
            dashPressed = Input.GetKeyDown(KeyCode.RightShift);
        }

        if (controlScheme == eInputDevice.ControllerOne)
        {
            Vector2 axis = Gamepad.all[0].leftStick.ReadValue();
            move.x = axis.x;
            move.z = axis.y;
            dashPressed = Gamepad.all[0].aButton.ReadValue() > 0f;
        }
        else if (controlScheme == eInputDevice.ControllerTwo)
        {
            Vector2 axis = Gamepad.all[1].leftStick.ReadValue();
            move.x = axis.x;
            move.z = axis.y;
            dashPressed = Gamepad.all[1].aButton.ReadValue() > 0f;
        }

        if (dashPressed && Time.time >= lastDashTime + dashCooldown)
        {
            if (move == Vector3.zero)
                move.y = 1; //dash up forward if no direction
            // else
            // {
            //     foreach (GameObject alignObject in alignToDash)
            //     {
            //         alignObject.transform.LookAt(transform.position + move);
            //     }
            //     Debug.DrawLine(transform.position, transform.position + move * 3f, Color.red, 10f);
            // }
            StartDash(move);
        }

        if (isDashing)
        {
            if (Time.time < dashEndTime)
            {
                if (move == Vector3.zero)
                    rb.linearVelocity = Vector3.up * 0.6f * dashSpeed;
                else
                {
                    move = BallAutoAim(move);
                    rb.linearVelocity = move.normalized * dashSpeed;
                }
            }
            else
            {
                isDashing = false;
                OnDashEnd.Invoke();
                if (!didDashPushHit)
                    DoForcePush(move);
            }
        }
        else
        {
            if (move.sqrMagnitude > 0.02f)
            {
                float angleToMove = Vector3.Angle(rb.linearVelocity, move);
                if (angleToMove > 90.0f)
                {
                    //reset x and z velocity
                    float damping = 1.0f - 15 * Time.deltaTime;
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x * damping, rb.linearVelocity.y, rb.linearVelocity.z * damping);
                }

                rb.linearVelocity += move * (speed * Time.deltaTime);
            }
        }
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            positionIndicator.transform.position = hit.point + Vector3.up * 0.5f;
            if (positionIndicator.TryGetComponent(out Light pointLight))
            {
                float lightStrength = Mathf.Clamp((5f - hit.distance) / 4f, 0.1f, 1f);
                pointLight.intensity = initialLightIntensity * lightStrength;

            }
        }
    }

    void StartDash(Vector3 direction)
    {
        direction = BallAutoAim(direction);

        didDashPushHit = false;
        DoForcePush(direction);

        foreach (GameObject alignObject in alignToDash)
        {
            alignObject.transform.LookAt(transform.position + direction);
        }

        isDashing = true;
        dashEndTime = Time.time + dashTime;
        lastDashTime = Time.time;
        rb.ResetInertiaTensor();
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = direction.normalized * dashSpeed;
        OnDashStart.Invoke();
    }

    private Vector3 BallAutoAim(Vector3 direction)
    {
        // Find the ball
        if (ball && direction.y < 0.33f)
        {
            Vector3 ballPos = ball.transform.position;
            Vector3 playerPos = transform.position;
            // calc angle to ball ignoring y
            Vector3 ballDirection = ballPos - playerPos;
            ballDirection.y = 0;
            Vector3 playerDirection = direction;
            playerDirection.y = 0;
            float angleToBall = Vector3.Angle(playerDirection, ballDirection);
            float distanceToBall = 0.33f * ballDirection.magnitude;

            // Check if the ball is within a certain angle and distance
            if (angleToBall < hitBallAutoAimAngle && distanceToBall < dashPushBoxSize.z - 1)
            {
                direction = ballPos - playerPos;
                direction.Normalize();
                //Debug.Log("Auto-aiming at ball, adjusted angle: " + angleToBall);
            }
        }

        return direction;
    }

    private void DoForcePush(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.01f)
            return; //no force push if no direction

        // Perform a box sweep in the direction of the dash
        //var hitColliders = Physics.BoxCastAll(transform.position, dashPushBoxSize / 2, direction);
        var checkPosOffset = direction.normalized * (dashPushBoxSize.z / 2);
        var hitColliders = Physics.OverlapBox(transform.position + checkPosOffset, dashPushBoxSize / 2, Quaternion.LookRotation(checkPosOffset), dashPushLayerMask);

        lastDashDirection = transform.position + checkPosOffset;
        lastDashOrientation = Quaternion.LookRotation(checkPosOffset);

        foreach (var hit in hitColliders)
        {
            // Check if the hit object is the ball or another player
            if (hit.CompareTag("Ball") || hit.CompareTag("Player"))
            {
                if (hit.gameObject == gameObject)
                    continue;
                // Apply an additional force push to the hit object
                Rigidbody hitRb = hit.GetComponent<Rigidbody>();
                if (hitRb)
                {
                    Vector3 pushDirection = direction.normalized;
                    if (hit.CompareTag("Player"))
                    {
                        hit.GetComponent<GgjController>().OnBeingHit(direction, dashPushForcePlayer);
                    }
                    else
                    {
                        float dashPushForce = dashPushForceBall;
                        hitRb.AddForce(pushDirection * dashPushForce, ForceMode.Impulse);
                    }

                    didDashPushHit = true;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        //draw a box in the direction of the dash
        if (lastDashDirection == Vector3.zero)
            return;
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(lastDashDirection, lastDashOrientation, dashPushBoxSize);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}