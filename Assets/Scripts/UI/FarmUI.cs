using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FarmUI : MonoBehaviour
{
    public CropObject crop;
    UIManager UiManager;

    [SerializeField] public TMP_Text nameTxt;
    [SerializeField] TMP_Text priceTxt;
    [SerializeField] Image UIImage;
    GameObject farmingPanel;

    // Start is called before the first frame update

    void Start()
    {
        UiManager = FindObjectOfType<UIManager>();
        farmingPanel = GameObject.Find("FarmingPanel");
        updateUI();
    }

    public void BuyCrop()
    {
        UiManager.SelectCrop(this);
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