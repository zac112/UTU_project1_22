using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GameSettings : MonoBehaviour
{
    public int gameSeed { get; set; }
    public int enemyStartingStrength { get; set; }
    public int enemyMaxStrength { get; set; }

    private static GameSettings _instance;

    public static GameSettings Instance()
    {
        if (_instance == null) {
            GameObject go = new GameObject("GameSettings");
            _instance = go.AddComponent<GameSettings>();
            _instance.enemyStartingStrength = 2;
            _instance.enemyStartingStrength = 20;
        }

        return _instance;

    }    
    
}