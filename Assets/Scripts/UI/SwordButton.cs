using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SwordButton : MonoBehaviour
{
    [SerializeField] Image UIImage;
    [SerializeField] TMP_Text UIName;
    [SerializeField] TMP_Text UICost;
    [SerializeField] TMP_Text UIDamage;
    InventoryManager im;

    public void UpdateUI(string name, int damage, int cost, Sprite sprite){
        UIName.text = "Name" + name;
        UIName.text = "Damage:" + damage;
        UICost.text = "Cost: " + cost;
        UIImage.sprite = sprite;
    }
    
    public void OnButtonDown(){
        // TODO: Buying sword will move it to inventory or replace current sword
        // Also remove bought sword from smithy?  
    }
}
