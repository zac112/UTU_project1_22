using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    public Animator anim;

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
        if(x!=0&&y!=0){
            if(x>0){
                if(y>0){
                    anim.SetInteger("Direction",2);
                }else{
                    anim.SetInteger("Direction",4);
                }
            } else{
                if(y>0){
                    anim.SetInteger("Direction",1);
                }else{
                    anim.SetInteger("Direction",3); 
                }
            }
        }


    }



}
