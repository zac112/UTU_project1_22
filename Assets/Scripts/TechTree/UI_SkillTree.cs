using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillTree : MonoBehaviour
{
    private PlayerSkills playerSkills;

        [SerializeField] Material skillUnlockableMaterial;
    [SerializeField] Material skillLockedMaterial;
     [SerializeField] SkillUnlockPath[] skillUnlockPathArray;
    [SerializeField] Sprite lineSprite;
    [SerializeField] Sprite lineGlowSprite;

    private List<SkillButton> skillButtonList;

    void Awake()
    {
        playerSkills = new PlayerSkills();
        SetPlayerSkills(playerSkills);
        
    }

    public void SetPlayerSkills(PlayerSkills playerSkills) {
        this.playerSkills = playerSkills;

        skillButtonList = new List<SkillButton>();

        skillButtonList.Add(new SkillButton(transform.Find("moveSpeed1Btn"), playerSkills, PlayerSkills.SkillType.MoveSpeed_1, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("moveSpeed2Btn"), playerSkills, PlayerSkills.SkillType.MoveSpeed_2, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("healthMax1Btn"), playerSkills, PlayerSkills.SkillType.HealthMax_1, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("healthMax2Btn"), playerSkills, PlayerSkills.SkillType.HealthMax_2, skillLockedMaterial, skillUnlockableMaterial));
        GameEvents.current.OnSkillUnlockedEvent += PlayerSkills_OnSkillUnlocked;
        UpdateVisuals();
    }

    private void PlayerSkills_OnSkillUnlocked(PlayerSkills.SkillType skillType) {
        UpdateVisuals();
    }

    public void UpdateVisuals() {
        foreach (SkillButton skillButton in skillButtonList) {
            skillButton.UpdateVisual();
        }

        // Darken all links
        foreach (SkillUnlockPath skillUnlockPath in skillUnlockPathArray) {
            foreach (Image linkImage in skillUnlockPath.linkImageArray) {
                linkImage.color = new Color(.5f, .5f, .5f);
                linkImage.sprite = lineSprite;
            }
        }
        
        foreach (SkillUnlockPath skillUnlockPath in skillUnlockPathArray) {
            if (playerSkills.IsSkillUnlocked(skillUnlockPath.skillType) || playerSkills.CanUnlock(skillUnlockPath.skillType)) {
                // Skill unlocked or can be unlocked
                foreach (Image linkImage in skillUnlockPath.linkImageArray) {
                    linkImage.color = Color.white;
                    linkImage.sprite = lineGlowSprite;
                }
            }
        }
    }

    public void HealthMax1Click() {
        playerSkills.TryUnlockSkill(PlayerSkills.SkillType.HealthMax_1);
    }

    public void HealthMax2Click() {
        playerSkills.TryUnlockSkill(PlayerSkills.SkillType.HealthMax_2);
    }
    public void MoveSpeed1Click() {
        playerSkills.TryUnlockSkill(PlayerSkills.SkillType.MoveSpeed_1);
    }
    public void MoveSpeed2Click() {
        playerSkills.TryUnlockSkill(PlayerSkills.SkillType.MoveSpeed_2);
    }

    private class SkillButton {

        private Transform transform;
        private Image image;
        private Image backgroundImage;
        private PlayerSkills playerSkills;
        private PlayerSkills.SkillType skillType;
        private Material skillLockedMaterial;
        private Material skillUnlockableMaterial;

        public SkillButton(Transform transform, PlayerSkills playerSkills, PlayerSkills.SkillType skillType, Material skillLockedMaterial, Material skillUnlockableMaterial) {
            this.transform = transform;
            this.playerSkills = playerSkills;
            this.skillType = skillType;
            this.skillLockedMaterial = skillLockedMaterial;
            this.skillUnlockableMaterial = skillUnlockableMaterial;

            image = transform.Find("image").GetComponent<Image>();
            backgroundImage = transform.Find("Background").GetComponent<Image>();
        }

        public void UpdateVisual() {
            if (playerSkills.IsSkillUnlocked(skillType)) {
                //image.material = null;
                backgroundImage.material = null;
            } else {
                if (playerSkills.CanUnlock(skillType)) {
                    //image.material = skillUnlockableMaterial;
                    backgroundImage.color = new Color(0.4f, 0.18f, 0f, 0.51f);
                    transform.GetComponent<Button>().enabled = true;
                } else {
                    //image.material = skillLockedMaterial;
                    backgroundImage.color = new Color(.3f, .3f, .3f);
                    transform.GetComponent<Button>().enabled = false;
                }
            }
        }

    }


    [System.Serializable]
    public class SkillUnlockPath {
        public PlayerSkills.SkillType skillType;
        public Image[] linkImageArray;
    }
    
}

