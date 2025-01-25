using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private List<GameObject> focusObjects;

    [SerializeField] private float minimalFov = 5f;

    [SerializeField] private float maxFov = 90f;
    
    [SerializeField] 
    [Range(0.01f, 1f)] 
    private float lerpTime = 0.5f;
    
    private Vector3 startPosition;
    private Camera controlledCamera;
    
    private void Awake()
    {
        controlledCamera = Camera.main;       
        startPosition = transform.position;
    }

    private void Update()
    {
        Vector3 minPositions = focusObjects.Count > 0 ? focusObjects[0].transform.position : Vector3.zero;
        Vector3 maxPositions = focusObjects.Count > 0 ? focusObjects[0].transform.position : Vector3.zero;;
        
        foreach (GameObject focusObject in focusObjects)
        {
            Vector3 focusObjectPosition = focusObject.transform.position;
            
            minPositions.x = Mathf.Min(focusObjectPosition.x, minPositions.x);
            minPositions.y = Mathf.Min(focusObjectPosition.y, minPositions.y);
            minPositions.z = Mathf.Min(focusObjectPosition.z, minPositions.z);
            
            maxPositions.x = Mathf.Max(focusObjectPosition.x, maxPositions.x);
            maxPositions.y = Mathf.Max(focusObjectPosition.y, maxPositions.y);
            maxPositions.z = Mathf.Max(focusObjectPosition.z, maxPositions.z);
        }

        Vector3 focusCenter = (minPositions + maxPositions) / 2f;

        Vector3 projectedPosition = Vector3.ProjectOnPlane(focusCenter, Vector3.forward);
        Vector3 desiredPosition = new Vector3(projectedPosition.x, projectedPosition.y + (projectedPosition.z - startPosition.z) * 0.5f, startPosition.z);
        controlledCamera.transform.position = Vector3.Lerp(controlledCamera.transform.position, desiredPosition, lerpTime);

        Vector3 minDirection = minPositions - controlledCamera.transform.position;
        Vector3 maxDirection = maxPositions - controlledCamera.transform.position;
        Vector3 minDirectionFlat = minDirection.ProjectOntoPlane(Vector3.up);
        Vector3 maxDirectionFlat = maxDirection.ProjectOntoPlane(Vector3.up);
        float openingAngleWidth = Mathf.Acos(Vector3.Dot(minDirectionFlat.normalized, maxDirectionFlat.normalized)) * Mathf.Rad2Deg;
        
        minDirectionFlat = minDirection.ProjectOntoPlane(Vector3.left);
        maxDirectionFlat = maxDirection.ProjectOntoPlane(Vector3.left);
        float openingAngleHeight = Mathf.Acos(Vector3.Dot(minDirectionFlat.normalized, maxDirectionFlat.normalized)) * Mathf.Rad2Deg;
        openingAngleHeight *= controlledCamera.aspect;

        float openingAngle = Mathf.Max(minimalFov, Mathf.Min(maxFov, Mathf.Max(openingAngleWidth * 0.75f, openingAngleHeight * 0.75f)));

        float newFov = Mathf.Lerp(openingAngle, controlledCamera.fieldOfView, lerpTime);
        if (!float.IsNaN(newFov))
        {
            controlledCamera.fieldOfView = newFov;
            Quaternion lookAtQuaternion = Quaternion.LookRotation(focusCenter - controlledCamera.transform.position, Vector3.up);
            controlledCamera.transform.rotation = Quaternion.Slerp(controlledCamera.transform.rotation, lookAtQuaternion, lerpTime);

        } 
    }
}
