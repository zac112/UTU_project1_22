using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingPlacementSystem : MonoBehaviour
{
    [SerializeField] Transform buildingToPlace;
    [SerializeField] List<Transform> buildableBuildings;
    List<Vector3> tilesOccupiedByBuilding;

    [Range(0,2)]
    [SerializeField] int buildBuildingMouseButton = 1;
    [SerializeField] KeyCode buildingDeselect;
    [SerializeField] KeyCode building1Hotkey;
    [SerializeField] KeyCode building2Hotkey;

    [SerializeField] Tilemap tilemap;
    Transform selectedBuilding;

    void Start()
    {
        // TODO: Find a more reliable way to find Tilemap component that does not break every time names are changed
        tilemap = GameObject.Find("Grid(Clone)").GetComponentInChildren<Tilemap>();
        tilesOccupiedByBuilding = new List<Vector3>();
    }

    void Update()
    {
        SelectBuilding();

        if (Input.GetMouseButtonDown(buildBuildingMouseButton) && selectedBuilding != null) 
        {
            // Get selected buildings width and height
            // Testing with GoldMineScript
            // TODO: Get the script attached to the individual selectedBuilding
            IBuildable script = selectedBuilding.GetComponent<IBuildable>();
            int buildingWidth = script.Width;
            int buildingLength = script.Length;

            Vector3 tileLocationInWorld = GetTileLocationUnderMouse();
            //Debug.Log(tileLocationInWorld);

            // Calculate tile coordinates that the building will occupy based on buildingWidth and buildingLength
            // Currently, moving NW will modify X by -0.50 and Y by +0.25
            // Moving NE will modify X by +0.50 and Y by +0.25

            // Loop through width and height and add these tiles to tilesOccupiedByBuilding
            float currentX;
            float currentY;
            
            for (int width = 0; width < buildingWidth; width++) 
            {
                for (int length = 0; length < buildingLength; length++) 
                {
                    currentX = tileLocationInWorld.x + 0.50f * width;
                    currentY = tileLocationInWorld.y + 0.25f * length;

                    script.OccupiedTiles.Add(new Vector3(currentX, currentY, 0));
                    //occupiedTiles.Add(new Vector3(currentX, currentY, 0));
                }
            }

            // PrintTilesOccupiedByBuildingList();

            // Copy tileLocationInWorld, so that we can use it in occupiedTiles, without the change in z-axis
            Vector3 originalTileLocationInWorld = tileLocationInWorld;

            // Set Z to avoid buildings being underground
            tileLocationInWorld.z = 20;

            // Add tiles to occupiedTiles
            // TODO: Add all tiles occupied by the building
            // OccupyTiles(script.OccupiedTiles);

            // Instantiate building on tileLocation
            // TODO: Instantiate building on the center point of the buildings occupied tiles
            Instantiate(selectedBuilding, calculateBuildingLocation(script.OccupiedTiles), Quaternion.identity);

            Debug.Log(script.OccupiedTiles.Count);

            Debug.Log(calculateBuildingLocation(script.OccupiedTiles));

            GameStats.BuildingsBuilt++;  // increase GameStats record of finished buildings
            
            /*
            for (int i = 0; i < occupiedTiles.Count; i++) 
            {
                Debug.Log(occupiedTiles[i].ToString());
            }
            */

        }
    }

    Vector3 GetTileLocationUnderMouse()
    {
        Vector3 mousePosition = GetMousePosition();
        Vector3Int tileLocation = GetTileLocation();
        //Debug.Log("Tilelocation:" + tileLocation);

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

    public Vector3Int GetTileLocation() 
    {
        // Get the location of the tile under the mouse
        Vector3Int tileLocation = tilemap.WorldToCell(GetMousePosition());
        return tileLocation;
    }

    public Vector3 GetMousePosition() 
    {
        // Get the mouse position and store it in a variable
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePosition;
    }

    private void printTilesOccupiedByBuildingList()
    {
        foreach (Vector3 tile in tilesOccupiedByBuilding)
        {
            Debug.Log(tile);
        }
    }

    private Vector3 calculateBuildingLocation(List<Vector3> occupiedTiles) 
    {
        float xPosition = 0;
        float yPosition = 0;

        for (int i = 0; i < occupiedTiles.Count; i++) 
        {
            xPosition += occupiedTiles[i].x;
            yPosition += occupiedTiles[i].y;
        }

        xPosition /= occupiedTiles.Count;
        yPosition /= occupiedTiles.Count;

        return new Vector3(xPosition, yPosition, 10);
    }
}
