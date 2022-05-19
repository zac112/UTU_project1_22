using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour{

    
    [SerializeField] TMP_Text statsText;
    [SerializeField] PlayerStats stats;

    void Update(){
        UpdateStatText(); 
    }

    void UpdateStatText(){
        statsText.text = $"health:{stats.GetCurrentHealth()}\ngold:{stats.GetGold()}";
    }

}
