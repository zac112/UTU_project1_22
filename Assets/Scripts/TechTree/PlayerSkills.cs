using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkills

{



   public enum SkillType {
    None,
    MoveSpeed_1,
    MoveSpeed_2,
    HealthMax_1,
    HealthMax_2,
    HealthMax_3,
    FogReveal_1,
    FogReveal_2,
   }



   private List<SkillType> unlockedSkillTypeList;

   public PlayerSkills() {
       unlockedSkillTypeList = new List<SkillType>();
   }


   private void UnlockSkill(SkillType skillType) {
    if (!IsSkillUnlocked(skillType)) {
        unlockedSkillTypeList.Add(skillType);
        GameEvents.current.OnSkillUnlocked(skillType);
    }
   }

   public bool IsSkillUnlocked(SkillType skillType) {
       return unlockedSkillTypeList.Contains(skillType);
   }
   
   public bool CanUnlock(SkillType skillType) {
   SkillType[] skillRequirements = GetSkillRequirements(skillType);
    if (skillRequirements[0] != SkillType.None) {
        foreach(SkillType sr in skillRequirements) {
            if (!IsSkillUnlocked(sr)) {
                return false;
            }
        }
    }
    return true;
   }

   public SkillType[] GetSkillRequirements(SkillType skillType) {
    switch (skillType) {
        case SkillType.HealthMax_2: 
            return new SkillType[] {SkillType.HealthMax_1};
        case SkillType.HealthMax_3: 
            return new SkillType[] {SkillType.HealthMax_1, SkillType.HealthMax_2};
        case SkillType.MoveSpeed_2: 
            return new SkillType[] {SkillType.MoveSpeed_1};
        case SkillType.FogReveal_2: 
            return new SkillType[] {SkillType.FogReveal_1};
    }
    return new SkillType[] {SkillType.None};
   }

   public bool TryUnlockSkill(SkillType skillType) {
   if (CanUnlock(skillType)) {
    UnlockSkill(skillType);
    return true;
   }
   return false;
   }

   
}
