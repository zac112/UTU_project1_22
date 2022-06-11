using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour{

    [SerializeField] PlayerStats stats;

    EnemyDifficulty diff;

    private Rigidbody2D rb;
    public GameObject PlayerMeleeHitbox;
    public GameObject PlayerProjectile;
    private GameObject difficultyController;
    
    private float attackCooldown = 0.7f; //seconds
    private float lastAttackedAt = 0f;

    public int enemyMeleeDamage;
    public int enemyRangeDamage;



    void Start(){
        rb = GetComponent<Rigidbody2D>();
        difficultyController=GameObject.Find("EnemyDifficultyController");
        diff=difficultyController.GetComponent<EnemyDifficulty>();
        UpdateDifficulty();
    }


    //call this to update difficulty
    void UpdateDifficulty(){
        //add more difficulty levels here
        switch (diff.DifficultyLevel)
        {
        case 1:
            enemyMeleeDamage = 5;
            enemyRangeDamage = 3;
            break;
        case 2:
            enemyMeleeDamage = 10;
            enemyRangeDamage = 5;
            break;
        case 3:
            enemyMeleeDamage = 15;
            enemyRangeDamage = 8;
            break;
        case 4:
            enemyMeleeDamage = 20;
            enemyRangeDamage = 10;
            break;
        case 5:
            enemyMeleeDamage = 25;
            enemyRangeDamage = 13;
            break;
        default:
            break;
        }
    }


    //take damage when inside enemy hitbox
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="EnemyMeleeHitbox"){
            stats.DamagePlayer(enemyMeleeDamage); 
        }else if(other.tag=="EnemyRangedHitbox"){
            stats.DamagePlayer(enemyRangeDamage);
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
                GameObject melee = Instantiate(PlayerMeleeHitbox, hitboxPosition, new Quaternion(0, 0, 0, 0));
                melee.transform.parent = this.gameObject.transform; //make child of player
            }
    }


    public void RangedAttack(){

            if(Time.time > lastAttackedAt + attackCooldown){
                lastAttackedAt = Time.time;

                //stop player movement
                rb.velocity = Vector2.zero;

                //spawn projectile
                GameObject proj = Instantiate(PlayerProjectile, this.gameObject.transform.position+new Vector3 (0f, 0f, 50f), new Quaternion(0, 0, 0, 0));
                proj.transform.parent = this.gameObject.transform; //make child of player
            }
    }
}

