using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDataTest : MonoBehaviour
{
    BuildingPlacementSystem bps;
    Tilemap tilemap;
    Grid grid;

    private void Start()
    {
        grid = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Grid>();
        tilemap = grid.GetComponentInChildren<Tilemap>();
        bps = GameObject.Find("BuildingPlacementSystem").GetComponent<BuildingPlacementSystem>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            Vector3Int CellLocation = bps.GetTileCellLocation();
            TileBase tile = tilemap.GetTile(CellLocation);

            Debug.Log(tile);
        }
    }
}
