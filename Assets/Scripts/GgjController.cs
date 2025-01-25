using UnityEngine;
using UnityEngine.Serialization;

public class GgjController : MonoBehaviour
{
    public enum eInputDevice { KeyboardWASD, KeyboardArrows } //todo: gamepads
    
    [FormerlySerializedAs("player")] 
    public eInputDevice controlScheme = eInputDevice.KeyboardWASD;
    public float speed = 5.0f;
    
    public TeamSettings teamSettings;

    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        UpdateColors();
    }
    
    void UpdateColors()
    {
        var allRenderer = GetComponentsInChildren<Renderer>();
        foreach (var renderer in allRenderer)
        {
            renderer.material = teamSettings.teamMaterial;
        }
    }
    
    void Update()
    {
        Vector3 move = Vector3.zero;

        if (controlScheme == eInputDevice.KeyboardWASD)
        {
            move.x = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
            move.z = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        }
        else if (controlScheme == eInputDevice.KeyboardArrows)
        {
            move.x = Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
            move.z = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
        }

        if (move.sqrMagnitude > 0.02f)
        {
            float angleToMove = Vector3.Angle(rb.linearVelocity, move);
            if(angleToMove > 90.0f)
            {
                //reset x and z velocity
                float damping = 1.0f - 20 * Time.deltaTime;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x * damping, rb.linearVelocity.y, rb.linearVelocity.z * damping);
            }
            //var angleBoost = 1.0f + Mathf.Clamp(angleToMove / 90.0f, 0.0f, 2.0f);
            rb.linearVelocity += move * (speed * Time.deltaTime);
        }
    }
}
