using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldNodeScript : MonoBehaviour
{
    private List<GameObject> attachedGoldMines = new List<GameObject>();

    public List<GameObject> GetAttachedGoldMines() { return attachedGoldMines; }

    public void AddToAttachedGoldMines(GameObject goldmine)
    {
        if (!attachedGoldMines.Contains(goldmine)) attachedGoldMines.Add(goldmine);
    }

    public void UpdateAttachedGoldMinesEfficiency()
    {
        foreach (GameObject goldmine in attachedGoldMines)
        {
            GoldMineScript gms = goldmine.GetComponent<GoldMineScript>();
            gms.DefineMiningEfficiency();
        }
    }


}
