using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int startingHealth;
    [SerializeField] int startingGold;
    [SerializeField] UIManager UIManager;

    int maxHealth;
    int health;
    int gold;

    GameEvents events;  // game's event system
    List<Village> FriendlyVillages;
    Village CurrentVillage;  // currently active village
           
    void Start(){
        health = startingHealth;
        maxHealth = startingHealth;
        gold = startingGold;
        GameStats.Gold = startingGold;
        events = gameObject.GetComponent<GameEvents>();
        GameStats.FriendlyVillages = FriendlyVillages;  // initialize player's villages
        CurrentVillage = new Village();
        FriendlyVillages.Add(CurrentVillage);

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

    public void RemoveVillage (int village_id)
        // TODO: Build separately mechanics to remove village when all buildings inside it are destroyed
    {
        Village v = FriendlyVillages.Find(village => village.ID == village_id);
        FriendlyVillages.Remove(v);
        GameStats.FriendlyVillages.Remove(v);
        Destroy(v);
        if (FriendlyVillages.Count <= 0)
        {
            events.OnGameOver(GameOverType.OwnVillagesDestroyed);  // trigger game over if all villages destroyed
        } else
        {
            CurrentVillage = FriendlyVillages[0];  // do something more sophisticated maybe later... the village closest to camera should become current?
        }
    }

    public void AddVillage()
    {
        Village v = new Village();
        CurrentVillage = v;
        FriendlyVillages.Add(v);
        GameStats.FriendlyVillages.Add(v);
    }

    public void AddVillage(Village village)
    {
        CurrentVillage = village;
        FriendlyVillages.Add(village);
        GameStats.FriendlyVillages.Add(village);
    }

}
