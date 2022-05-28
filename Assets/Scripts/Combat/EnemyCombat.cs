using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{

    [SerializeField] EnemyStats stats;
    [SerializeField] AIPathfinding ai;


    private GameObject player;
    private Rigidbody2D rb;
    public GameObject hitbox;
    public GameObject projectile;

    private GameObject target;
    
    private float attackCooldown = 2.0f; //seconds
    private float lastAttackedAt = 0f;

    private bool MeleeAggro;

    private float distance;


    void Start()
    {
        MeleeAggro=false;
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }



   void Update()
    {

        //If an ally is destroyed, the target will become null and cause errors. This statement will set the player as the target when if it is null.
        //TODO: move this somewhere else
        if(ai.getTarget()==null){
            ai.setTarget(player.transform);
        }
    }





    void MeleeAttack(GameObject target){

            if(Time.time > lastAttackedAt + attackCooldown){

                //stop enemy movement
                rb.velocity = Vector2.zero;

                //offset hitbox position
                Vector3 horisontalOffset = (target.transform.position - this.gameObject.transform.position).normalized*3.5f;
                Vector3 verticalOffset = new Vector3 (0f, 0f, 50f);
                Vector3 hitboxPosition = this.gameObject.transform.position + horisontalOffset + verticalOffset;

                //spawn hitbox
                Instantiate(hitbox, hitboxPosition, new Quaternion(0, 0, 0, 0));

                lastAttackedAt = Time.time;
            }


    }

    void RangedAttack(GameObject target){

        if(Time.time >= lastAttackedAt + attackCooldown){
            Vector3 verticalOffset = new Vector3 (0f, 0f, 50f);

            GameObject inst = Instantiate(projectile, transform.position+verticalOffset, Quaternion.identity);

            Vector3 dir = target.transform.position - inst.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            inst.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            Rigidbody2D rb = inst.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.AddForce(inst.transform.right*250.0f);

            lastAttackedAt = Time.time;
        }

    }



    void OnTriggerEnter2D(Collider2D other)
    {

    switch (other.tag)
    {
        case "PlayerMeleeHitbox":
            stats.TakeDamage(20);
            break;

        case "AllyMeleeHitbox":
            stats.TakeDamage(10);
            break;

        case "PlayerRangedHitbox":
            Destroy(other.gameObject); //delete projectile after getting hit
            stats.TakeDamage(10);
            break;

        case "AllyRangedHitbox":
            Destroy(other.gameObject); //delete projectile after getting hit
            stats.TakeDamage(5);
            break;

        case "EnemyMeleeRange":
            target = other.gameObject;
            ai.setTarget(target.transform); //set AIPathinding target to whatever enters aggro range
            MeleeAggro = true; // MeleeAggro boolean prevents enemy from doing ranged attacks if within melee range.
            break;

    /*  
        case "EnemyRangedRange":
            if(!MeleeAggro){
            target = other.gameObject;
            ai.setTarget(target.transform); //set AIPathinding target to whatever enters aggro range
            }
            break; 
    */

        default:
            break;
    }

    }


   void OnTriggerStay2D(Collider2D other){
       
        distance = Vector3.Distance((other.gameObject.transform.position-new Vector3(0f,0f,5f)), this.transform.position);
        if(other.tag=="EnemyRangedRange"&&distance>=2.5f&&!MeleeAggro){
            target = other.gameObject;
            ai.setTarget(target.transform);
            RangedAttack(target);
        }else if(other.tag=="EnemyMeleeRange"){
            MeleeAttack(target);
        }
    }


    void OnTriggerExit2D(Collider2D other){
        if(other.tag=="EnemyMeleeRange"){
            MeleeAggro=false;
        }
    }
    

}