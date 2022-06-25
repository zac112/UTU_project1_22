using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterRanged : MonoBehaviour, IFighter
{
    [SerializeField] float attackCooldown = 3;
    [SerializeField] float projectileSpeed = 2;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject target;
    [SerializeField] List<CombatTarget> targets = new List<CombatTarget>();
    [SerializeField] List<CombatTargetType> preferredTargets = new List<CombatTargetType>();
    
    private float range = 1;


    public void Init(List<CombatTargetType> preferredTargets, float range, GameObject projectilePrefab)
    {
        this.preferredTargets = preferredTargets;
        this.range = range;
        this.projectile = projectilePrefab;
    }

    void Start()
    {
        GameObject targetCollider = new GameObject("RangedTargetFinder");
        targetCollider.transform.SetParent(transform);
        targetCollider.transform.localPosition = Vector3.zero;

        CircleCollider2D col = targetCollider.AddComponent<CircleCollider2D>();
        col.radius = range;
        col.isTrigger = true;

        AITargetFinder targetFinder = targetCollider.AddComponent<AITargetFinder>();
        targetFinder.Init(this, preferredTargets);
        StartCoroutine(SelectTarget());
        StartCoroutine(RangedAttack());
    }

    IEnumerator SelectTarget()
    {
        while (true)
        {
            
            yield return new WaitWhile(() => targets.Count == 0);
            List<CombatTarget> sortedTargets = new List<CombatTarget>();
            foreach (CombatTargetType ctt in preferredTargets)
            {
                sortedTargets.AddRange(targets.FindAll(t => t.GetTargetType() == ctt));

            }
            if (sortedTargets.Count > 0)
            {
                target = sortedTargets[0].gameObject;
            }
            int targetCount = targets.Count;
            yield return new WaitWhile(() => targets.Count == targetCount);

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

    IEnumerator RangedAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => target);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GameObject proj = Instantiate(projectile, gameObject.transform.position + new Vector3(0f, 0f, 50f), new Quaternion(0, 0, 0, 0));
            proj.transform.parent = gameObject.transform;
            
            DamageInflicter df = proj.GetComponent<DamageInflicter>();
            df.SetStats(gameObject.GetComponent<Stats>());

            Projectile p = proj.GetComponent<Projectile>();            
            Vector3 dir = (target.transform.position - transform.position).normalized;
            dir.z = 0;
            p.direction = dir;
            p.speed = projectileSpeed;
            yield return new WaitForSeconds(attackCooldown);
        }
    }
}
