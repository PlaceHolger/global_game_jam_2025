using UnityEngine;

public class ViewAlign : MonoBehaviour
{
    private Transform _cam;

    void Start(){
        _cam = Camera.main.transform;
    }

    void Update(){
        transform.LookAt(_cam.position, Vector3.up);
    }
}
