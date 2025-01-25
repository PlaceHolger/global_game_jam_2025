using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;

public class GgjController : MonoBehaviour
{
    public enum eInputDevice { KeyboardWASD, KeyboardArrows, ControllerOne, ControllerTwo }

    [FormerlySerializedAs("player")]
    public eInputDevice controlScheme = eInputDevice.KeyboardWASD;
    public float speed = 5.0f;
    public float dashSpeed = 20.0f;
    public float dashTime = 0.1f;
    public float dashCooldown = 1.0f;

    public TeamSettings teamSettings;
    public UnityEvent OnDashStart;
    public UnityEvent OnDashEnd;

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
    public float knockHitTime = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateColors();
    }

    public void OnBeingHit()
    {
        lastHitTime = Time.time;
    }

    void UpdateColors()
    {
        var feetRender = GetComponentInChildren<SkinnedMeshRenderer> ();
        if(feetRender) 
            feetRender.material.color = teamSettings.teamColor;
        
        var light = GetComponentInChildren<Light> ();
        if(light) 
            light.color = teamSettings.teamColor;
    }

    void Update()
    {
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
        if ( controlScheme == eInputDevice.ControllerOne ) {
            Vector2 axis = Gamepad.all [ 0 ].leftStick.ReadValue ();
            move.x = axis.x;
            move.z = axis.y;
            dashPressed = Gamepad.all [ 0 ].aButton.ReadValue()>0f;
        } else if ( controlScheme == eInputDevice.ControllerTwo ) {
            Vector2 axis = Gamepad.all [ 1 ].leftStick.ReadValue ();
            move.x = axis.x;
            move.z = axis.y;
            dashPressed = Gamepad.all [ 1 ].aButton.ReadValue () > 0f;
        }

        if (dashPressed && Time.time >= lastDashTime + dashCooldown)
        {
            if (move == Vector3.zero)
                move.y = 1; //dash up forward if no direction
            StartDash(move);
        }

        if (isDashing)
        {
            if (Time.time < dashEndTime)
            {
                if (move == Vector3.zero)
                    rb.linearVelocity = Vector3.up * 0.6f * dashSpeed;
                else 
                    rb.linearVelocity = move.normalized * dashSpeed;
            }
            else
            {
                isDashing = false;
                OnDashEnd.Invoke();
                if(!didDashPushHit)
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
    }

    void StartDash(Vector3 direction)
    {
        didDashPushHit = false;
        DoForcePush(direction);

        isDashing = true;
        dashEndTime = Time.time + dashTime;
        lastDashTime = Time.time;
        rb.ResetInertiaTensor();
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = direction.normalized * dashSpeed;
        OnDashStart.Invoke();
    }

    private void DoForcePush(Vector3 direction)
    {
        if(direction.sqrMagnitude < 0.01f)
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
                if(hit.gameObject == gameObject)
                    continue;
                // Apply an additional force push to the hit object
                Rigidbody hitRb = hit.GetComponent<Rigidbody>();
                if (hitRb)
                {
                    Vector3 pushDirection = direction.normalized;
                    float dashPushForce = hit.CompareTag("Ball") ? dashPushForceBall : dashPushForcePlayer;
                    hitRb.AddForce(pushDirection * dashPushForce, ForceMode.Impulse);
                    didDashPushHit = true;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        //draw a box in the direction of the dash
        if(lastDashDirection == Vector3.zero)
            return;
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(lastDashDirection, lastDashOrientation, dashPushBoxSize);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}