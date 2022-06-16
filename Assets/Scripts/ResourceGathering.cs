using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGathering : MonoBehaviour
{
    [SerializeField] public ResourceNode res;
    [SerializeField] public int goldGatherSpeed;
    [SerializeField] public int woodGatherSpeed;

    InventoryManager inventoryManager;
    PlayerStats player;

    void Start()
    {
        // For now lets give some exmaple values for gathering
        goldGatherSpeed = 5;
        woodGatherSpeed = 2;

        // Access other scripts
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
        inventoryManager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>();
    }

    // Checking which tool is selected
    void Update(){
        if(gameObject.name.Equals("Pickaxe")){
            res = GameObject.FindWithTag("GoldNode").GetComponent<ResourceNode>();
        }

        else if(gameObject.name.Equals("Axe")){
            res = GameObject.FindWithTag("Tree").GetComponent<ResourceNode>();
        }   
    }

    // Gather resource when inside collider
    void OnTriggerEnter2D(Collider2D resource)
    {
        if(resource.tag == "GoldNode"){
            res.Gather(goldGatherSpeed);
            player.AddGold(goldGatherSpeed);

        }
        if(resource.tag == "Tree"){
            res.Gather(woodGatherSpeed);
            inventoryManager.AddItem("Wood", goldGatherSpeed);
        }
    }
}
