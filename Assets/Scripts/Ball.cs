using UnityEngine;

public class Ball : MonoBehaviour
{
    public string StartPosition = "StartPosition";
    
    void OnEnable()
    {
        Globals.OnResetAfterGoal.AddListener(ResetBall);
    }

    private void OnDisable()
    {
        Globals.OnResetAfterGoal.RemoveListener(ResetBall);
    }

    private void ResetBall()
    {
        transform.position = GameObject.FindWithTag(StartPosition).transform.position;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
