using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTarget : MonoBehaviour
{
    [SerializeField] CombatTargetType targetType;

    public GameObject GetTarget() => gameObject;
    public CombatTargetType GetTargetType() => targetType;
}
