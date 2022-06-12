using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    private PlayerSkills playerSkills;

    void Awake()
    {
        playerSkills = new PlayerSkills();
    }

    public void SetPlayerSkills(PlayerSkills playerSkills) {
        this.playerSkills = playerSkills;
    }

    public void HealthMax1Click() {
        playerSkills.UnlockSkill(PlayerSkills.SkillType.HealthMax_1);
    }
}
