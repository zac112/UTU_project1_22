using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUITechButton : MonoBehaviour
{
    [SerializeField] Technology tech;
    TMP_Text regText;
    List<TechnologyPrerequisite> prereqs;

    int lineheight = 35;
    void Start()
    {
        prereqs = new List<TechnologyPrerequisite>(tech.prereqs);
        regText = transform.Find("Prereqs").GetComponent<TMP_Text>();
        regText.text = ParseTechs(prereqs);

        transform.Find("Name").GetComponent<TMP_Text>().text = tech.name;
        transform.Find("Text").GetComponent<TMP_Text>().text = $"{tech.cost}";
        transform.Find("Image").GetComponent<Image>().sprite = tech.image;
        GameEvents.current.TechnologyUnlock += TechUnlocked;

        ResizeButton();
    }

    private string ParseTechs(List<TechnologyPrerequisite> prereqs) {
        string res = "";
        foreach (TechnologyPrerequisite s in prereqs)
        {
            res += s.ToString();
            res += "\n";
        }
        return res;
    }

    private void ResizeButton() {
        regText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 150 + lineheight * prereqs.Count);
        regText.GetComponent<RectTransform>().ForceUpdateRectTransforms();
    }

    public void SetTech(Technology t) {
        tech = t;
    }

    public void OnButtonPress() {
        if (prereqs.Count > 0) return;

        GameEvents.current?.OnTechnologyPurchase(tech);
    }

    void TechUnlocked(TechnologyPrerequisite prereq) {
        if (prereq == tech.unlocksPreq) { 
            Destroy(gameObject);
            GameEvents.current.TechnologyUnlock -= TechUnlocked;
            return;
        }

        prereqs.Remove(prereq);        
        regText.text = ParseTechs(prereqs);
        ResizeButton();
    }
}
