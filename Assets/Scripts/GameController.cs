using System;
using TMPro;
using UnityEngine;
using static TeamSettings;

public class GameController : MonoBehaviour
{
    [System.Serializable]
    public struct Teamscore {
        public TeamSettings team;
        public int currentScore;
        public TextMeshProUGUI scoreDisplay;
    }

    [SerializeField]
    public Teamscore[] teamscores;

    private void Start () {
        SetTeamColors ();
    }

    private void SetTeamColors () {
        for ( int i = 0; i < teamscores.Length; i++ ) {
            teamscores [ i ].scoreDisplay.color = teamscores [ i ].team.teamColor;
        }
    }

    private void OnEnable () {
        Globals.OnGoal.AddListener ( EvaluateGoal );
    }

    private void OnDisable () {
        Globals.OnGoal.RemoveListener ( EvaluateGoal );
    }

    void EvaluateGoal( TeamSettings.eTeam team ) {
        Debug.Log ( "GOAL CONTROLLER!" );
        for ( int i = 0; i< teamscores.Length; i++ )
        {
            Debug.Log ( "SHIT " + teamscores [ i ].team.team + " vs " + team );
            if ( teamscores[ i ].team.team != team ) {
                teamscores [ i ].currentScore++;
                teamscores [ i ].scoreDisplay.text = teamscores [ i ].currentScore.ToString();
            }   
        }
    }

}
