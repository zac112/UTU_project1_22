using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    private PlayerSkills playerSkills;

    void Awake()
    {
        
    }

    public void SetPlayerSkills(PlayerSkills playerSkills) {
        this.playerSkills = playerSkills;
    }

}
