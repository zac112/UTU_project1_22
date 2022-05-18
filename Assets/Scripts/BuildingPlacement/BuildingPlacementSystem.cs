using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingPlacementSystem : MonoBehaviour
{
    [SerializeField] Transform buildingToPlace;
    [SerializeField] List<Transform> buildableBuildings;

    Tilemap tilemap;
    Transform selectedBuilding;

    void Start()
    {
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
    }

    void Update()
    {
        SelectBuilding();

        if (Input.GetMouseButtonDown(1) && selectedBuilding != null) 
        {
            Vector3 tileLocationInWorld = GetTileOnMouse();

            // Instantiate building on tileLocation
            Instantiate(selectedBuilding, tileLocationInWorld, Quaternion.identity);
        }
    }

    Vector3 GetTileOnMouse()
    {
        // Get the mouse position and store it in a variable
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get the location of the tile under the mouse
        Vector3Int tileLocation = tilemap.WorldToCell(mousePosition);

        // Get the center of the tile under the mouse?
        // Perhaps a bit redundant to first get the tile location in the tilemap and then turn it back to world position?
        Vector3 tileLocationInWorld = tilemap.GetCellCenterWorld(tileLocation);

        // Set Z to 0, to avoid building being underground
        tileLocationInWorld.z = 0;

        return tileLocationInWorld;
    }

    void SelectBuilding() 
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            selectedBuilding = null;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            selectedBuilding = buildableBuildings[0];
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            selectedBuilding = buildableBuildings[1];
        }
    }
}
