using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordController : MonoBehaviour
{

    bool attackDirection = false;
    bool attacking = false;
    float attackWidth = 170f; //how wide the attack swing is in degrees
    float attackTime = 0.25f; //how long the attack takes

    //rotate sword toward mouse
    void Update(){
        if(!attacking){
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 dir = Input.mousePosition - pos;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public void Attack(){
        if(attackDirection){
            transform.Rotate(0.0f, 0.0f, attackWidth/2, Space.Self); //rotate sword 90 degrees to prepare for attack
            StartCoroutine(SmoothRotate(Vector3.forward * -attackWidth, attackTime)); //rotate sword smoothly during attack
            transform.Rotate(0.0f, 0.0f, -attackWidth/2, Space.Self); //reset sword position
        }else{
            transform.Rotate(0.0f, 0.0f, -attackWidth/2, Space.Self);
            StartCoroutine(SmoothRotate(Vector3.forward * attackWidth, attackTime));
            transform.Rotate(0.0f, 0.0f, attackWidth/2, Space.Self);
        }
        attackDirection = !attackDirection; //swap attack direction after attack
    }

    IEnumerator SmoothRotate(Vector3 angle, float time){   
        attacking = true; //stops sword from moving toward mouse during attack
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + angle);
    
        for(var t = 0f; t < 1; t += Time.deltaTime/time) {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }
        yield return new WaitForSeconds(0.2f); //cooldown after attack so the sword doesnt instantly snap back

        attacking=false;
    }

}
