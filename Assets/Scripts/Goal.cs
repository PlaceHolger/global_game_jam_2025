using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private string BallTag = "Ball";
    
    public TeamSettings currentTeam;

    public float RotateOnGoalChance = 0.5f;

    private void OnEnable()
    {
        Globals.OnSwitchGoals.AddListener(OnSwitchGoals);
        Globals.OnRotateZ.AddListener(OnSwitchGoals);
        UpdateColors();
    }
    
    private void OnDisable()
    {
        Globals.OnSwitchGoals.RemoveListener(OnSwitchGoals);
        Globals.OnRotateZ.RemoveListener(OnSwitchGoals);
    }

    private void OnSwitchGoals()
    {
        var newTeam = Globals.GetInstance().GetOtherTeam(currentTeam.team);
        if (newTeam)
        {
            currentTeam = newTeam;
            UpdateColors();
        }
    }

    private void UpdateColors()
    {
        var allRenderer = GetComponentsInChildren<Renderer>();
        foreach (var renderer in allRenderer)
        {
            if(renderer.material.HasColor("_Color"))
                renderer.material.color = new Color(currentTeam.teamColor.r, currentTeam.teamColor.g, currentTeam.teamColor.b, renderer.material.color.a); 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(BallTag))
        {
           // Debug.Log ( "GOAL!" );
            Globals.GetInstance ().Goal ( currentTeam.team );
            Globals.GetInstance().ResetAfterGoal(); 
            
            if (Random.value < RotateOnGoalChance)
            {
                FieldMover2 fieldMover = FindAnyObjectByType<FieldMover2>();
                if (fieldMover)
                {
                    if(currentTeam.team == TeamSettings.eTeam.Team1)
                    {
                        fieldMover.RotateZ(true);
                    }
                    else
                    {
                        fieldMover.RotateZ(false);
                    }
                }
            }
        }
    }
}
