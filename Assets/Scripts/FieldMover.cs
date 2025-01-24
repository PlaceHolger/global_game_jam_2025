using UnityEngine;

public class FieldMover : MonoBehaviour
{
    public float rotationSpeed = 100.0f;

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            transform.Rotate(Vector3.right, -rotationSpeed * Time.deltaTime);
        }
    }
}
