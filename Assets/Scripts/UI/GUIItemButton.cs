using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIItemButton : MonoBehaviour{
    [SerializeField] TMP_Text UIName;
    [SerializeField] TMP_Text UIAmount;
    [SerializeField] Image UIImage;

    public void UpdateUI(string name, int amount, Sprite sprite){
        UIName.text = name;
        UIAmount.text = "Amount: " + amount;
        UIImage.sprite = sprite;
    }

    public void OnButtonDown(){
        //TODO:
    }
}
