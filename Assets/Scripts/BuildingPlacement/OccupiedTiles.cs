using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccupiedTiles : MonoBehaviour
{
    private List<Vector3> occupiedTiles;

    private void Start()
    {
        occupiedTiles = new List<Vector3>();
    }

    public void OccupyTile(Vector3 tileLocation)
    {
        if (!occupiedTiles.Contains(tileLocation))
        {
            occupiedTiles.Add(tileLocation);
        }
        else
        {
            Debug.Log("occupiedTiles already contains this tile");
        }
    }

    public void OccupyTiles(List<Vector3> tileLocations)
    {
        for (int i = 0; i < tileLocations.Count; i++)
        {
            if (!occupiedTiles.Contains(tileLocations[i]))
            {
                occupiedTiles.Add(tileLocations[i]);
            }
            else
            {
                Debug.Log($"occupiedTiles already contains this tile [{i}]");
            }
        }
    }

    public void RemoveFromOccupiedTiles(Vector3 tileLocation)
    {
        if (!occupiedTiles.Contains(tileLocation))
        {
            Debug.Log("occupiedTiles does not contain this tile");
        }
        else
        {
            occupiedTiles.Remove(tileLocation);
        }
    }

    public bool IsTileOccupied(Vector3 tileLocation)
    {
        if (occupiedTiles.Contains(tileLocation))
        {
            return true;
        }
        else return false;
    }

    public void UpdateOccupiedMarkers() 
    { 
        
    }
}
