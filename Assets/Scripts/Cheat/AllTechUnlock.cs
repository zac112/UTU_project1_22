using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTechUnlock : MonoBehaviour
{
    
    void Start()
    {
        foreach(TechnologyPrerequisite t in Enum.GetValues(typeof(TechnologyPrerequisite))){
            GameEvents.current.OnTechnologyUnlock(t);
        }
    }

}
