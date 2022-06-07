using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDataTest : MonoBehaviour
{
    BuildingPlacementSystem bps;
    Tilemap tilemap;
    Grid grid;

    [SerializeField] GameObject circlePrefab;

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
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int location = tilemap.WorldToCell(mp);
            Vector3 CellCenter = tilemap.GetCellCenterWorld(location);
            Vector3Int center = tilemap.WorldToCell(CellCenter);

            /*
            if (tilemap.GetTile(center))
            {
                Debug.Log("Tile");
            }
            else 
            {
                Debug.Log("No tile");
            }
            */
        }
    }
}
