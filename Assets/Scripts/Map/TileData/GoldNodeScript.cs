using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldNodeScript : MonoBehaviour
{
    private List<GameObject> attachedGoldMines = new List<GameObject>();

    public List<GameObject> GetAttachedGoldMines() { return attachedGoldMines; }

}
