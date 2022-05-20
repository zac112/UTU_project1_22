using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{

    private GameObject player;
    private Rigidbody2D rb;
    private float meleeAttackRange = 2.5f;
    private float rangedAttackRange = 15.0f;    
    public GameObject hitbox;
    public GameObject enemyProjectile;
    private float attackCooldown = 2.0f; //seconds
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

        //if the enemy is within meleeAttackRange of the player, and hasn't attacked within attackCoolDown, the enemy will do a melee attack
        if(Vector2.Distance(this.gameObject.transform.position, player.transform.position) < meleeAttackRange){
            if(Time.time > lastAttackedAt + attackCooldown){
                MeleeAttack();
                lastAttackedAt = Time.time;
            }
            //if the enemy is within rangedAttackRange of the player, and hasn't attacked within attackCoolDown, the enemy will do a ranged attack
        }else if(Vector2.Distance(this.gameObject.transform.position, player.transform.position) < rangedAttackRange){
            if(Time.time > lastAttackedAt + attackCooldown){
                RangedAttack();
                lastAttackedAt = Time.time;
            }
        }

        //enemy dies if hp reaches 0
        if(hitPoints<=0){
            Destroy(gameObject);
        }

    }


    void MeleeAttack(){

            //stop enemy movement
            rb.velocity = Vector2.zero;

            //offset hitbox position
            Vector3 horisontalOffset = (player.transform.position - this.gameObject.transform.position).normalized*3.5f;
            Vector3 verticalOffset = new Vector3 (0f, 0f, 50f);
            Vector3 hitboxPosition = this.gameObject.transform.position + horisontalOffset + verticalOffset;

            //spawn hitbox
            Instantiate(hitbox, hitboxPosition, new Quaternion(0, 0, 0, 0));
    }

    void RangedAttack(){

            //stop enemy movement
            rb.velocity = Vector2.zero;

            //spawn projectile
            Instantiate(enemyProjectile, this.gameObject.transform.position+new Vector3 (0f, 0f, 50f), new Quaternion(0, 0, 0, 0));

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="PlayerMeleeHitbox"){
            hitPoints-=20;
        }else if(other.tag=="PlayerRangedHitbox"){
            hitPoints-=10;
            Destroy(other.gameObject); //delete projectile after getting hit
        }
    }

}