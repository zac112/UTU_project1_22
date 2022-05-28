using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMeleeHitbox : MonoBehaviour
{

    private float lifeTime = 0.1f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

}
