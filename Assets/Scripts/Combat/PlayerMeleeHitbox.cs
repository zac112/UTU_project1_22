using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeHitbox : MonoBehaviour
{

    private float lifeTime = 0.1f;

    void Start()
    {

        //hitbox gets deleted after lifeTime has passed
        Destroy(gameObject, lifeTime);

        //rotate hitbox towards mouse

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        
    }

}
