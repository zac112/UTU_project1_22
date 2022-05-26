using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingPlacementSystem : MonoBehaviour
{
    [SerializeField] Transform buildingToPlace;
    [SerializeField] List<Transform> buildableBuildings;
    public List<(float, float)> occupiedTiles;

    [Range(0,2)]
    [SerializeField] int buildBuildingMouseButton = 1;
    [SerializeField] KeyCode buildingDeselect;
    [SerializeField] KeyCode building1Hotkey;
    [SerializeField] KeyCode building2Hotkey;

    Tilemap tilemap;
    Transform selectedBuilding;

    

    void Start()
    {
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        occupiedTiles = new List<(float, float)>();
    }

    void Update()
    {
        SelectBuilding();

        if (Input.GetMouseButtonDown(buildBuildingMouseButton) && selectedBuilding != null) 
        {
            Vector3 tileLocationInWorld = GetTileLocationUnderMouse();

            // Set Z to avoid buildings being underground
            tileLocationInWorld.z = 10;

            // Instantiate building on tileLocation
            Instantiate(selectedBuilding, tileLocationInWorld, Quaternion.identity);
        }
    }

    Vector3 GetTileLocationUnderMouse()
    {
        Vector3 mousePosition = GetMousePosition();
        Vector3Int tileLocation = GetTileLocation();

        // Get the center of the tile under the mouse?
        // Perhaps a bit redundant to first get the tile location in the tilemap and then turn it back to world position?
        Vector3 tileLocationInWorld = tilemap.GetCellCenterWorld(tileLocation);

        return tileLocationInWorld;
    }

    void SelectBuilding() 
    {
        if (Input.GetKeyDown(buildingDeselect))
        {
            selectedBuilding = null;
        }

        else if (Input.GetKeyDown(building1Hotkey))
        {
            selectedBuilding = buildableBuildings[0];
        }

        else if (Input.GetKeyDown(building2Hotkey))
        {
            selectedBuilding = buildableBuildings[1];
        }
    }


    private void OccupyTile(Vector3 tileLocation) 
    {
        (float, float) tileXY = GetXY(tileLocation);

        if (!occupiedTiles.Contains(tileXY)) 
        {
            occupiedTiles.Add(tileXY);
        }
        else 
        {
            Debug.Log("occupiedTiles already contains this tile");
        }
    }

    private void RemoveFromOccupiedTiles(Vector3 tileLocation) 
    {
        (float, float) tileXY = GetXY(tileLocation);

        if (!occupiedTiles.Contains(tileXY))
        {
            Debug.Log("occupiedTiles does not contain this tile");
        }
        else 
        {
            occupiedTiles.Remove(tileXY);
        }
    }

    private Vector3Int GetTileLocation() 
    {
        // Get the location of the tile under the mouse
        Vector3Int tileLocation = tilemap.WorldToCell(GetMousePosition());
        return tileLocation;
    }

    private Vector3 GetMousePosition() 
    {
        // Get the mouse position and store it in a variable
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePosition;
    }

    private bool IsTileOccupied(Vector3 tileLocation) 
    {
        if (occupiedTiles.Contains(GetXY(tileLocation)))
        {
            return true;
        }
        else return false;
    }

    private (float, float) GetXY(Vector3 tileLocation) 
    {
        // Make Vector3 into a tuple, so that we can ignore z-coordinates (just in case?)
        (float, float) tileXY = (tileLocation.x, tileLocation.y);
        return tileXY;
    }
}
