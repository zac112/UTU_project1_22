using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{

    private GameObject player;
    private Rigidbody2D rb;
    private float attackRange = 2.0f;
    public GameObject hitbox;
    private float attackCooldown = 1.0f; //seconds
    private float lastAttackedAt = 0f;
    
    // HP could probably be moved to playerStats
    private float hitPoints=100f;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {

        //if the enemy is within attackRange of the player, and hasn't attacked within attackCoolDown, the enemy will attack
        if(Vector2.Distance(this.gameObject.transform.position, player.transform.position) < attackRange){
            if(Time.time > lastAttackedAt + attackCooldown){
                Attack();
                lastAttackedAt = Time.time;
            }
        }

        //enemy dies if hp reaches 0
        if(hitPoints<=0){
            Destroy(gameObject);
        }

    }


    void Attack(){

            //stop enemy movement
            rb.velocity = Vector2.zero;


            //offset hitbox position
            Vector3 horisontalOffset = (player.transform.position - this.gameObject.transform.position).normalized*3.5f;
            Vector3 verticalOffset = new Vector3 (0f, 0f, 50f);
            Vector3 hitboxPosition = this.gameObject.transform.position + horisontalOffset + verticalOffset;

            //spawn hitbox
            Instantiate(hitbox, hitboxPosition, new Quaternion(0, 0, 0, 0));
    }

}