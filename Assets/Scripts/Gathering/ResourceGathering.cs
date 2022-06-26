using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceGathering : MonoBehaviour
{
    [SerializeField] ResourceNode res;
    [SerializeField] GameObject resourceInfo;
    [SerializeField] int goldGatherSpeed;
    [SerializeField] int woodGatherSpeed;
    
    TMP_Text infoText;
    GameObject canvas;
    InventoryManager inventoryManager;
    PlayerStats player;
    AudioSource source;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        inventoryManager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>();
        canvas = GameObject.Find("Canvas");
        resourceInfo = canvas.transform.GetChild(7).gameObject;
        infoText = resourceInfo.GetComponent<TMP_Text>();
        source = GetComponent<AudioSource>();
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
            GetComponent<AudioPlayer>().Play(AudioType.Build, 0);
            player.AddGold(goldGatherSpeed);

        }
        if(resource.tag == "Tree" && gameObject.name.Equals("Axe")){

            res = resource.gameObject.GetComponent<ResourceNode>();
            resourceInfo.SetActive(true);
            infoText.text = $"Resource left:{res.capacity}";

            // Gather from node and add to inventory
            res.Gather(woodGatherSpeed);
            GetComponent<AudioPlayer>().Play(AudioType.Build, 0);
            player.AddWood(woodGatherSpeed);
        }
    }
    
    // Set infopanel unactive;
    void OnTriggerExit2D(Collider2D resource){
        resourceInfo.SetActive(false);
    }
}
