using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private string BallTag = "Ball";
    
    public UnityEvent OnGoal;
    
    public TeamSettings currentTeam;

    private void OnEnable()
    {
        Globals.OnSwitchGoals.AddListener(OnSwitchGoals);
        UpdateColors();
    }
    
    private void OnDisable()
    {
        Globals.OnSwitchGoals.RemoveListener(OnSwitchGoals);
    }

    private void OnSwitchGoals()
    {
        currentTeam = Globals.GetInstance().GetOtherTeam(currentTeam.team);
        UpdateColors();
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
            Globals.GetInstance().ResetAfterGoal();
            OnGoal.Invoke();
        }
    }
}
