using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private string BallTag = "Ball";
    
    [SerializeField]
    private string StartPosition = "StartPosition";

    private float m_ResetTime = 1.0f;
    
    public UnityEvent OnGoal;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(BallTag))
        {
            Invoke(nameof(ResetBall), m_ResetTime);
            OnGoal.Invoke();
        }
    }

    private void ResetBall() //todo: the ball should reset itself
    {
        var ball = GameObject.FindWithTag(BallTag);
        ball.transform.position = GameObject.FindWithTag(StartPosition).transform.position;
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
