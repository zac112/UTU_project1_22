using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class InputListener : NetworkBehaviour
{
    //set keybinds here
    string ToggleSwordMode = "1";
    string RangedAttackButton = "2";
    public string PickaxeButton = "3";
    public string AxeButton = "4";

    NetworkVariable<int> weapon = new NetworkVariable<int>();

    void Start()
    {
        weapon.OnValueChanged += ToggleWeapon;
        GameObject.FindObjectOfType<UIManager>().RegisterWeaponChanges(this);
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
        Toggle(1);
    }

    public void ToggleBow() {        
        
    }
    public void TogglePick()
    {
        Toggle(2);
    }
    public void ToggleAxe()
    {
        Toggle(3);
    }

    private void Toggle(int child) {
        if (!NetworkObject.IsLocalPlayer) return;
        //if (!NetworkManager.IsHost) ToggleWeapon(child);

        ToggleWeaponServerRpc(child==weapon.Value ? 0 : child);
    }

    private List<GameObject> Weapons() {
        List<GameObject> res = new List<GameObject>();
        Transform t = transform.Find("Weapons");
        for (int i = 0; i < t.childCount; i++) {
            res.Add(t.GetChild(i).gameObject);
        }
        return res;
    }

    void ToggleWeapon(int oldweapon, int newweapon) {
        List<GameObject> children = Weapons();
        foreach (GameObject go in children) go.SetActive(false);
        children[newweapon].SetActive(true);
    }
    [ServerRpc]
    public void ToggleWeaponServerRpc(int child) {
        weapon.Value = child;
    }
}
