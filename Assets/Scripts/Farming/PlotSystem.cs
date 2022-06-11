using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotSystem : MonoBehaviour
{

    bool isPlanted = false;
    SpriteRenderer crop;
    PolygonCollider2D cropCollider;

    int cropPhases = 0;
    float timer;

    SpriteRenderer plot;
    CropObject selectedCrop;
    FarmingSystem farming;

    public Color availableColor = Color.green;
    public Color unavailableColor = Color.red;

    public PlayerStats player;

    BuildingPlacementSystem bps;

    // Start is called before the first frame update
    void Start()
    {
        crop = transform.GetChild(0).GetComponent<SpriteRenderer>();
        cropCollider = transform.GetChild(0).GetComponent<PolygonCollider2D>();
        farming = FindObjectOfType<FarmingSystem>();
        plot = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
        bps = GameObject.Find("BuildingPlacementSystem").GetComponent<BuildingPlacementSystem>();
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
            if (cropPhases == selectedCrop.cropPhases.Length - 1 && !farming.isPlanting)
            {
                Collect();
            }
        }
        else if(farming.isPlanting && farming.selectCrop.crop.price <= player.GetGold())
        {
            {
                Plant(farming.selectCrop.crop);
                player.RemoveGold(farming.selectCrop.crop.price);
            }
        }
    }

    private void OnMouseOver()
    {
        if (farming.isPlanting)
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

