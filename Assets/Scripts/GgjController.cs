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
        
        if(move.sqrMagnitude > 0.02f)
        {
            rb.linearVelocity += move * (speed * Time.deltaTime);
        }
    }
}
