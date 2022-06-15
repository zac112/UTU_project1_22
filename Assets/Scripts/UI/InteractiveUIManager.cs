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
    Building building;
    BuildingPlacementSystem bps;
    string new_name;

    // Start is called before the first frame update
    void Start(){
        
        // Find a Better way to find the UI object here ???
        // Now it's hardcoded
        canvas = GameObject.Find("Canvas");
        interactiveUI = canvas.transform.GetChild(8).gameObject;
        smithyUI = canvas.transform.GetChild(7).gameObject;
        objectName = interactiveUI.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        objectHealth = interactiveUI.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();

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

            // Removing (Clone) from the name
            new_name = gameObject.name.Remove((gameObject.name.Length-7),7);

            // Setting values
            objectName.text = $"Name: {new_name}";
            objectHealth.text = $"Health: {building.HealthPoints}";
        }

            // If statemens for different interactable buldings
        if(gameObject.name == "Smithy(Clone)" && Input.GetMouseButtonDown(0)){
                smithyUI.SetActive(true);
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

