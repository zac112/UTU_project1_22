using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyCombat : MonoBehaviour
{

    [SerializeField] AllyStats stats;
    [SerializeField] AIPathfinding ai;


    private Rigidbody2D rb;
    public GameObject hitbox;
    public GameObject projectile;
    private float attackCooldown = 2.0f; //seconds
    private float lastAttackedAt = 0f;

    private float distance;

    GameObject target;
    private GameObject player;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }





    // If a target is destroyed, the target will become null and cause errors. This function will set itself as the target if it is null.
    // because Destroy() destroys gameobjects at the end of the frame, we have to wait until the next frame to reset the AI target. 
    IEnumerator UpdateTarget(){
        yield return 0; // wait 1 frame
        if(ai.getTarget()==null){
            ai.setTarget(this.transform);
        }
    }



    void MeleeAttack(){

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


    void RangedAttack(){
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
        if(other.tag=="EnemyMeleeHitbox"){
            stats.TakeDamage(10); 
        }else if(other.tag=="EnemyRangedHitbox"){
            stats.TakeDamage(5);
            Destroy(other.gameObject); //delete projectile after hitting player
        }
    }


   void OnTriggerStay2D(Collider2D other){
       
        distance = Vector3.Distance((other.gameObject.transform.position-new Vector3(0f,0f,5f)), this.transform.position);
        if(other.tag=="AllyRangedRange"&&distance>=2.5f){
            target = other.gameObject;
            ai.setTarget(target.transform); //set enemy as target when enemy is inside AllyRangedRange
            RangedAttack();
        }else if(other.tag=="AllyMeleeRange"){
            target = other.gameObject;
            ai.setTarget(target.transform);
            MeleeAttack();
        }
    }


   void OnTriggerExit2D(Collider2D other){
        if(other.tag=="AllyMeleeRange"){
            StartCoroutine(UpdateTarget());
        } else if(other.tag=="AllyRangedRange"){
            StartCoroutine(UpdateTarget());
        }
    }

}