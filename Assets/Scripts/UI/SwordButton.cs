using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SwordButton : MonoBehaviour
{
    [SerializeField] Image UIImage;
    [SerializeField] TMP_Text UIName;
    [SerializeField] TMP_Text UICost;
    [SerializeField] TMP_Text UIDamage;
    SpriteRenderer sr;
    GameObject player;
    GameObject sword;
    GameObject weapons;
    PlayerStats ps;
    public Weapon weapon;

    void Start(){

        // Get Sword gameobject
        player = GameObject.Find("Player(Clone)");
        weapons = player.transform.GetChild(3).gameObject;
        sword = weapons.transform.GetChild(1).gameObject;
        sr = sword.GetComponent<SpriteRenderer>();

        ps = player.GetComponent<PlayerStats>();

        // Update UI with weapon class values
        UpdateUI();
    }

    public void UpdateUI(){
        UIImage.sprite = weapon.sprite;
        UIName.text = "Name: " + weapon.name;
        UIDamage.text = "Damage: " + weapon.damage;
        UICost.text = "Cost: " + weapon.cost;
    }

    // Remove player gold and switch sprite
    public void BuySword()
    {
        if (ps.GetGold() >= weapon.cost){
            ps.RemoveGold(weapon.cost);
            sr.sprite = weapon.sprite;
            
            // Remove bought sword from shop
            gameObject.SetActive(false);
        }
    }
}
