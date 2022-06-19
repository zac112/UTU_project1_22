using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyStats : NetworkBehaviour
{

    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth;

    [SerializeField] HealthBar healthBar;
    
    void Init()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        //what happens when enemy dies
        //maybe enemy dying should give xp/score or resources
        if(currentHealth<=0){
            if (IsServer || NetworkManager.Singleton.IsHost) GetComponent<NetworkObject>().Despawn();
            GameStats.EnemiesKilled++;
        }
    }

    public void SetMaxHP(int HP){
        maxHealth = HP;
        Init();
    }

}
