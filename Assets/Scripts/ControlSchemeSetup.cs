using UnityEngine;

[CreateAssetMenu(fileName = "ControlSchemeSetup", menuName = "ScriptableObjects/ControlSchemeSetup")]
public class ControlSchemeSetup : ScriptableObject
{
    public GgjController.eInputDevice playerOneControl;
    public GgjController.eInputDevice playerTwoControl;
    public GgjController.eInputDevice playerThreeControl;
    public GgjController.eInputDevice playerFourControl;

}
