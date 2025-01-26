using UnityEngine;

[CreateAssetMenu(fileName = "TeamData", menuName = "ScriptableObjects/TeamSettings", order = 1)]
public class TeamSettings : ScriptableObject
{
    [System.Serializable]
    public enum eTeam
    {
        Unknown = 0,
        Team1 = 1,
        Team2 = 2,
        Team3 = 3,
        Team4 = 4
    }
    
    public eTeam team;
    public string teamName;
    public Color teamColor;
    public Material teamMaterial;
}