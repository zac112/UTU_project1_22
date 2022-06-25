using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterMelee : MonoBehaviour, IFighter
{
    
    [SerializeField] List<CombatTarget> targets = new List<CombatTarget>();
    [SerializeField] List<CombatTargetType> preferredTargets = new List<CombatTargetType>();
    AISwordController swordController;
    bool usesWeapon;

    private float range = 1;

    public void Init(List<CombatTargetType> preferredTargets, float range, bool usesWeapon)
    {
        this.preferredTargets = preferredTargets;
        this.range = range;
        this.usesWeapon = usesWeapon;
    }

    void Start()
    {
        GameObject targetCollider = new GameObject("MeleeTargetFinder");
        targetCollider.transform.SetParent(transform);
        targetCollider.transform.localPosition = Vector3.zero;

        CircleCollider2D col = targetCollider.AddComponent<CircleCollider2D>();
        col.radius = range;
        col.isTrigger = true;

        AITargetFinder targetFinder = targetCollider.AddComponent<AITargetFinder>();
        targetFinder.Init(this, preferredTargets);


        GameObject weapon;
        if (usesWeapon)
        {
            weapon = Instantiate(Resources.Load<GameObject>("Weapon"));
            swordController = weapon.AddComponent<AISwordController>();
        }
        else
        {
            weapon = Instantiate(Resources.Load<GameObject>("AoEAttack"));
            swordController = weapon.AddComponent<BruteSwordController>();
        }
        weapon.transform.SetParent(gameObject.transform);
        weapon.transform.localPosition = Vector3.zero;
        swordController.SetCollider(weapon.GetComponent<Collider2D>());
        DamageInflicter df = weapon.GetComponent<DamageInflicter>();
        df.SetStats(GetComponent<Stats>());
        df.SetDamager(CombatTargetType.Enemy);
        
        //StartCoroutine(Toggle(targetCollider));
        StartCoroutine(SelectTarget());
    }

    IEnumerator SelectTarget() 
    {
        while (true)
        {
            Transform oldTarget = GetComponent<AIPathfinding>().getTarget();
            yield return new WaitWhile(() => targets.Count == 0);
            List<CombatTarget> sortedTargets = new List<CombatTarget>();
            foreach(CombatTargetType ctt in preferredTargets)
            {
                sortedTargets.AddRange( targets.FindAll(t => t.GetTargetType() == ctt));

            }
            if (sortedTargets.Count > 0)
            {
                GetComponent<AIPathfinding>().setTarget(sortedTargets[0].GetTarget().transform);
                swordController.SetTarget(sortedTargets[0]);
            }
            int targetCount = targets.Count;
            yield return new WaitWhile(() => targets.Count == targetCount);
            GetComponent<AIPathfinding>().setTarget(oldTarget ? oldTarget : null);
            swordController.SetTarget(null);

        }
    }

    public void AddTarget(CombatTarget target)
    {
        if (!targets.Contains(target)) targets.Add(target);
    }

    public void RemoveTarget(CombatTarget target)
    {
        targets.Remove(target);
    }
}
