using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour{

    [SerializeField] PlayerStats stats;

    private int playerHP;

    void Start(){
        playerHP = stats.GetCurrentHealth();
    }



    void Update()
    {
        if(stats.GetCurrentHealth()<=0f){

            //what happens when the player dies

            Debug.Log("YOU DIED");
            Destroy(gameObject);
        }

    }


    //take damage when inside enemy hitbox
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="EnemyMeleeHitbox"){
            stats.DamagePlayer(10);
        }else if(other.tag=="EnemyRangedHitbox"){
            stats.DamagePlayer(5);
            Destroy(other.gameObject); //delete projectile after hitting player
        }
    }

}
