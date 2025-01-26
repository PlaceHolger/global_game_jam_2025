using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Globals : MonoBehaviour
{
    public static UnityEvent OnResetAfterGoal = new UnityEvent();
    public static UnityEvent OnSwitchGoals = new UnityEvent();
    public static UnityEvent OnRotateZ = new UnityEvent();
    public static UnityEvent<TeamSettings.eTeam> OnGoal = new UnityEvent<TeamSettings.eTeam>();

    public List<TeamSettings> TeamData = new List<TeamSettings>();
            
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

    public void Goal(TeamSettings.eTeam team ) {
        OnGoal?.Invoke ( team );
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

        CheckIfDisableButtonPressed();
    }
    
    private void CheckIfDisableButtonPressed()
    {
        bool anyDisableButtonPressed = Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Alpha0);

        if (anyDisableButtonPressed)
        {
            var players = GameObject.FindObjectsByType<GgjController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var player in players)
            {
                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    if (player.controlScheme == GgjController.eInputDevice.KeyboardWASD)
                        player.gameObject.SetActive(!player.gameObject.activeSelf);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    if (player.controlScheme == GgjController.eInputDevice.KeyboardArrows)
                        player.gameObject.SetActive(!player.gameObject.activeSelf);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha9) && Gamepad.all.Count > 0)
                {
                    if (player.controlScheme == GgjController.eInputDevice.ControllerOne)
                        player.gameObject.SetActive(!player.gameObject.activeSelf);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha0) && Gamepad.all.Count > 1)
                {
                    if (player.controlScheme == GgjController.eInputDevice.ControllerTwo)
                        player.gameObject.SetActive(!player.gameObject.activeSelf);
                }
            }
        }
    }
}