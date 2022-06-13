using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyUnlocker : MonoBehaviour
{
    [SerializeField] TechnologyPrerequisite unlocks;
    
    void Start()
    {
        GameEvents.current.OnTechnologyUnlock(unlocks);
    }
}
