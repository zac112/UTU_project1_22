using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class InteractiveUIManager : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject interactiveUI;
    [SerializeField] GameObject smithyUI;
    [SerializeField] TMP_Text objectName;
    [SerializeField] TMP_Text objectHealth;
    [SerializeField] TMP_Text objectMiningEfficiency;
    [SerializeField] GameObject objectMiningEfficiencyGO;
    Building building;
    BuildingPlacementSystem bps;
    GameObject player;
    string new_name;
    Transform goldHeight;

    // Start is called before the first frame update
    void Start(){
        
        // Find a Better way to find the UI object here ???
        // Now it's hardcoded
        canvas = GameObject.Find("Canvas");
        interactiveUI = canvas.transform.GetChild(6).gameObject;
        smithyUI = canvas.transform.GetChild(10).gameObject;
        objectName = interactiveUI.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        objectHealth = interactiveUI.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();
        objectMiningEfficiencyGO = interactiveUI.transform.GetChild(2).gameObject;
        objectMiningEfficiencyGO.SetActive(false); // mining efficiency not visible by default
        objectMiningEfficiency = interactiveUI.transform.GetChild(2).gameObject.GetComponent<TMP_Text>();
        player = GameObject.Find("Player(Clone)");

        // Acessing other scripts
        building = gameObject.GetComponent<Building>();
        bps = GameObject.Find("BuildingPlacementSystem").GetComponent<BuildingPlacementSystem>();

    }

    void OnMouseOver(){
        // Don't show building info if mouse on UI
        if (!IsMouseOverUI()){
            // Displaying UI and moving it with the mouse
            interactiveUI.SetActive(true);
            interactiveUI.transform.position = Input.mousePosition;

            if (gameObject.tag == "GoldMine")
            {
                objectMiningEfficiencyGO.SetActive(true);
                string efficiency = (System.Math.Round(building.GetComponent<GoldMineScript>().GetMiningEfficiency(), 2) * 100).ToString() + " %";               
                objectMiningEfficiency.text = $"Mining efficiency: {efficiency}";               
            }

            // Removing (Clone) from the name
            new_name = gameObject.name.Remove((gameObject.name.Length-7),7);

            // Setting values
            objectName.text = $"Name: {new_name}";
            objectHealth.text = $"Health: {building.HealthPoints}";
        }

        
        // If statemens for different interactable buldings
        if(gameObject.tag == "Smithy" && Input.GetMouseButtonDown(0)){
                smithyUI.SetActive(true);
            }

        // TODO: Open library UI
        if(gameObject.tag == "Library" && Input.GetMouseButtonDown(0)){
                return;
            }
    }

    void OnMouseExit(){
        interactiveUI.SetActive(false);
    }

    private bool IsMouseOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
}

