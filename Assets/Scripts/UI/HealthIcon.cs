using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIcon : MonoBehaviour{
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image image;

    public void UpdateIcon(float percentage){
        if(percentage <= 0.25) image.sprite = sprites[0];
        else if(percentage <= 0.50) image.sprite = sprites[1];
        else if(percentage <= 0.75) image.sprite = sprites[2];
        else image.sprite = sprites[3];
    }
}
