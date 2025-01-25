using UnityEngine;

public class GgjController : MonoBehaviour
{
    public enum Player { Player1, Player2 }
    public Player player = Player.Player1;
    public float speed = 5.0f;

    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        Vector3 move = Vector3.zero;

        if (player == Player.Player1)
        {
            move.x = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
            move.z = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        }
        else if (player == Player.Player2)
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
