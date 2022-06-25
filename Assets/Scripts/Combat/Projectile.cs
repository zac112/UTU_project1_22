using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction { get; set; }
    public float speed { get; set; }
    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        transform.position += direction * speed;
    }

}
