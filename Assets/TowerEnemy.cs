using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEnemy : MonoBehaviour
{

    [SerializeField] EnemyStats stats;
    [SerializeField] float speed;

    private Rigidbody2D rb;

    public GameObject hitbox;
    public List<GameObject> towers;

    private float attackCooldown = 2.0f; //seconds
    private float lastAttackedAt = 0f;

    private GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!target) {
           UpdateTarget();
        }
    }
    void FixedUpdate() 
    {
        if (!target) return;
        Vector2 position = rb.position;
        rb.position = Vector2.MoveTowards(position, target.GetComponent<Rigidbody2D>().position, speed * Time.deltaTime);
    }

    void UpdateTarget(){
        if(!target){
            float shortestDistance = Mathf.Infinity;
            foreach(GameObject t in towers){
                if (shortestDistance > Mathf.Min(shortestDistance, Vector2.Distance(this.transform.position, t.transform.position))) {
                    target = t;
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.tag=="Tower"){
            MeleeAttack();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

    switch (other.tag)
    {
        case "PlayerMeleeHitbox":
            stats.TakeDamage(20);
            break;
                    
        case "PlayerRangedHitbox":
            stats.TakeDamage(10);
            Destroy(other.gameObject);
            break;

        case "AllyMeleeHitbox":
            stats.TakeDamage(10);
            break;

        case "AllyRangedHitbox":
            Destroy(other.gameObject); //delete projectile after getting hit
            stats.TakeDamage(5);
            break;

        default:
            break;
    }

    }

    void MeleeAttack() {

            if(Time.time > lastAttackedAt + attackCooldown){

                //stop enemy movement
                rb.velocity = Vector2.zero;

                Vector3 towerEnemyPos = this.gameObject.GetComponent<Rigidbody2D>().position;
                Vector3 targetPos = target.GetComponent<Rigidbody2D>().position;
                //offset hitbox position
                Vector3 horisontalOffset = (targetPos - towerEnemyPos).normalized*1.5f;
                Vector3 verticalOffset = new Vector3 (0f, 0f, 50f);
                Vector3 hitboxPosition = towerEnemyPos + horisontalOffset + verticalOffset;

                //spawn hitbox
                Instantiate(hitbox, hitboxPosition, new Quaternion(0, 0, 0, 0));

                lastAttackedAt = Time.time;
            }
    }
}
