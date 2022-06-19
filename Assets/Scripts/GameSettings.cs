using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GameSettings : MonoBehaviour
{
    public int gameSeed { get; set; }
    public int enemyStartingStrength { get; set; }
    public int enemyMaxStrength { get; set; }
<<<<<<< HEAD
=======
    public NetworkMode networkMode { get; set; }
>>>>>>> networking_wip

    private static GameSettings _instance;

    public static GameSettings Instance()
    {
        if (_instance == null) {
            GameObject go = new GameObject("GameSettings");
<<<<<<< HEAD
            _instance = go.AddComponent<GameSettings>();
            _instance.enemyStartingStrength = 2;
            _instance.enemyStartingStrength = 20;
=======
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<GameSettings>();
            _instance.enemyStartingStrength = 2;
            _instance.enemyStartingStrength = 20;
            _instance.networkMode = NetworkMode.Offline;
>>>>>>> networking_wip
        }

        return _instance;

    }    
    
}