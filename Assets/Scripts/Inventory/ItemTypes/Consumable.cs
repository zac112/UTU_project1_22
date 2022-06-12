using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Consumable", menuName = "Items/Consumable" )]
public class Consumable : InventoryItem{

    public int healAmount;

    public void Eat(){
        if(amount > 0){
            // HEAL PLAYER
        }
    }

}
