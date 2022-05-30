using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingPlacementSystem : MonoBehaviour
{
    [SerializeField] Transform buildingToPlace;
    [SerializeField] List<Transform> buildableBuildings;
    public List<Vector3> occupiedTiles;
    List<Vector3> tilesOccuipedByBuilding;

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
        occupiedTiles = new List<Vector3>();
        tilesOccuipedByBuilding = new List<Vector3>();
    }

    void Update()
    {
        SelectBuilding();

        if (Input.GetMouseButtonDown(buildBuildingMouseButton) && selectedBuilding != null) 
        {
            // Get selected buildings width and height
            // Testing with GoldMineScript
            // TODO: Get the script attached to the individual selectedBuilding
            GoldMineScript script = selectedBuilding.GetComponent<GoldMineScript>();
            int buildingWidth = script.Width;
            int buildingLength = script.Length;

            Vector3 tileLocationInWorld = GetTileLocationUnderMouse();
            //Debug.Log(tileLocationInWorld);

            // Calculate tile coordinates that the building will occupy based on buildingWidth and buildingLength
            // Currently, moving NW will modify X by -0.50 and Y by +0.25
            // Moving NE will modify X by +0.50 and Y by +0.25
            //float targetX = tileLocationInWorld.x - (0.50f * buildingWidth);
            //float targetY = tileLocationInWorld.y + (0.25f * buildingLength);

            // Loop through width and height and add these tiles to tilesOccupiedByBuilding
            float currentX;
            float currentY;
            
            for (int width = 0; width < buildingWidth; width++) 
            {
                for (int length = 0; length < buildingLength; length++) 
                {
                    currentX = tileLocationInWorld.x + 0.50f * width;
                    currentY = tileLocationInWorld.y + 0.25f * length;

                    tilesOccuipedByBuilding.Add(new Vector3(currentX, currentY, 0));
                }
            }

            // PrintTilesOccupiedByBuildingList();

            // Copy tileLocationInWorld, so that we can use it in occupiedTiles, without the change in z-axis
            Vector3 originalTileLocationInWorld = tileLocationInWorld;

            // Set Z to avoid buildings being underground
            tileLocationInWorld.z = 10;

            // Instantiate building on tileLocation
            Instantiate(selectedBuilding, tileLocationInWorld, Quaternion.identity);

            // Add tiles to occupiedTiles
            OccupyTile(originalTileLocationInWorld);
            
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


    public void OccupyTile(Vector3 tileLocation) 
    {
        if (!occupiedTiles.Contains(tileLocation)) 
        {
            occupiedTiles.Add(tileLocation);
        }
        else 
        {
            Debug.Log("occupiedTiles already contains this tile");
        }
    }

    public void RemoveFromOccupiedTiles(Vector3 tileLocation) 
    {
        if (!occupiedTiles.Contains(tileLocation))
        {
            Debug.Log("occupiedTiles does not contain this tile");
        }
        else 
        {
            occupiedTiles.Remove(tileLocation);
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

    public bool IsTileOccupied(Vector3 tileLocation) 
    {
        if (occupiedTiles.Contains(tileLocation))
        {
            return true;
        }
        else return false;
    }

    /*
    public (float, float) GetXY(Vector3 tileLocation) 
    {
        // Make Vector3 into a tuple, so that we can ignore z-coordinates (just in case?)
        (float, float) tileXY = (tileLocation.x, tileLocation.y);
        return tileXY;
    }
    */

    private void PrintTilesOccupiedByBuildingList()
    {
        foreach (Vector3 tile in tilesOccuipedByBuilding)
        {
            Debug.Log(tile);
        }
    }
}
