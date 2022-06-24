using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITargetFinder : MonoBehaviour
{
    [SerializeField] IFighter fighterScript;
    [SerializeField] List<CombatTargetType> preferredTargets;

    public void Init(IFighter fighter, List<CombatTargetType> preferredTargets)
    {
        this.fighterScript = fighter;
        this.preferredTargets = preferredTargets;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CombatTarget target = collision.gameObject.GetComponent<CombatTarget>();
        if(target && preferredTargets.Contains(target.GetTargetType()))
            fighterScript.AddTarget(target);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CombatTarget target = collision.gameObject.GetComponent<CombatTarget>();
        if (target)
            fighterScript.RemoveTarget(target);
    }
}
