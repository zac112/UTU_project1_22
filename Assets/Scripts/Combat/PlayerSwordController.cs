using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSwordController : NetworkBehaviour
{


    Vector3 rotationLast = Vector3.zero;
    Vector3 rotationDelta;
    float AttackThreshold = 8.0f;

    protected float attackCooldown = 0.7f; //seconds
    protected float lastAttackedAt = 0f;

    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] Collider2D col;

    NetworkVariable<Quaternion> netRotation = new NetworkVariable<Quaternion>();

    void Start() {
    }

    void Update () 
    {
        if (!NetworkObject.IsLocalPlayer)
        {
            transform.rotation = netRotation.Value;
            return;
        }        
        HandleUpdate();
    }

    protected void HandleUpdate()
    {
        transform.rotation = GetRotation();
        MoveSwordServerRpc(transform.rotation);

        //calculate rotation speed
        rotationDelta = transform.rotation.eulerAngles - rotationLast;
        rotationLast = transform.rotation.eulerAngles;

        //if rotation speed > threshold and cooldown not active, enable hitbox
        if (ShouldEnableHitbox())
        {
            lastAttackedAt = Time.time;
            StartCoroutine(EnableHitbox()); //enable hitbox for 100ms 
        }
    }
    protected virtual bool ShouldEnableHitbox() 
    {
        return Mathf.Abs(rotationDelta.z) > AttackThreshold && Time.time > lastAttackedAt + attackCooldown;
    }

    protected virtual Quaternion GetRotation() {
        //rotate sword toward mouse
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
    IEnumerator EnableHitbox(){
        col.enabled=true;
        sr.color=new Color(1f, 0f, 0f, 1f); //change sword color to make it more obvious when you are attacking

        yield return new WaitForSeconds(1f);
        col.enabled=false;
        sr.color=new Color(1f, 1f, 1f, 1f);
    }

    [ServerRpc]
    void MoveSwordServerRpc(Quaternion rot) {
        netRotation.Value = rot;
    }

    public void SetCollider(Collider2D col) { this.col = col; }
}
