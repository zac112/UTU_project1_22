using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundTileData : MonoBehaviour
{
    Tilemap tilemap;
    Grid grid;

    private void Start()
    {
        grid = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Grid>();
        tilemap = grid.GetComponentInChildren<Tilemap>();
    }

    public GroundTile GetGroundTile(Vector3Int position) 
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int location = tilemap.WorldToCell(mousePosition);
        location.z = 0;

        return tilemap.GetTile<GroundTile>(location);
    }
}
