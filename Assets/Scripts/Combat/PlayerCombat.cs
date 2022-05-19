using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    private float playerHP = 100f;

    void Start()
    {
        
    }



    void Update()
    {
        if(playerHP<=0f){

            //what happens when the player dies

            Debug.Log("YOU DIED");
            Destroy(gameObject);
        }

    }


    //take damage when inside enemy hitbox
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="EnemyHitbox"){
            playerHP=playerHP-10f;
            Debug.Log(playerHP + " HP");
        }
    }

}
