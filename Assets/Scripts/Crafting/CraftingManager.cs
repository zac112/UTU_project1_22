using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    Item currentItem;
    [SerializeField] Image customCursor;

    [SerializeField] Slot[] craftingSlots;
    [SerializeField] List<Item> itemList;
    [SerializeField] string[] recipes;
    [SerializeField] Item[] recipeResults;
    [SerializeField] Slot resultSlot;

    void Update() 
    {
        if (Input.GetMouseButtonUp(0)) {
            if (currentItem != null){
                customCursor.gameObject.SetActive(false);
                Slot nearestSlot = null;
                float shortestDistance = 100f;

                foreach(Slot slot in craftingSlots) {
                    float dist = Vector2.Distance(Input.mousePosition, slot.transform.position);
                    
                    if (dist < shortestDistance){
                        shortestDistance = dist;
                        nearestSlot = slot;
                    }
                }
                if (nearestSlot == null) {
                    currentItem = null;
                    return;
                }
                nearestSlot.gameObject.SetActive(true);
                nearestSlot.GetComponent<Image>().sprite = currentItem.GetComponent<Image>().sprite;
                nearestSlot.item = currentItem;

                itemList[nearestSlot.index] = currentItem;

                currentItem = null;

                CheckForCompletedRecipes();
            }
        }
    }

    void CheckForCompletedRecipes() {
        resultSlot.gameObject.SetActive(false);
        resultSlot.item = null;

        string currentRecipeString = "";
        foreach(Item item in itemList) {
            if (item != null){
                currentRecipeString += item.itemName;
            } else {
                currentRecipeString += "null";
            }
        }

        for (int i = 0; i < recipes.Length; i++) {
            
            if (recipes[i] == currentRecipeString) {
                resultSlot.gameObject.SetActive(true);
                resultSlot.GetComponent<Image>().sprite = recipeResults[i].GetComponent<Image>().sprite;
                resultSlot.item = recipeResults[i];
            }
        }
    }
    public void OnClickSlot(Slot slot){
        slot.item = null;
        itemList[slot.index] = null;
        slot.gameObject.SetActive(false);
        CheckForCompletedRecipes();
    }

    public void OnMouseDownItem(Item item)
    {
        if (currentItem == null)
        {
            currentItem = item;
            customCursor.gameObject.SetActive(true);
            customCursor.sprite = currentItem.GetComponent<Image>().sprite;
        }
    }
}
