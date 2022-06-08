using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills
{
   public enum SkillType {
       MoveSpeed_1,
       MoveSpeed_2,
       HealthMax_1,
    HealthMax_2
   }

   private List<SkillType> unlockedSkillTypeList;

   public PlayerSkills() {
       unlockedSkillTypeList = new List<SkillType>();
   }

   public void UnlockSkill(SkillType skillType) {
       if (!IsSkillUnlocked(skillType)) {
            unlockedSkillTypeList.Add(skillType);
       }
      
   }

   public bool IsSkillUnlocked(SkillType skillType) {
       return unlockedSkillTypeList.Contains(skillType);
   }
}
