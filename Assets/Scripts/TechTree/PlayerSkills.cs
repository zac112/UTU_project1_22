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
    Farming,
    RangeAttack,
   }



   private List<SkillType> unlockedSkillTypeList;

   public PlayerSkills() {
       unlockedSkillTypeList = new List<SkillType>();
   }


   private void UnlockSkill(SkillType skillType) {
    
        unlockedSkillTypeList.Add(skillType);
        //GameEvents.current.OnSkillUnlocked(skillType);

      
   }

   public bool IsSkillUnlocked(SkillType skillType) {
       return unlockedSkillTypeList.Contains(skillType);
   }
   
   public bool CanUnlock(SkillType skillType) {
   SkillType skillRequirement = GetSkillRequirement(skillType);
    if (skillRequirement != SkillType.None) {
        if (!IsSkillUnlocked(skillRequirement)) {
            return true;
        }
        return false;
    }
    return true;
   }

   public SkillType GetSkillRequirement(SkillType skillType) {
    switch (skillType) {
        case SkillType.HealthMax_2: return SkillType.HealthMax_1;
    }
    return SkillType.None;
   }

   public bool TryUnlockSkill(SkillType skillType) {
   if (CanUnlock(skillType)) {
    UnlockSkill(skillType);
    return true;
   }
   return false;
   }

   
}
