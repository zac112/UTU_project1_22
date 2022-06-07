using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedHitbox : MonoBehaviour
{


    private float lifeTime = 5.0f;
    private float projectileSpeed=225.0f;


    void Start()
    {
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;

        //projectile gets deleted after lifeTime has passed
        Destroy(gameObject, lifeTime);

        //rotate projectile towards mouse
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //add velocity
        dir.z=0;
        dir.Normalize();
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(dir*projectileSpeed);

    }


}
