using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headquarters : MonoBehaviour
{
    [SerializeField] GameObject nextTier;
    [SerializeField] TechnologyPrerequisite requirement;

    void OnEnable()
    {        
        GameEvents.current.TechnologyUnlock += Upgrade;
    }

    void OnDisable()
    {
        GameEvents.current.TechnologyUnlock -= Upgrade;
    }

    void OnDestroy()
    {
        GameEvents.current.TechnologyUnlock -= Upgrade;
    }

    virtual protected void Upgrade(TechnologyPrerequisite prerq)
    {
        if (!nextTier || prerq != requirement) return;
        GameObject.Instantiate(nextTier, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
