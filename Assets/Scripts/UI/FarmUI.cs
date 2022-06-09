using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FarmUI : MonoBehaviour
{
    public CropObject crop;
    FarmingSystem fm;

    [SerializeField] public TMP_Text nameTxt;
    [SerializeField] TMP_Text priceTxt;
    [SerializeField] Image UIImage;
    GameObject farmingPanel;

    // Start is called before the first frame update

    void Start()
    {
        fm = FindObjectOfType<FarmingSystem>();
        farmingPanel = GameObject.Find("FarmingPanel");
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

    public void OnButtonDown()
    {
        farmingPanel.SetActive(false);
    }
}