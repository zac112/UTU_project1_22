using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedHitbox : MonoBehaviour
{


/*
    private float lifeTime = 5.0f;
    private float projectileSpeed=250.0f;


    void Start()
    {

        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;

        //projectile gets deleted after lifeTime has passed
        Destroy(gameObject, lifeTime);

        //rotate projectile towards target
        Vector3 target = GameObject.FindWithTag("Player").transform.position;


        Vector3 hitboxPosition = transform.position;
        target.z = 0f;
        target.x = target.x - hitboxPosition.x;
        target.y = target.y - hitboxPosition.y;
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        //add velocity
        target.Normalize();
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(target*projectileSpeed);

    }

*/

}
