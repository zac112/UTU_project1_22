using EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Keeps track of game statistics
/// </summary>
public static class GameStats
{
    /// <summary>
    /// How many enemies killed during the game
    /// </summary>
    public static int EnemiesKilled { get; set; }

    /// <summary>
    /// How many buildings built during the game
    /// </summary>
    public static int BuildingsBuilt { get; set; }
    
    /// <summary>
    /// How much gold
    /// </summary>
    public static int Gold { get; set; }

    /// <summary>
    /// How many buildings destroyed during the game
    /// </summary>
    public static int BuildingsDestroyed { get; set; }

    /// <summary>
    /// Game duration in seconds
    /// </summary>
    public static float GameDuration { get; set; }

    public static void CollectEndStats()
    {
        GameObject ges = GameObject.Find("Game Event System");   // assumes that the GES will be named this in the game main scene
        if (ges != null)
        {
            Tick t = ges.GetComponent<Tick>();
            GameDuration = (float)t.GetCurrentTick() / (float)t.GetTickSpeed();
        }
    }

}
