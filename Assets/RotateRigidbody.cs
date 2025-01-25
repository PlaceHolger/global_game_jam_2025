using System;
using UnityEngine;

public class RotateRigidbody : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);

    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //rb.rotation = Quaternion.Euler(0, 0, rb.rotation.eulerAngles.z + rotationSpeed * Time.deltaTime);
        //rb.rotation = rb.rotation * Quaternion.Euler(rotationSpeed * Time.deltaTime);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotationSpeed * Time.fixedDeltaTime));
    }
}
