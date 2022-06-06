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

    [SerializeField] TMP_Text UIName;
    [SerializeField] TMP_Text UICost;
    [SerializeField] Image UIImage;

    BuildingPlacementSystem bps;
    GameObject buildingsPanel;

    void UpdateUI(){
        UIName.text = name;
        UICost.text = "Cost:" + cost;
        UIImage.sprite = sprite;
    }

    void Start(){
        UpdateUI();

        bps = GameObject.Find("BuildingPlacementSystem").GetComponent<BuildingPlacementSystem>();

        buildingsPanel = GameObject.Find("BuildingsPanel");
    }

    public void OnButtonDown()
    {
        bps.selectBuildingGUI(building);
        Debug.Log("Button pressed down");

        // Hide building menu
        buildingsPanel.SetActive(false);
    }
}
