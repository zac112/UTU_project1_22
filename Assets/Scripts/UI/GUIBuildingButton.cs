using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIBuildingButton : MonoBehaviour{

    [SerializeField] string buildingName;
    [SerializeField] GameObject building;
    [SerializeField] int cost;
    [SerializeField] Sprite sprite;
    

    [SerializeField] TMP_Text UIName;
    [SerializeField] TMP_Text UICost;
    [SerializeField] Image UIImage;

    BuildingPlacementSystem bps;
    GameObject buildingsPanel;
    BuildCost buildCost;

   void UpdateUI(){
        UIName.text = buildingName;
        if (building != null)
        {
            UICost.text = $"Cost: {buildCost.Cost}";
        }
        else {
            UICost.text = $"Cost: {cost}";
        }
        
        UIImage.sprite = sprite;
    }

    void Start(){
        
        if (building != null) {
            buildCost = building.GetComponent<BuildCost>();
        }
        buildingsPanel = GameObject.Find("BuildingsPanel");
        UpdateUI();
    }

    public void OnButtonDown()
    {
        GameEvents.current.OnBuildingSelected(building);
        buildingsPanel.SetActive(false);
    }

}
