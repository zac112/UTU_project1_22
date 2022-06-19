using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    //add gameobjects that require inputs here
    GameObject player;

    //set keybinds here
    string ToggleSwordMode = "1";
    string RangedAttackButton = "2";
    public string PickaxeButton = "3";
    public string AxeButton = "4";

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }


    //call gameobject methods here
    void Update()
    {
        if (Input.GetKeyDown(ToggleSwordMode)) ToggleSword();
        if (Input.GetKeyDown(RangedAttackButton)) ToggleBow();
        if (Input.GetKeyDown(PickaxeButton)) TogglePick();
        if (Input.GetKeyDown(AxeButton)) ToggleAxe();
    }

    public void ToggleSword() {
        Toggle(0);        
    }

    public void ToggleBow() {
        player.GetComponent<PlayerCombat>().RangedAttack();
    }
    public void TogglePick()
    {
        Toggle(1);
    }
    public void ToggleAxe()
    {
        Toggle(2);
    }

    private void Toggle(int child) {
        List<GameObject> children = Children();
        foreach (GameObject go in children) go.SetActive(false);
        children[child].SetActive(!children[child].activeSelf);

    }

    private List<GameObject> Children() {
        List<GameObject> res = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++) {
            res.Add(transform.GetChild(i).gameObject);
        }
        return res;
    }
}
