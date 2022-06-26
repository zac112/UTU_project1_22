using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CombatTarget : MonoBehaviour
{
    [SerializeField] CombatTargetType targetType;
    [SerializeField] Stats thisUnit;
    [SerializeField] List<CombatTargetType> canBeDamagedBy;

    public GameObject GetTarget() => gameObject;
    public CombatTargetType GetTargetType() => targetType;

    private void Start()
    {
        if (!thisUnit) thisUnit = GetComponent<Stats>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageInflicter other = collision.GetComponent<DamageInflicter>();
        if (!other) return;
        if (!canBeDamagedBy.Contains(other.GetDamager())) return;
        if (Vector2.Distance(collision.transform.position, transform.position) > 5) return;
        thisUnit.ReceiveDamage(other.GetStats().GetAttack());
    }
}
