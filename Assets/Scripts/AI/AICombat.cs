using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICombat : MonoBehaviour
{    
    [Serializable]
    private class CombatPreference {
        public EnemyCombatType combatType;
        public List<CombatTargetType> targetType;
    }

    [SerializeField] List<CombatPreference> combatType;
    [SerializeField] bool usesWeapon;
    
    void Start()
    {
        foreach (CombatPreference t in combatType) {
            switch (t.combatType)
            {
                case EnemyCombatType.Melee:
                    FighterMelee fm = gameObject.AddComponent<FighterMelee>();
                    fm.Init(t.targetType, 5, usesWeapon);
                    break;
                case EnemyCombatType.Ranged:
                    FighterRanged fr = gameObject.AddComponent<FighterRanged>();
                    break;
            }
        }
    }
}
