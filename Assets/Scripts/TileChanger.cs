using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChanger : MonoBehaviour
{
    public bool isActive = false;

    public Tilemap tilemap;

    public Tile tile;

    public Vector3Int location;

    public BuildingPlacementSystem bps;

 
    void Update()
    {
        // Get tilemap and buildinplacementsystem
        tilemap = GameObject.Find("Grid(Clone)").GetComponentInChildren<Tilemap>();
        bps = GameObject.Find("BuildingPlacementSystem").GetComponent<BuildingPlacementSystem>();

        // If mouse clicked change tile to given tile
        // HAS TO BE FIXED (can now change water to any tile)
        if (isActive && Input.GetMouseButton(0)){
            location = bps.GetTileCellLocation();
            location.z = 0;
            location.x = location.x -5;
            location.y = location.y -5;
            tilemap.SetTile(location, tile);
        }
    }
}