using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIBuildingButton : MonoBehaviour{

    [SerializeField] string name;
    [SerializeField] string cost;
    [SerializeField] Sprite sprite;
    [SerializeField] Transform building;

    [SerializeField] TMP_Text UIName;
    [SerializeField] TMP_Text UICost;
    [SerializeField] Image UIImage;

    BuildingPlacementSystem bps;

    void UpdateUI(){
        UIName.text = name;
        UICost.text = "Cost:" + cost;
        UIImage.sprite = sprite;
    }

    void Start(){
        UpdateUI();

        bps = GameObject.Find("BuildingPlacementSystem").GetComponent<BuildingPlacementSystem>();
    }

    void OnButtonClick() 
    {
        bps.selectBuildingGUI(building);
    }
}
