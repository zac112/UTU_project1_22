using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour{

    [SerializeField] InventoryItem[] items;

    void Start(){
        StartInventoryUI();
    }

    public void AddItem(string itemName, int count){
        for(int i = 0; i < items.Length; i++){
            if(itemName == items[i].name){
                items[i].amount += count;
                UpdateItemUI(i);
            }
        }
    }

    public void RemoveItem(string itemName, int count){
        for(int i = 0; i < items.Length; i++){
            if(itemName == items[i].name){
                items[i].amount = (int) Mathf.Clamp(items[i].amount-count, 0,float.MaxValue);
                UpdateItemUI(i);
            }
        }
    }

    void StartInventoryUI(){
        UIManager.current.GenerateInventoryButtons(items);
    }

    void UpdateItemUI(int index){
        UIManager.current.UpdateItemButton(index, items[index]);
    }

}
