using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechnologyListPopulator : MonoBehaviour
{
    [SerializeField]GameObject techButtonPrefab;
    [SerializeField]GameObject techContentPanel;

    void Awake()
    {
        transform.Find("TechTreePanel").gameObject.SetActive(true);
        Technology[] techs = Resources.LoadAll<Technology>("TechnologySO");
        foreach(Technology t in techs)
        {
            GameObject go = Instantiate<GameObject>(techButtonPrefab);
            go.transform.SetParent(techContentPanel.transform);
            go.transform.localScale = Vector3.one;
            go.GetComponent<GUITechButton>().SetTech(t);
        }

        Invoke("Hide",0.1f);
    }

    void Hide() {        
        transform.Find("TechTreePanel").gameObject.SetActive(false);
    }
}
