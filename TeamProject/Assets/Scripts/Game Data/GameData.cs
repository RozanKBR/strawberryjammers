#region
using UnityEngine;
using System.Collections;
#endregion


public class GameData
{
    private PlayerStats m_player_saved_stats;
    public PlayerStats PlayerSavedStats { get { return m_player_saved_stats; } }
}


public struct PlayerStats
{
    public float MaxHealth;
    public float CurrentHealth;

    public float Attack;
    public float Defense;
}

