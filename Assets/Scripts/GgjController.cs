using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GgjController : MonoBehaviour
{
    public enum eInputDevice { KeyboardWASD, KeyboardArrows } //todo: gamepads

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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateColors();
    }

    void UpdateColors()
    {
        var allRenderer = GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in allRenderer)
        {
            renderer.material = teamSettings.teamMaterial;
        }
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

        if (dashPressed && Time.time >= lastDashTime + dashCooldown)
        {
            StartDash(move);
        }

        if (isDashing)
        {
            if (Time.time < dashEndTime)
            {
                rb.linearVelocity = move.normalized * dashSpeed;
            }
            else
            {
                isDashing = false;
                OnDashEnd.Invoke();
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
        isDashing = true;
        dashEndTime = Time.time + dashTime;
        lastDashTime = Time.time;
        rb.ResetInertiaTensor();
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = direction.normalized * dashSpeed;
        OnDashStart.Invoke();
    }
}