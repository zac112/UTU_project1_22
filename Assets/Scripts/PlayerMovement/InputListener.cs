using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    //add gameobjects that require inputs here
    public GameObject player;

    //set keybinds here
    string ToggleSwordMode = "1";
    string RangedAttackButton = "2";


    //call gameobject methods here
    void Update()
    {
        if(Input.GetKeyDown(ToggleSwordMode)){
            player.transform.GetChild(0).gameObject.SetActive(!player.transform.GetChild(0).gameObject.activeSelf); 
        }

        if(Input.GetKeyDown(RangedAttackButton)){
            player.GetComponent<PlayerCombat>().RangedAttack();
        }
        
    }
}
