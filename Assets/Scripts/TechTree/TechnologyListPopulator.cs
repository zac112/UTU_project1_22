using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechnologyListPopulator : MonoBehaviour
{
    [SerializeField]GameObject techButtonPrefab;
    void Awake()
    {
        Technology[] techs = Resources.LoadAll<Technology>("TechnologySO");
        foreach(Technology t in techs)
        {
            GameObject go = Instantiate<GameObject>(techButtonPrefab);
            go.transform.SetParent(transform);
            go.transform.localScale = Vector3.one;
            go.GetComponent<GUITechButton>().SetTech(t);
        }
    }
    
}
