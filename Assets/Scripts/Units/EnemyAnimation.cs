using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{

    public Animator anim;
    public SpriteRenderer sr;

    Vector3 lastUpdatePos = Vector3.zero;
    Vector3 dist;
    float currentSpeed;


    void FixedUpdate() {

        //calculate movement speed and send to animator
        dist = transform.position - lastUpdatePos;
        currentSpeed = dist.magnitude / Time.deltaTime;
        lastUpdatePos = transform.position;
        anim.SetFloat("Speed",currentSpeed);


        float x = dist.x;
        float y = dist.y;


        //calculate direction and send to animator
        // BL = 1, BR = 2, FL = 3, FR = 4
        //because some enemies dont have separate animations for left and right, its easier to just flip the spriterenderer 
        if(currentSpeed>0.1f){
            if(x!=0&&y!=0){
                if(x>0){
                    if(y>0){
                        anim.SetInteger("Direction",2);
                        sr.flipX=true;
                    }else{
                        anim.SetInteger("Direction",4);
                        sr.flipX=true;
                    }
                } else{
                    if(y>0){
                        anim.SetInteger("Direction",1);
                        sr.flipX=false;
                    }else{
                        anim.SetInteger("Direction",3); 
                        sr.flipX=false;
                    }
                }
            }
        }
    }

    public void AttackAnimation() {
        StartCoroutine(SetAttackBool());
    }

    IEnumerator SetAttackBool()
    {
        anim.SetInteger("IsAttacking", 1); //not sure why, but booleans did not work in the animator so this has to be an integer
        yield return new WaitForSeconds(1f);
        anim.SetInteger("IsAttacking", 0);

    }

}
