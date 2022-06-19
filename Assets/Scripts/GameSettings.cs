using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GameSettings : MonoBehaviour
{
    public int gameSeed { get; set; }
    public int enemyStartingStrength { get; set; }
    public int enemyMaxStrength { get; set; }
    public NetworkMode networkMode { get; set; }

    private static GameSettings _instance;

    public static GameSettings Instance()
    {
        if (_instance == null) {
            GameObject go = new GameObject("GameSettings");
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<GameSettings>();
            _instance.enemyStartingStrength = 2;
            _instance.enemyStartingStrength = 20;
            _instance.networkMode = NetworkMode.Offline;
        }

        return _instance;

    }    
    
}