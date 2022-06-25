using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform endCanvas = GameObject.Find("EndStatsCanvas").transform;
        TMP_Text enemiesKilled = endCanvas.Find("EnemiesKilled").GetComponent<TMP_Text>();
        enemiesKilled.text = GameStats.EnemiesKilled.ToString();
        TMP_Text buildingsDestroyed = endCanvas.Find("BuildingsDestroyed").GetComponent<TMP_Text>();
        buildingsDestroyed.text = GameStats.BuildingsDestroyed.ToString();
        TMP_Text endGold = endCanvas.Find("EndGold").GetComponent<TMP_Text>();
        endGold.text = GameStats.Gold.ToString();
        TMP_Text ownBuildings = endCanvas.Find("OwnBuildings").GetComponent<TMP_Text>();
        ownBuildings.text = GameStats.BuildingsBuilt.ToString();
        TMP_Text gameDuration = endCanvas.Find("GameDuration").GetComponent<TMP_Text>();
        gameDuration.text = GameStats.GameDuration.ToString();

        TMP_Text gameOverTypeText = endCanvas.Find("GameOverTypeText").GetComponent<TMP_Text>();

        if (GameStats.GameOverReason == GameOverType.Victory)
        {
            gameOverTypeText.text = "Victory!";
        }
        if (GameStats.GameOverReason == GameOverType.PlayerDied)
        {
            gameOverTypeText.text = "You died!";
        }
        if (GameStats.GameOverReason == GameOverType.OwnVillagesDestroyed)
        {
            gameOverTypeText.text = "Own villages destroyed!";
        }
    }

}
