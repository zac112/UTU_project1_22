using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FarmUI : MonoBehaviour
{
    public CropObject crop;
    FarmingSystem fm;

    [SerializeField] TMP_Text nameTxt;
    [SerializeField] TMP_Text priceTxt;
    [SerializeField] Image UIImage;


    // Start is called before the first frame update
    void Start()
    {
        fm = FindObjectOfType<FarmingSystem>();
        updateUI();
    }

    public void BuyCrop()
    {
        fm.SelectCrop(this);
    }

    void updateUI()
    {
        nameTxt.text = crop.cropName;
        priceTxt.text = "Cost:" + crop.price;
    }
}