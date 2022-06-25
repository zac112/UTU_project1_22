using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISwordController : PlayerSwordController
{
    CombatTarget target;

    Quaternion rotation = Quaternion.identity;
    Vector2 thispos;
    Vector2 targetpos;
    Vector2 dir;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(Rotate());
    }

    public void SetTarget(CombatTarget target)
    {
        this.target = target;
    }

    void Update()
    {        
        HandleUpdate();
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            yield return new WaitUntil(() => target);

            CalculatePositions();
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
            rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            while (target && Vector2.Distance(thispos, targetpos) < 70)
            {

                CalculatePositions();                
                transform.Rotate(transform.forward, angle);

                angle += 10;
                bool enabled = Vector2.Dot(dir, transform.parent.right) == Vector2.Dot(transform.right, transform.parent.right);
                sr.enabled = enabled;
                gameObject.GetComponent<Collider2D>().enabled = enabled;
                yield return null;
            }
            sr.enabled = true;
            gameObject.GetComponent<Collider2D>().enabled = false;
            yield return null;
        }
    }

    private void CalculatePositions() {
        if (!target) return;
        thispos = Camera.main.WorldToScreenPoint(transform.position);
        targetpos = Camera.main.WorldToScreenPoint(target.transform.position);
        dir = (targetpos - thispos).normalized;
    }

    protected override Quaternion GetRotation()
    {
        return rotation;        
    }
}
