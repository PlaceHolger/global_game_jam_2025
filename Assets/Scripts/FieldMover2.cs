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
    
    public bool RotateAround(Vector3 axis) //will rotate by rotationStep degrees
    {
        if(IsRotating())
            return false;
        Vector3 target = transform.eulerAngles + new Vector3(axis.x * rotationStep, axis.y * rotationStep, axis.z * rotationStep);
        m_CurrentTween = Tween.EulerAngles(transform, transform.eulerAngles, target, rotationTime);
        return true;
    }
    
    public bool RotateZ(bool isClockwise)
    {
        if(RotateAround(Vector3.forward * (isClockwise ? 1 : -1)))
        {
            Globals.OnRotateZ.Invoke();
            return true;
        }

        return false;
    }
    
    public bool RotateX(bool isClockwise)
    {
        return RotateAround(Vector3.right * (isClockwise ? 1 : -1));
    }
    
    public bool RotateY(bool isClockwise)
    {
        return RotateAround(Vector3.up * (isClockwise ? 1 : -1));
    }
}
