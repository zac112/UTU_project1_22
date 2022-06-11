using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

 
public class TileChanger : MonoBehaviour
{
    public Tile tile;
    Vector3Int location;
    Tilemap tilemap;
    [SerializeField] GameObject plot;
    PlayerStats player;

    private int hoeToolPrice = 50;
    private int roadTilePrice = 100;

    [SerializeField] BuildingPlacementSystem bps;

    void Start()
    {
        tilemap = GameObject.Find("Grid(Clone)").GetComponentInChildren<Tilemap>();
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    void Update()
    {
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        location = tilemap.WorldToCell(mp);
        location.z = 0;
        location.x = location.x -5;
        location.y = location.y -5;

        // Get instantiated tile GameObject
        // This gets called every update, maybe change to on mouseclick
        GameObject groundTile = tilemap.GetInstantiatedObject(location);
        
        if (groundTile != null) {
            // Get the script attached to the GameObject
            GroundTileData tileScript = groundTile.GetComponent<GroundTileData>();

            // For now player can only put FarmTiles on grass
            if (Input.GetMouseButtonDown(0) && tilemap.GetTile(location).name.Equals("GrassTile") && tile.name.Equals("FarmTile") && player.GetGold() >= hoeToolPrice && !tileScript.isOccupied)
            {
                tilemap.SetTile(location, tile);
                tileScript.isOccupied = true;
            }
            // Place crops only on FarmTile and remove gold from player
            else if (tilemap.GetTile(location).name.Equals("FarmTile") && Input.GetMouseButtonDown(0) && player.GetGold() >= hoeToolPrice)
            {
                player.RemoveGold(hoeToolPrice);
                AddCrop();
            }

            // If player is building road --> can also build on water to make bridges but not on placed FarmTiles
            // Remove gold from player
            else if (Input.GetMouseButtonDown(0) && tile.name.Equals("RoadTile") && tilemap.GetTile(location).name != ("FarmTile") && player.GetGold() >= roadTilePrice && !tileScript.isOccupied)
            {
                tilemap.SetTile(location, tile);
                player.RemoveGold(roadTilePrice);
                tileScript.isOccupied = true;
            }
        }
        
    }

    private void AddCrop()
    { 
        Vector3 location = GetTileRealPosition();
        Instantiate(plot,location,transform.rotation);
        plot.gameObject.SetActive(true);
    }


    private Vector3 GetTileRealPosition()
    {
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = tilemap.WorldToCell(mp);
        mp = tilemap.GetCellCenterWorld(cellPosition);
        mp.z = 0;

        return mp;
    }
}

