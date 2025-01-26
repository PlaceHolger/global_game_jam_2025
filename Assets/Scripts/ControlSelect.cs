using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlSelect : MonoBehaviour
{
    [SerializeField]
    private ControlSchemeSetup _controlSetup;

    public void SetPlayerOne(Int32 index ) {
        _controlSetup.playerOneControl = (GgjController.eInputDevice)index;
    }

    public void SetPlayerTwo ( Int32 index ) {
        _controlSetup.playerTwoControl = ( GgjController.eInputDevice ) index;
    }

    public void SetPlayerThree ( Int32 index ) {
        _controlSetup.playerThreeControl = ( GgjController.eInputDevice ) index;
    }

    public void SetPlayerFour ( Int32 index ) {
        _controlSetup.playerFourControl = ( GgjController.eInputDevice ) index;
    }

    public void StartTheGame () {
        SceneManager.LoadScene ( 1 );
    }
}
