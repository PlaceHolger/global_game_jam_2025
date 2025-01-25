using UnityEngine;

[CreateAssetMenu(fileName = "TeamData", menuName = "ScriptableObjects/TeamSettings", order = 1)]
public class TeamSettings : ScriptableObject
{
    public enum eTeam
    {
        Team1,
        Team2,
        Team3,
        Team4
    }
    
    public eTeam team;
    public string teamName;
    public Color teamColor;
    public Material teamMaterial;
}