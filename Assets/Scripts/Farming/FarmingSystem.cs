using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmingSystem : MonoBehaviour
{
    public FarmUI selectCrop;
    public bool isPlanting = false;


    public void SelectCrop(FarmUI newCrop)
    {
        if(selectCrop == newCrop)
        {
            selectCrop = null;
            isPlanting = false;
            
        }
        else
        {
            selectCrop = newCrop;
            isPlanting = true;
        }
    }
}
