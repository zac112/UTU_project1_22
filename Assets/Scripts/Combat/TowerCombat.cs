using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCombat : MonoBehaviour{

    [SerializeField] float attackCooldown;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed;

    [SerializeField] EnemyStats stats;

    float lastAttackTime;
    GameObject target;

    void Update(){
        // TODO: CHANGE TO EVENTSYSTEM

        if(target) Attack();

    }



    void OnTriggerEnter2D(Collider2D other){
        if(other.TryGetComponent<EnemyCombat>(out EnemyCombat e) || other.TryGetComponent<TowerEnemy>(out TowerEnemy et)){
            target = other.gameObject;
        }

        if(other.tag=="EnemyMeleeHitbox"){
            stats.TakeDamage(10); 
            Debug.Log("got hit");
        }else if(other.tag=="EnemyRangedHitbox"){
            stats.TakeDamage(5);
            Destroy(other.gameObject); //delete projectile after getting hit
        }
    }

    void Attack(){

        if(Time.time >= lastAttackTime + attackCooldown){
            GameObject inst = Instantiate(projectile, transform.position, Quaternion.identity);

            Vector3 dir = target.transform.position - inst.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            inst.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            Rigidbody2D rb = inst.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.AddForce(inst.transform.right*projectileSpeed);

            lastAttackTime = Time.time;
        }
    }

}
