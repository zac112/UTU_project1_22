using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem : ScriptableObject {

    //BASE FOR ALL INVENTORY ITEMTYPES

    public new string name;
    public Sprite sprite;
    public string description;
    public int amount;

}
