using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechUnlockListener : MonoBehaviour
{
    [SerializeField] TechnologyPrerequisite unlocksOn;

    void Start()
    {
        GameEvents.current.TechnologyUnlock += TechUnlock;
        gameObject.SetActive(unlocksOn == TechnologyPrerequisite.None || GameObject.FindObjectOfType<AllTechUnlock>());
    }

    private void TechUnlock(TechnologyPrerequisite tech)
    {
        if (tech != unlocksOn) return;

        gameObject.SetActive(true);
        GameEvents.current.TechnologyUnlock -= TechUnlock;
    }
}
