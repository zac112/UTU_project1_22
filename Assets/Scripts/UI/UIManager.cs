using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour{

    public static UIManager current;
    void Awake() => current = this;


    [SerializeField] TMP_Text health;
    [SerializeField] TMP_Text gold;
    [SerializeField] PlayerStats stats;
    [SerializeField] HealthIcon healthIcon;

    [Header("Inventory")]

    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject inventoryButtonPrefab;


    GUIItemButton[] itemButtons;


    public void UpdateHealthText(int amount){
        health.text = $"Health: {amount}";
    }

    public void UpdateHealthIcon(float percentage){
        healthIcon.UpdateIcon(percentage);
    }

    public void UpdateGoldText(int amount) => gold.text = $"Gold: {amount}";



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

}
