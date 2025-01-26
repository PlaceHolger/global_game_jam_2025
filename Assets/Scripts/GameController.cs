using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private float _matchTime = 180f;

    [SerializeField]
    private float _finishTime = 5f;

    [SerializeField]
    public Teamscore[] teamscores;

    [SerializeField]
    private ControlSchemeSetup _controlSetup;

    [SerializeField]
    private GgjController _playerOne;

    [SerializeField]
    private GgjController _playerTwo;

    [SerializeField]
    private GgjController _playerThree;

    [SerializeField]
    private GgjController _playerFour;

    [SerializeField]
    private TextMeshProUGUI _timeDisplay;


    private void Start () {
        SetTeamColors ();
        SetPlayerControls ();
    }

    private void SetTeamColors () {
        for ( int i = 0; i < teamscores.Length; i++ ) {
            teamscores [ i ].scoreDisplay.color = teamscores [ i ].team.teamColor;
        }
    }

    private void SetPlayerControls () {
        _playerOne.controlScheme = _controlSetup.playerOneControl;
        _playerOne.gameObject.SetActive(_playerOne.CheckControlAvailability ());
        _playerTwo.controlScheme = _controlSetup.playerTwoControl;
        _playerTwo.gameObject.SetActive ( _playerTwo.CheckControlAvailability ());
        _playerThree.controlScheme = _controlSetup.playerThreeControl;
        _playerThree.gameObject.SetActive ( _playerThree.CheckControlAvailability ());
        _playerFour.controlScheme = _controlSetup.playerFourControl;
        _playerFour.gameObject.SetActive ( _playerFour.CheckControlAvailability ());
    }

    private void OnEnable () {
        Globals.OnGoal.AddListener ( EvaluateGoal );
    }

    private void OnDisable () {
        Globals.OnGoal.RemoveListener ( EvaluateGoal );
    }

    void EvaluateGoal( TeamSettings.eTeam team ) {
        for ( int i = 0; i< teamscores.Length; i++ )
        {
            if ( teamscores[ i ].team.team != team ) {
                teamscores [ i ].currentScore++;
                teamscores [ i ].scoreDisplay.text = teamscores [ i ].currentScore.ToString();
            }   
        }
    }

    private void Update () {
        if ( _matchTime > 0f ) {
            _matchTime -= Time.deltaTime;
            if ( _matchTime > 0f ) {
                _timeDisplay.text = "Time : " + Mathf.CeilToInt ( _matchTime );
            } else {
                _timeDisplay.text = "Game End!";
                Invoke ( nameof(BackToMenu), _finishTime ); 
            }
        }
    }

    public void BackToMenu () {
        SceneManager.LoadScene ( 0 );
    }

}
