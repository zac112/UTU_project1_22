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
    

    void Start(){
        health = startingHealth;
        maxHealth = startingHealth;
        gold = startingGold;
    }

    public void AddGold(int amount){
        gold += amount;
    }

    public void RemoveGold(int amount){
        gold -= amount;
    }

    public int GetGold(){return gold;}

    public void DamagePlayer(int amount){
        health -= amount;
        if(health<=0){
            //Die
        }
    }

    public void HealPlayer(int amount){
        health = Mathf.Clamp(0, maxHealth, health+amount);
    }

    public void AddMaxHealth(int amount){
        maxHealth += amount;
    }

    public int GetCurrentHealth(){return health;}
}
