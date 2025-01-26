using System;
using UnityEngine;
using PrimeTween;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FieldMover2 : MonoBehaviour
{
    public float rotationTime = 2;
    public float rotationStep = 90.0f;
    
    private Tween m_CurrentTween;
    private bool m_IsRotating = false;
    
    public UnityEvent OnRotationZ90 = new UnityEvent();
    public UnityEvent OnRotationZ180 = new UnityEvent();
    public UnityEvent OnRotationZ270 = new UnityEvent();
    [FormerlySerializedAs("OnRotationZ360")] public UnityEvent OnRotationZ0 = new UnityEvent();

    public void DoRandomRotateZ()
    {
        bool isClockwise = Random.value > 0.5f;
        RotateZ(isClockwise);
    }
    
    public bool IsRotating()
    {
        return m_CurrentTween.isAlive;
    }

    private void Update()
    {
        if(m_CurrentTween.isAlive)
        {
            m_IsRotating = true;
        }
        else
        {
            if (m_IsRotating)
            {
                //so we were rotating and now we are not, fire the correct event
                if (Mathf.Approximately(transform.eulerAngles.z, 90))
                    OnRotationZ90.Invoke();
                else if (Mathf.Approximately(transform.eulerAngles.z, 180))
                    OnRotationZ180.Invoke();
                else if (Mathf.Approximately(transform.eulerAngles.z, 270))
                    OnRotationZ270.Invoke();
                else if (Mathf.Approximately(transform.eulerAngles.z, 0) || Mathf.Approximately(transform.eulerAngles.z, 360))
                    OnRotationZ0.Invoke();
                m_IsRotating = false;
            }
        }
    }

    public void SetRotationZ(float z)
    {
        if(IsRotating())
            return;
        
        //check which direction is shorter, we dont want to rotate 270 degrees if we can rotate 90
        float angle = z - transform.eulerAngles.z;
        if (angle > 180)
            z -= 360;
        else if (angle < -180)
            z += 360;
        
        m_CurrentTween = Tween.EulerAngles(transform, transform.eulerAngles, new Vector3(0, 0, z), rotationTime);
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
