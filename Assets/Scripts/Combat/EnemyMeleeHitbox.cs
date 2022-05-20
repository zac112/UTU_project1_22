using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeHitbox : MonoBehaviour
{

    private float lifeTime = 0.1f;

    void Start()
    {

        //hitbox gets deleted after lifeTime has passed
        Destroy(gameObject, lifeTime);


        //rotate hitbox towards player
        Vector3 target = GameObject.FindWithTag("Player").transform.position;
        Vector3 hitboxPosition = transform.position;
        target.z = 0f;
        target.x = target.x - hitboxPosition.x;
        target.y = target.y - hitboxPosition.y;
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    }



}
