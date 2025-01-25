using UnityEngine;
using PrimeTween;

public class FieldMover2 : MonoBehaviour
{
    public float rotationTime = 2;
    public float rotationStep = 90.0f;
    
    public float cameraShakeWhileRotatingDuration = 0.5f;
    
    private Tween m_CurrentTween;

    public void DoRandomRotateZ()
    {
        bool isClockwise = Random.value > 0.5f;
        m_CurrentTween = Tween.EulerAngles(transform, transform.eulerAngles, transform.eulerAngles + new Vector3(0, 0, isClockwise ? rotationStep : -rotationStep), rotationTime);
    }
    
    public void DoRandomRotateX()
    {
        bool isClockwise = Random.value > 0.5f;
        m_CurrentTween = Tween.EulerAngles(transform, transform.eulerAngles, transform.eulerAngles + new Vector3(isClockwise ? rotationStep : -rotationStep, 0, 0), rotationTime);
    }

    void Update()
    {
        if(m_CurrentTween.isAlive)
            return;
        
        //roate "rotationStep" degrees on Z axis
        if (Input.GetKey(KeyCode.Alpha1))
        {
            m_CurrentTween = Tween.EulerAngles(transform, transform.eulerAngles, transform.eulerAngles + new Vector3(0, 0, rotationStep), rotationTime);
            if(cameraShakeWhileRotatingDuration > 0)
                Tween.ShakeCamera(Camera.main, cameraShakeWhileRotatingDuration, cameraShakeWhileRotatingDuration);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            m_CurrentTween = Tween.EulerAngles(transform, transform.eulerAngles, transform.eulerAngles - new Vector3(0, 0, rotationStep), rotationTime);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            m_CurrentTween = Tween.EulerAngles(transform, transform.eulerAngles, transform.eulerAngles + new Vector3(rotationStep, 0, 0), rotationTime);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            m_CurrentTween = Tween.EulerAngles(transform, transform.eulerAngles, transform.eulerAngles - new Vector3(rotationStep, 0, 0), rotationTime);
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            m_CurrentTween = Tween.EulerAngles(transform, transform.eulerAngles, transform.eulerAngles + new Vector3(0, rotationStep, 0), rotationTime);
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            m_CurrentTween = Tween.EulerAngles(transform, transform.eulerAngles, transform.eulerAngles - new Vector3(0, rotationStep, 0), rotationTime);
        }
    }
}
