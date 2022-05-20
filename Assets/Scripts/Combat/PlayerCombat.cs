using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour{

    [SerializeField] PlayerStats stats;

    private int playerHP;
    private string MeleeButton = "1";
    private string RangedButton = "2";
    private Rigidbody2D rb;
    public GameObject PlayerMeleeHitbox;
    private float attackCooldown = 0.7f; //seconds
    private float lastAttackedAt = 0f;


    void Start(){
        playerHP = stats.GetCurrentHealth();
        rb = GetComponent<Rigidbody2D>();
    }



    void Update()
    {
        if(stats.GetCurrentHealth()<=0f){

            //what happens when the player dies
            Debug.Log("YOU DIED");
            Destroy(gameObject);
        }

        if (Input.GetKeyDown(MeleeButton)){
            if(Time.time > lastAttackedAt + attackCooldown){
                MeleeAttack();
                lastAttackedAt = Time.time;
            }
        }
        if (Input.GetKeyDown(RangedButton)){
            if(Time.time > lastAttackedAt + attackCooldown){
                RangedAttack();
                lastAttackedAt = Time.time;
            }
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

    void MeleeAttack(){
            //stop player movement
            rb.velocity = Vector2.zero;


            //offset hitbox position towards mouse
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 dir = Input.mousePosition - pos;

            Vector3 horisontalOffset = dir.normalized;
            Vector3 verticalOffset = new Vector3 (0f, 0f, 50f);
            Vector3 hitboxPosition = this.gameObject.transform.position + horisontalOffset + verticalOffset;

            //spawn hitbox
            Instantiate(PlayerMeleeHitbox, hitboxPosition, new Quaternion(0, 0, 0, 0));
    }

    void RangedAttack(){

    }

}
