using UnityEngine;

public class ViewAlign : MonoBehaviour
{
    private Transform _cam;

    void Start(){
        _cam = Camera.main.transform;
    }

    void LateUpdate(){
        transform.LookAt(_cam.position, Vector3.up);
    }
}
