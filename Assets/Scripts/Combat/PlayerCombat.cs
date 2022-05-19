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
        if(other.tag=="EnemyHitbox"){
            stats.DamagePlayer(10);
            playerHP-=10;
            Debug.Log(playerHP + " HP");
        }
    }

}
