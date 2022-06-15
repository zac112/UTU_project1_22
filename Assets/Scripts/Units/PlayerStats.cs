using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int startingHealth;
    [SerializeField] int startingGold;

    int maxHealth;
    int health;
    int gold;
    
    List<Village> FriendlyVillages = new List<Village>();
    Village CurrentVillage;  // currently active village
       
           
    void Start(){
        health = startingHealth;
        maxHealth = startingHealth;
        gold = startingGold;
        GameStats.Gold = startingGold;
        GameStats.FriendlyVillages = FriendlyVillages;  // initialize player's villages
        CurrentVillage = new Village();
        FriendlyVillages.Add(CurrentVillage);

        UIManager.current.UpdateGoldText(gold);
        UIManager.current.UpdateHealthText(health);

        
    }

    private void OnEnable()
    {
        GameEvents.current.GameOver += OnGameOver;
        GameEvents.current.TechnologyPurchaseAttempt += AttemptPurchase;
    }

    private void OnDisable()
    {
        GameEvents.current.GameOver -= OnGameOver;
        GameEvents.current.TechnologyPurchaseAttempt -= AttemptPurchase;
    }

    private void AttemptPurchase(Technology tech) {
        if (gold > tech.cost)
        {
            gold -= tech.cost;
            GameEvents.current.OnTechnologyUnlock(tech.unlocksPreq);
        }
    }

    public void AddGold(int amount){
        gold += amount;
        GameStats.Gold += amount;
        UIManager.current.UpdateGoldText(gold);
    }
    public void SetGold(int amount)
    {
        gold = amount;
        GameStats.Gold = amount;
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

            GameEvents.current.OnGameOver(GameOverType.PlayerDied);  // launch game over screen through event system

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
        
        if (FriendlyVillages.Count <= 0)
        {
            GameEvents.current.OnGameOver(GameOverType.OwnVillagesDestroyed);  // trigger game over if all villages destroyed
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
