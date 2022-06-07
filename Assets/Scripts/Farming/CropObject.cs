using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName ="New Crop", menuName ="Crop")]
public class CropObject : ScriptableObject
{
    public string cropName;
    public Sprite[] cropPhases;
    public float growTimeBtw;
    public int price;
}
