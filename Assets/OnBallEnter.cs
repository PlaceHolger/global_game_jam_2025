using System;
using UnityEngine;
using UnityEngine.Events;

public class OnBallEnter : MonoBehaviour
{
    [SerializeField]
    private string BallTag = "Ball";
    private bool NotWhileRotating = true;
    
    public UnityEvent OnBallEnterEvent;
    private FieldMover2 m_FieldRotator;

    private void Awake()
    {
        m_FieldRotator = FindAnyObjectByType<FieldMover2>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(BallTag))
        {
            if(m_FieldRotator && m_FieldRotator.IsRotating() && NotWhileRotating)
                return;
            OnBallEnterEvent.Invoke();
        }
    }
}

//custom editor with a button to trigger the event
#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(OnBallEnter))]
public class OnBallEnterEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        OnBallEnter myScript = (OnBallEnter)target;
        if (GUILayout.Button("Trigger Event"))
        {
            myScript.OnBallEnterEvent.Invoke();
        }
    }
}
#endif