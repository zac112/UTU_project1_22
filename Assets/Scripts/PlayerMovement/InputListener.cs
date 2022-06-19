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
    public string PickaxeButton = "3";
    public string AxeButton = "4";

    void Start(){
        player = GameObject.Find("Player(Clone)");
    }


    //call gameobject methods here
    void Update()
    {
        if(Input.GetKeyDown(ToggleSwordMode)){
            player.transform.GetChild(2).gameObject.SetActive(false);
            player.transform.GetChild(1).gameObject.SetActive(false);
            player.transform.GetChild(0).gameObject.SetActive(!player.transform.GetChild(0).gameObject.activeSelf); 
        }

        if(Input.GetKeyDown(RangedAttackButton)){
            player.GetComponent<PlayerCombat>().RangedAttack();
        }

        if(Input.GetKeyDown(PickaxeButton)){
            player.transform.GetChild(0).gameObject.SetActive(false);
            player.transform.GetChild(2).gameObject.SetActive(false);
            player.transform.GetChild(1).gameObject.SetActive(!player.transform.GetChild(1).gameObject.activeSelf);
        }

        if(Input.GetKeyDown(AxeButton)){
            player.transform.GetChild(0).gameObject.SetActive(false);
            player.transform.GetChild(1).gameObject.SetActive(false);
            player.transform.GetChild(2).gameObject.SetActive(!player.transform.GetChild(2).gameObject.activeSelf);
        }
        
    }
}
