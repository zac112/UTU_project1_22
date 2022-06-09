using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEvents : MonoBehaviour
{
    // Current event manager used by the game
    // I'm not entirely sure why this is here but the tutorial had this
    public static GameEvents current;

    // Set the current event manager to this instance of the GameEvents class
    private void Awake()
    {
        current = this;
    }

    public event Action<GameObject> BuildingSelectedForBuilding;
    public void OnBuildingSelected(GameObject building){ BuildingSelectedForBuilding?.Invoke(building); }

    public event Action<Vector3,int> MapChange;
    public void OnMapChanged(Vector3 worldPos, int size){ MapChange?.Invoke(worldPos, size); }  

    public event Action<int> Tick;
    public void OnTick(int currentTick){ Tick?.Invoke(currentTick); }

    public event Action<Vector2, Vector2> MovementInputChanged;
    public void OnMovementInputChanged(Vector2 input, Vector2 delta){ MovementInputChanged?.Invoke(input, delta); }

    public event Action<Vector3, Vector3> MouseMoved;
    public void OnMouseMoved(Vector3 position, Vector3 delta) { MouseMoved?.Invoke(position, delta); }

    public event Action<GameOverType> GameOver;
    public void OnGameOver(GameOverType t)
    {
        GameStats.CollectEndStats();
        SceneManager.LoadScene("GameOverScene");
        // GameObject EndCanvas = GameObject.Find("EndStatsCanvas");
        TextMesh EnemiesKilled = GameObject.Find("EnemiesKilled").GetComponent<TextMesh>();
        EnemiesKilled.text = GameStats.EnemiesKilled.ToString();
        TextMesh BuildingsDestroyed = GameObject.Find("BuildingsDestroyed").GetComponent<TextMesh>();
        BuildingsDestroyed.text = GameStats.BuildingsDestroyed.ToString();
        TextMesh EndGold = GameObject.Find("EndGold").GetComponent<TextMesh>();
        EndGold.text = GameStats.Gold.ToString();
        TextMesh OwnBuildings = GameObject.Find("OwndBuildings").GetComponent<TextMesh>();
        OwnBuildings.text = GameStats.FriendlyBuildingsCount.ToString();
        TextMesh GameDuration = GameObject.Find("GameDuration").GetComponent<TextMesh>();
        GameDuration.text = GameStats.GameDuration.ToString();

        TextMesh GameOverTypeText = GameObject.Find("GameOverTypeText").GetComponent<TextMesh>();

        if (t == GameOverType.Victory)
        {
            GameOverTypeText.text = "Victory!";
        }
        if (t == GameOverType.PlayerDied)
        {
            GameOverTypeText.text = "You died!";
        }
        if (t == GameOverType.OwnVillagesDestroyed)
        {
            GameOverTypeText.text = "Own villages destroyed!";
        }
    }


}
