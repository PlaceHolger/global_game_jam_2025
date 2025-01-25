using UnityEngine;
using PrimeTween;

public class FieldMover2 : MonoBehaviour
{
    public float rotationTime = 2;
    public float rotationStep = 90.0f;
    
    private Tween m_CurrentTween;

    public void DoRandomRotateZ()
    {
        bool isClockwise = Random.value > 0.5f;
        RotateZ(isClockwise);
    }
    
    public bool IsRotating()
    {
        return m_CurrentTween.isAlive;
    }
    
    public void RotateAround(Vector3 axis) //will rotate by rotationStep degrees
    {
        if(IsRotating())
            return;
        Vector3 target = transform.eulerAngles + new Vector3(axis.x * rotationStep, axis.y * rotationStep, axis.z * rotationStep);
        m_CurrentTween = Tween.EulerAngles(transform, transform.eulerAngles, target, rotationTime);
    }
    
    public void RotateZ(bool isClockwise)
    {
        RotateAround(Vector3.forward * (isClockwise ? 1 : -1));
    }
    
    public void RotateX(bool isClockwise)
    {
        RotateAround(Vector3.right * (isClockwise ? 1 : -1));
    }
    
    public void RotateY(bool isClockwise)
    {
        RotateAround(Vector3.up * (isClockwise ? 1 : -1));
    }
}
