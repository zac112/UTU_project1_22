using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChanger : MonoBehaviour
{
    public Tile tile;
    public Vector3Int location;
    Tilemap tilemap;
    public GameObject plot;
    PlayerStats player;
    private int hoeToolPrice = 50;

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

        if (player.GetGold() >= hoeToolPrice){
            // For now player can only put farming tiles on grass
            if (Input.GetMouseButtonDown(0) && tilemap.GetTile(location).name.Equals("grassTiles1"))
            {
                tilemap.SetTile(location, tile);

                // Place crops only on farmingTile
                if (tilemap.GetTile(location).name.Equals("farmingTile"))
                {
                    player.RemoveGold(hoeToolPrice);
                    AddCrop();
            
                }
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

