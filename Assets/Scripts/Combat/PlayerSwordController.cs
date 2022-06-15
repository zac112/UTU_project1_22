using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordController : MonoBehaviour
{


    Vector3 rotationLast;
    Vector3 rotationDelta;
    float AttackThreshold = 8.0f;

    private float attackCooldown = 0.7f; //seconds
    private float lastAttackedAt = 0f;

    public SpriteRenderer sr;


    void Start() {
        rotationLast = transform.rotation.eulerAngles;
    }

    void Update () {
        //rotate sword toward mouse
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //calculate rotation speed
        rotationDelta = transform.rotation.eulerAngles - rotationLast;
        rotationLast = transform.rotation.eulerAngles;

        //if rotation speed > threshold and cooldown not active, enable hitbox
        if(Mathf.Abs(rotationDelta.z)>AttackThreshold&&Time.time > lastAttackedAt + attackCooldown){
            lastAttackedAt = Time.time;
            StartCoroutine(EnableHitbox()); //enable hitbox for 100ms 
        }
   
    }

    IEnumerator EnableHitbox(){
        this.gameObject.GetComponent<Collider2D>().enabled=true;
        sr.color=new Color(1f, 0f, 0f, 1f); //change sword color to make it more obvious when you are attacking

        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<Collider2D>().enabled=false;
        sr.color=new Color(1f, 1f, 1f, 1f);
    }


}
