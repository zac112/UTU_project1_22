using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotSystem : MonoBehaviour
{

    bool isPlanted = false;
    int cropPhases = 0;
    float timer;

    SpriteRenderer crop;
    SpriteRenderer plot;
    PolygonCollider2D cropCollider;
    CropObject selectedCrop;

    public Color availableColor = Color.green;
    public Color unavailableColor = Color.red;

    PlayerStats player;
    UIManager UiManager;
    BuildingPlacementSystem bps;
    InventoryManager im;
    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        crop = transform.GetChild(0).GetComponent<SpriteRenderer>();
        cropCollider = transform.GetChild(0).GetComponent<PolygonCollider2D>();
        UiManager = FindObjectOfType<UIManager>();
        plot = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player(Clone)").GetComponent<PlayerStats>();
        bps = GameObject.Find("BuildingPlacementSystem").GetComponent<BuildingPlacementSystem>();
        im = GameObject.Find("InventorySystem").GetComponent<InventoryManager>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlanted)
        {
            timer -= Time.deltaTime;

            if (timer < 0 && cropPhases<selectedCrop.cropPhases.Length-1)
            {
                timer = selectedCrop.growTimeBtw;
                cropPhases++;
                Grow();
            }
        }
    }

    private void OnMouseDown()
    {
        if (isPlanted)
        {
            if (cropPhases == selectedCrop.cropPhases.Length - 1 && !UiManager.isPlanting)
            {
                Collect();
                im.AddItem(selectedCrop.cropName, 1);
                GetComponent<AudioPlayer>().Play(AudioType.Build, 0);
            }
        }
        else if(UiManager.isPlanting &&UiManager.selectCrop.crop.price <= player.GetGold())
        {
            {
                Plant(UiManager.selectCrop.crop);
                player.RemoveGold(UiManager.selectCrop.crop.price);
                GetComponent<AudioPlayer>().Play(AudioType.Build, 1);
            }
        }
    }

    private void OnMouseOver()
    {
        if (UiManager.isPlanting)
        {
            if(isPlanted)
            {
                plot.color = unavailableColor;
            }
            else
            {
                plot.color = availableColor;
            }
        }
    }

    private void OnMouseExit()
    {
        plot.color = Color.white;
    }


    void Collect()
    {
        isPlanted = false;
        crop.gameObject.SetActive(false);
    }

    void Plant(CropObject newCrop)
    {
        selectedCrop = newCrop;
        isPlanted = true;
        cropPhases = 0;
        Grow();
        timer = selectedCrop.growTimeBtw;
        crop.gameObject.SetActive(true);
    }

    void Grow()
    {
        crop.sprite = selectedCrop.cropPhases[cropPhases];
    }



}   

