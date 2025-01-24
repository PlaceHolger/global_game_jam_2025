using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    public Transform respawnPosition;
    public List<GameObject> objectsToRespawn;

    void OnTriggerEnter(Collider other)
    {
        if (objectsToRespawn.Contains(other.gameObject))
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