using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{

    [SerializeField] EnemyStats stats;


    private GameObject player;
    private Rigidbody2D rb;
    public GameObject hitbox;
    public GameObject enemyProjectile;
    private float attackCooldown = 2.0f; //seconds
    private float lastAttackedAt = 0f;



    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    void MeleeAttack(){

            if(Time.time > lastAttackedAt + attackCooldown){

                //stop enemy movement
                rb.velocity = Vector2.zero;

                //offset hitbox position
                Vector3 horisontalOffset = (player.transform.position - this.gameObject.transform.position).normalized*3.5f;
                Vector3 verticalOffset = new Vector3 (0f, 0f, 50f);
                Vector3 hitboxPosition = this.gameObject.transform.position + horisontalOffset + verticalOffset;

                //spawn hitbox
                Instantiate(hitbox, hitboxPosition, new Quaternion(0, 0, 0, 0));

                lastAttackedAt = Time.time;
            }


    }

    void RangedAttack(){

            if(Time.time > lastAttackedAt + attackCooldown){
                //stop enemy movement
                rb.velocity = Vector2.zero;

                //spawn projectile
                Instantiate(enemyProjectile, this.gameObject.transform.position+new Vector3 (0f, 0f, 50f), new Quaternion(0, 0, 0, 0));
                lastAttackedAt = Time.time;
            }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="PlayerMeleeHitbox"){
            stats.TakeDamage(20);
        } else if(other.tag=="PlayerRangedHitbox"){
            Destroy(other.gameObject); //delete projectile after getting hit
            stats.TakeDamage(10);
        }

    }


   void OnTriggerStay2D(Collider2D other){

        if(other.tag=="EnemyRangedRange"&&Vector3.Distance((other.gameObject.transform.position-new Vector3(0f,0f,5f)), this.transform.position)>=2.5f){
            RangedAttack();
        }else if(other.tag=="EnemyMeleeRange"){
            MeleeAttack();
        }
    }
    

}