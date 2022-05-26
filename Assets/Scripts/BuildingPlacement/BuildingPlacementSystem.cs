using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingPlacementSystem : MonoBehaviour
{
    [SerializeField] Transform buildingToPlace;
    [SerializeField] List<Transform> buildableBuildings;
    public List<(int, int)> occupiedTiles;

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
        occupiedTiles = new List<(int, int)>();
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
        // Get the mouse position and store it in a variable
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get the location of the tile under the mouse
        Vector3Int tileLocation = tilemap.WorldToCell(mousePosition);

        // Get the center of the tile under the mouse?
        // Perhaps a bit redundant to first get the tile location in the tilemap and then turn it back to world position?
        Vector3 tileLocationInWorld = tilemap.GetCellCenterWorld(tileLocation);

        // Get tile gameobject
        Debug.Log(tileLocation);
        

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

    
    public void OccupyTile((int, int) tileXY) 
    { 
        if (!occupiedTiles.Contains(tileXY)) 
        {
            occupiedTiles.Add(tileXY);
        }
        else 
        {
            Debug.Log("occupiedTiles already contains tileXY");
        }
    }

    public void RemoveFromOccupiedTiles((int, int) tileXY) 
    {
        if (!occupiedTiles.Contains(tileXY))
        {
            Debug.Log("tileXY does not exist in the list");
        }
        else 
        {
            occupiedTiles.Remove(tileXY);
        }
    }
}
