using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour{

    public static UIManager current;
    void Awake() => current = this;

    [Header("Player")]

    [SerializeField] TMP_Text health;
    [SerializeField] TMP_Text gold;
    [SerializeField] TMP_Text wood;
    [SerializeField] PlayerStats stats;
    [SerializeField] HealthIcon healthIcon;

    [Header("Inventory")]

    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject inventoryButtonPrefab;

    [Header("Farming")]

    [SerializeField] public GUIFarmingButton selectCrop;
    [SerializeField] public bool isPlanting = false;

    GUIItemButton[] itemButtons;


    public void UpdateHealthText(int amount){
        health.text = $"{amount}";
    }

    public void UpdateHealthIcon(float percentage){
        healthIcon.UpdateIcon(percentage);
    }

    public void UpdateGoldText(int amount) => gold.text = $"{amount}";

    public void UpdateWoodText(int amount) => wood.text = $"{amount}";



    public void GenerateInventoryButtons(InventoryItem[] items){
        itemButtons = new GUIItemButton[items.Length];
        for(int i = 0; i < items.Length; i++){
            GameObject obj = Instantiate(inventoryButtonPrefab, inventoryPanel.transform);
            GUIItemButton button = obj.GetComponent<GUIItemButton>();
            button.UpdateUI(items[i].name, items[i].amount, items[i].sprite);
            itemButtons[i] = button; 
        }
    }

    public void UpdateItemButton(int index, InventoryItem item){
        itemButtons[index].UpdateUI(item.name, item.amount, item.sprite);
    }

    public void SelectCrop(GUIFarmingButton newCrop){
        if(selectCrop == newCrop){
            selectCrop = null;
            isPlanting = false;
        }
        else{
            selectCrop = newCrop;
            isPlanting = true;
        }
    }

    public void RegisterWeaponChanges(InputListener player)
    {
        Transform bar = transform.Find("Sidebar");
        bar.Find("SwordButton").GetComponent<Button>().onClick.AddListener(player.ToggleSword);
        bar.Find("BowButton").GetComponent<Button>().onClick.AddListener(player.ToggleBow);
        bar.Find("PickaxeButton").GetComponent<Button>().onClick.AddListener(player.TogglePick);
        bar.Find("AxeButton").GetComponent<Button>().onClick.AddListener(player.ToggleAxe);
        
    }
}
