using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterRanged : MonoBehaviour, IFighter
{
    GameObject projectile;

    void Start()
    {
        
    }

    public void AddTarget(CombatTarget target)
    {
     
    }

    public void RemoveTarget(CombatTarget target)
    {
     
    }
    
    public void RangedAttack()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameObject proj = Instantiate(projectile, gameObject.transform.position + new Vector3(0f, 0f, 50f), new Quaternion(0, 0, 0, 0));
        proj.transform.parent = gameObject.transform;
     
    }
}
