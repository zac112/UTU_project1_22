using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] Transform buildingToPlace;
    Tilemap tilemap;

    void Start()
    {
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            // Get the mouse position and store it as a variable
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Get the location of the tile under the mouse
            Vector3Int tileLocation = tilemap.WorldToCell(mousePosition);

            // Get the center of the tile under the mouse?
            // Perhaps a bit redundant to first get the tile location in the tilemap and then turn it back to world position?
            Vector3 tileLocationInWorld = tilemap.GetCellCenterWorld(tileLocation);

            // Set Z to 0, to avoid being underground
            tileLocationInWorld.z = 0;

            // Instantiate building on tileLocation
            Transform building = Instantiate(buildingToPlace, tileLocationInWorld, Quaternion.identity);
        }
    }
}
