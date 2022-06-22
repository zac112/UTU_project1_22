using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class PlayerStats : NetworkBehaviour, IDataManager
{
    [SerializeField] int startingHealth;
    [SerializeField] int startingGold;
    [SerializeField] int startingWood;

    NetworkVariable<int> networkMaxHealth = new NetworkVariable<int>();
    NetworkVariable<int> networkHealth = new NetworkVariable<int>();
    NetworkVariable<int> networkGold = new NetworkVariable<int>();
    NetworkVariable<int> networkWood = new NetworkVariable<int>();

    int health => networkHealth.Value;
    int gold => networkGold.Value;
    int wood => networkWood.Value;
    int maxHealth => networkMaxHealth.Value;

    List<Village> FriendlyVillages = new List<Village>();
    Village CurrentVillage;  // currently active village
    private PlayerSkills playerSkills;
    [SerializeField] WASDMovement movement;
    [SerializeField] FogRevealer fogRevealer;
    [SerializeField] GameObject goldMine;
        
           
    void Start(){
        SetHealthServerRpc(startingHealth);
        SetMaxHealthServerRpc(startingHealth);
        SetGoldServerRpc(startingGold);
        SetWoodServerRpc(startingWood);
        GameStats.Gold = startingGold;
        GameStats.Wood = startingWood;
        GameStats.FriendlyVillages = FriendlyVillages;  // initialize player's villages
        CurrentVillage = new Village();
        FriendlyVillages.Add(CurrentVillage);

        UIManager.current.UpdateGoldText(gold);
        UIManager.current.UpdateWoodText(wood);
        UIManager.current.UpdateHealthText(health);

        playerSkills = new PlayerSkills();

        GameEvents.current.OnSkillUnlockedEvent += OnSkillUnlocked;
        if (NetworkObject.IsLocalPlayer)
        {
            networkMaxHealth.OnValueChanged += OnMaxHealthChange;
            networkHealth.OnValueChanged += OnHealthChange;
            networkGold.OnValueChanged += OnGoldChange;
            networkWood.OnValueChanged += OnWoodChange;
        }

    }

    void OnMaxHealthChange(int oldVal, int newVal)
    {
        UIManager.current.UpdateHealthText(health);
        UIManager.current.UpdateHealthIcon(((float)health / (float)newVal));
    }
    void OnHealthChange(int oldVal, int newVal) {
        UIManager.current.UpdateHealthText(health);
        UIManager.current.UpdateHealthIcon(((float)newVal / (float)maxHealth));
    }
    void OnGoldChange(int oldVal, int newVal)
    {
        GameStats.Gold = newVal;
        UIManager.current.UpdateGoldText(newVal);
    }

    void OnWoodChange(int oldVal, int newVal)
    {
        GameStats.Wood = newVal;
        UIManager.current.UpdateWoodText(newVal);
    }

    public void LoadData(GameData data){
        this.networkGold.Value = data.gold;
    }

    public void SaveData(GameData data){
        data.gold = this.gold;
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

     public PlayerSkills GetPlayerSkills() {
        return playerSkills;
    }

    private void AttemptPurchase(Technology tech) {
        if (gold > tech.cost)
        {
            RemoveGold(tech.cost);
            GameEvents.current.OnTechnologyUnlock(tech.unlocksPreq);
        }
    }

    public void AddGold(int amount){
        SetGoldServerRpc(gold+amount);        
    }
    public void RemoveGold(int amount){
        SetGoldServerRpc(gold-amount);
    }

    public void AddWood(int amount){
        SetWoodServerRpc(wood+amount);        
    }
    public void RemoveWood(int amount){
        SetWoodServerRpc(wood-amount);
    }



    public int GetGold(){return gold;}

    public void DamagePlayer(int amount){
        SetHealthServerRpc(health - amount);

        //what happens when player dies
        if(health<=0){
            Destroy(gameObject);
            Debug.Log("YOU DIED");

            GameEvents.current.OnGameOver(GameOverType.PlayerDied);  // launch game over screen through event system

        }
    }

    public void HealPlayer(int amount){
        SetHealthServerRpc(Mathf.Clamp(0, maxHealth, health+amount));
    }

    [ServerRpc(RequireOwnership = false)]
    void SetHealthServerRpc(int health) {
        networkHealth.Value = health;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetMaxHealthServerRpc(int amount){
        networkMaxHealth.Value = amount;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetGoldServerRpc(int amount)
    {
        networkGold.Value = amount;
    }

    [ServerRpc(RequireOwnership = false)]
    void SetWoodServerRpc(int amount) {
        networkWood.Value = amount;
    }
    public void AddMaxHealth(int amount)
    {
        SetMaxHealthServerRpc(maxHealth+amount);
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
    public void OnSkillUnlocked(PlayerSkills.SkillType skillType) {
        switch (skillType) {
            case PlayerSkills.SkillType.HealthMax_1:
                AddMaxHealth(25);
                break;
            case PlayerSkills.SkillType.HealthMax_2:
                AddMaxHealth(15);
                break;
            case PlayerSkills.SkillType.HealthMax_3:
                AddMaxHealth(10);
                break;
            case PlayerSkills.SkillType.MoveSpeed_1:
                movement.setSpeed(3.5f);
                break;
            case PlayerSkills.SkillType.MoveSpeed_2:
                movement.setSpeed(4f);
                break;
            case PlayerSkills.SkillType.FogReveal_1:
                fogRevealer.GetComponent<BoxCollider2D>().size = new Vector2(2f, 2f);
                break;
            case PlayerSkills.SkillType.FogReveal_2:
                fogRevealer.GetComponent<BoxCollider2D>().size = new Vector2(3f, 3f);
                break;
            case PlayerSkills.SkillType.GoldMine_1:
                goldMine.GetComponent<GoldMineScript>().AddMiningSpeed(10);
                break;
            case PlayerSkills.SkillType.GoldMine_2:
                goldMine.GetComponent<GoldMineScript>().AddMiningSpeed(10);
                break;
        }
    }
     
}
