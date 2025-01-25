using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    public Transform respawnPosition;
    // public List<GameObject> objectsToRespawn;
    
    public string PlayerTag = "Player";
    public string BallTag = "Ball";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag) || other.CompareTag(BallTag))
        {
            other.gameObject.transform.position = respawnPosition.position;
            //reset rigidbody velocity
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}