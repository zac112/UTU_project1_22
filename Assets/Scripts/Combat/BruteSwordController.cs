using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteSwordController : AISwordController
{
    [SerializeField] bool attacking = false;
    [SerializeField] float range = 1;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(GetDistance());
    }

    protected override bool ShouldEnableHitbox()
    {
        return attacking && Time.time > lastAttackedAt + attackCooldown;
    }

    private IEnumerator GetDistance()
    {
        Animator anim = transform.parent.GetComponent<Animator>();
        while (true)
        {
            if (target && Vector3.Distance(transform.position, target.transform.position) < range)
            {
                print("Attacking");
                attacking = true;
                anim.SetInteger("IsAttacking", 1); //not sure why, but booleans did not work in the animator so this has to be an integer                
                yield return new WaitForSeconds(attackCooldown);
                anim.SetInteger("IsAttacking", 0);
                attacking = false;
            }
            yield return null;
        }
    }
}
