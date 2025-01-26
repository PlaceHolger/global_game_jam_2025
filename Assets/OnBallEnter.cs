using UnityEngine;
using UnityEngine.Events;

public class OnBallEnter : MonoBehaviour
{
    [SerializeField]
    private string BallTag = "Ball";
    
    public UnityEvent OnBallEnterEvent;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(BallTag))
        {
            OnBallEnterEvent.Invoke();
        }
    }
}
