using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Globals : MonoBehaviour
{
    public static UnityEvent OnResetAfterGoal = new UnityEvent();
    public static UnityEvent OnSwitchGoals = new UnityEvent();
    public static UnityEvent OnRotate = new UnityEvent();

    public List<TeamSettings> TeamData;
            
    private static Globals s_Instance = null;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static Globals GetInstance()
    {
        if (!s_Instance)
        {
            s_Instance = FindAnyObjectByType<Globals>(FindObjectsInactive.Include);
        }
        return s_Instance;
    }
            
    public void ResetAfterGoal()
    {
        OnResetAfterGoal.Invoke();
    }
    public void SwitchGoal()
    {
        OnSwitchGoals.Invoke();
    }
            
    public TeamSettings GetOtherTeam(TeamSettings.eTeam team)
    {
        foreach (var teamData in TeamData)
        {
            if (teamData.team != team)
            {
                return teamData;
            }
        }
        return null;
    }

    public void Update()
    {
        //just some global input checks
        //rotate "rotationStep" degrees on Z axis
        if (Input.GetKey(KeyCode.Alpha1))
        {
            FindAnyObjectByType<FieldMover2>().RotateZ(true);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            FindAnyObjectByType<FieldMover2>().RotateZ(false);
        }
        //rotate "rotationStep" degrees on X axis
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            FindAnyObjectByType<FieldMover2>().RotateX(true);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            FindAnyObjectByType<FieldMover2>().RotateX(false);
        }
        //rotate "rotationStep" degrees on Y axis
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            FindAnyObjectByType<FieldMover2>().RotateY(true);
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            FindAnyObjectByType<FieldMover2>().RotateY(false);
        }
    }
}