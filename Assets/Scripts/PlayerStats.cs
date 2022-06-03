using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int startingHealth;
    [SerializeField] int startingGold;

    int maxHealth;
    int health;
    int gold;

    GameEvents events;  // game's event system
           
    void Start(){
        health = startingHealth;
        maxHealth = startingHealth;
        gold = startingGold;
        GameStats.Gold = startingGold;
        events = gameObject.GetComponent<GameEvents>();

        UIManager.current.UpdateGoldText(gold);
        UIManager.current.UpdateHealthText(health);
    }

    public void AddGold(int amount){
        gold += amount;
        GameStats.Gold += amount;
        UIManager.current.UpdateGoldText(gold);
    }

    public void RemoveGold(int amount){
        gold -= amount;
        GameStats.Gold -= amount;
        UIManager.current.UpdateGoldText(gold);
    }

    public int GetGold(){return gold;}

    public void DamagePlayer(int amount){
        health -= amount;
        UIManager.current.UpdateHealthText(health);
        UIManager.current.UpdateHealthIcon(((float)health/(float)maxHealth));

        //what happens when player dies
        if(health<=0){
            Destroy(gameObject);
            Debug.Log("YOU DIED");

            if (events != null)  // if clause to prevent bugs for cases where event system is not yet plugged into the scene
            {
                events.OnGameOver(GameOverType.PlayerDied);  // launch game over screen through event system
            }

        }
    }

    public void HealPlayer(int amount){
        health = Mathf.Clamp(0, maxHealth, health+amount);
        UIManager.current.UpdateHealthText(health);
        UIManager.current.UpdateHealthIcon(((float)health/(float)maxHealth));
    }

    public void AddMaxHealth(int amount){
        maxHealth += amount;
    }

    public int GetCurrentHealth(){return health;}
}
