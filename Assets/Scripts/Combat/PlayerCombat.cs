using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour{

    [SerializeField] PlayerStats stats;

    private Rigidbody2D rb;
    public GameObject PlayerMeleeHitbox;
    public GameObject PlayerProjectile;
    
    private float attackCooldown = 0.7f; //seconds
    private float lastAttackedAt = 0f;


    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {

        //TODO: move this to event system
        if(stats.GetCurrentHealth()<=0f){

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

    public void MeleeAttack(){
            if(Time.time > lastAttackedAt + attackCooldown){
                lastAttackedAt = Time.time;

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
    }


    public void RangedAttack(){

            if(Time.time > lastAttackedAt + attackCooldown){
                lastAttackedAt = Time.time;

                //stop player movement
                rb.velocity = Vector2.zero;

                //spawn projectile
                Instantiate(PlayerProjectile, this.gameObject.transform.position+new Vector3 (0f, 0f, 50f), new Quaternion(0, 0, 0, 0));
            }



    }

}
