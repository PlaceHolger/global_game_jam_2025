using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public string StartPosition = "StartPosition";
    
    private float initialLightIntensity;
    public GameObject positionIndicator;
    
    void OnEnable()
    {
        Globals.OnResetAfterGoal.AddListener(ResetBall);
    }

    private void OnDisable()
    {
        Globals.OnResetAfterGoal.RemoveListener(ResetBall);
    }

    private void Start()
    {
        if (positionIndicator.TryGetComponent(out Light pointLight))
        {
            initialLightIntensity = pointLight.intensity;
        }
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            positionIndicator.transform.position = hit.point + Vector3.up * 0.5f;
            if (positionIndicator.TryGetComponent(out Light pointLight))
            {
                float lightStrength = Mathf.Clamp((5f - hit.distance) / 4f, 0.1f, 1f);
                pointLight.intensity = initialLightIntensity * lightStrength;

            }
        }
    }

    private void ResetBall()
    {
        transform.position = GameObject.FindWithTag(StartPosition).transform.position;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
