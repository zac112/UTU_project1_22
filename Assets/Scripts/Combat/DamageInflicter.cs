using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageInflicter : MonoBehaviour
{
    [SerializeField] Stats stats;
    [SerializeField] CombatTargetType typeOfDamage;

    public void SetDamager(CombatTargetType damager) { typeOfDamage = damager; }
    public CombatTargetType GetDamager() { return typeOfDamage; }
    public Stats GetStats(){ return stats; }
    public void SetStats(Stats stats) { this.stats = stats; }
}
