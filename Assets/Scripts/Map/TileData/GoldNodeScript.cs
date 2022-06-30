using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldNodeScript : MonoBehaviour
{
    private List<GameObject> attachedGoldMines = new List<GameObject>();

    public List<GameObject> GetAttachedGoldMines() { return attachedGoldMines; }

    public void AddToAttachedGoldMines(GameObject goldmine)
    {
        if (!attachedGoldMines.Contains(goldmine) && goldmine != null) attachedGoldMines.Add(goldmine);
    }

    public void RemoveFromAttachedGoldmines(GameObject goldmine)
    {
        attachedGoldMines.Remove(goldmine);
    }

    public void UpdateAttachedGoldMinesEfficiency()
    {
        foreach (GameObject goldmine in attachedGoldMines)
        {
            if (goldmine == null)  // nullcheck for destroyed mines
            {
                attachedGoldMines.Remove(goldmine);
                continue;
            }
            GoldMineScript gms = goldmine.GetComponent<GoldMineScript>();
            gms.DefineMiningEfficiency();
        }
    }

}
