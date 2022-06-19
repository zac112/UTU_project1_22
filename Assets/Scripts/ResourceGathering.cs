using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceGathering : MonoBehaviour
{
    [SerializeField] public ResourceNode res;
    [SerializeField] public GameObject resourceInfo;
    [SerializeField] public int goldGatherSpeed;
    [SerializeField] public int woodGatherSpeed;
    
    TMP_Text infoText;
    GameObject canvas;
    InventoryManager inventoryManager;
    PlayerStats player;

    void Start()
    {
        // For now lets give some exmaple values for gathering
        goldGatherSpeed = 5;
        woodGatherSpeed = 2;

        player = GameObject.Find("Player(Clone)").GetComponent<PlayerStats>();
        inventoryManager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>();
        canvas = GameObject.Find("Canvas");
        resourceInfo = canvas.transform.GetChild(9).gameObject;
        infoText = resourceInfo.GetComponent<TMP_Text>();
    }

    // Gather resource when inside collider
    void OnTriggerEnter2D(Collider2D resource)
    {
        if(resource.tag == "GoldNode" && gameObject.name.Equals("Pickaxe")){

            // Getting the gameobject that the pickaxe collided with and setting infopanel active/updating
            res = resource.gameObject.GetComponent<ResourceNode>();
            resourceInfo.SetActive(true);
            infoText.text = $"Resource left:{res.capacity}";
            
            // Gather from node and add gold to player
            res.Gather(goldGatherSpeed);
            player.AddGold(goldGatherSpeed);

        }
        if(resource.tag == "Tree" && gameObject.name.Equals("Axe")){

            res = resource.gameObject.GetComponent<ResourceNode>();
            resourceInfo.SetActive(true);
            infoText.text = $"Resource left:{res.capacity}";

            // Gather from node and add to inventory
            res.Gather(woodGatherSpeed);
            inventoryManager.AddItem("Wood", goldGatherSpeed);
        }
    }
    
    // Set infopanel unactive;
    void OnTriggerExit2D(Collider2D resource){
        resourceInfo.SetActive(false);
    }
}
