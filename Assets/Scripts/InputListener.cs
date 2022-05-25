using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    //add gameobjects that require inputs here
    public GameObject player;

    //set keybinds here
    string meleeAttackButton = "1";
    string rangedAttackButton = "2";



    //call gameobject methods here
    void Update()
    {
        if(Input.GetKeyDown(meleeAttackButton)){
            player.GetComponent<PlayerCombat>().MeleeAttack();
        }
        if(Input.GetKeyDown(rangedAttackButton)){
            player.GetComponent<PlayerCombat>().RangedAttack();
        }
        
    }
}
