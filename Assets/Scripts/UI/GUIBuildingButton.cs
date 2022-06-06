using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIBuildingButton : MonoBehaviour{

    [SerializeField] string name;
    [SerializeField] string cost;
    [SerializeField] Transform building;
    [SerializeField] Sprite sprite;

    BuildingPlacementSystem bps;

    [SerializeField] TMP_Text UIName;
    [SerializeField] TMP_Text UICost;
    [SerializeField] Image UIImage;

    void UpdateUI(){
        UIName.text = name;
        UICost.text = "Cost:" + cost;
        UIImage.sprite = sprite;
    }

    void Start(){
        UpdateUI();

        bps = GameObject.Find("BuildingPlacementSystem").GetComponent<BuildingPlacementSystem>();
    }

    public void OnButtonDown()
    {
        bps.selectBuildingGUI(building);
        Debug.Log("Button pressed down");
    }
}
