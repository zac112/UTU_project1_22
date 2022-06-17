using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public int health;
    public int gold;

    // Default values when a new game is created
    public GameData()
    {
        this.gold = 0;
        this.health = 100;
        
    }

}
