using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingPlacementSystem : MonoBehaviour
{
    [SerializeField] Transform buildingToPlace;
    [SerializeField] Transform cubePrefab;
    [SerializeField] List<Transform> buildableBuildings;
    [SerializeField] float buildingZ = 10;
    List<Vector3> selectedBuildingOccupiedTiles;


    [Range(0,2)]
    [SerializeField] int buildBuildingMouseButton = 1;
    [SerializeField] KeyCode buildingDeselect;
    [SerializeField] KeyCode building1Hotkey;
    [SerializeField] KeyCode building2Hotkey;

    Grid grid;
    Tilemap tilemap;
    OccupiedTiles occupiedTiles;
    Transform selectedBuilding;

    void Start()
    {
        // TODO: Find a more reliable way to find Tilemap component that does not break every time names are changed
        grid = GameObject.Find("Grid(Clone)").GetComponent<Grid>();
        tilemap = GameObject.Find("Grid(Clone)").GetComponentInChildren<Tilemap>();
        occupiedTiles = GameObject.Find("OccupiedTilesSystem").GetComponent<OccupiedTiles>();
        selectedBuildingOccupiedTiles = new List<Vector3>();

        //Instantiate(cubePrefab, new Vector3(0, 0, 10), Quaternion.identity);
    }

    void Update()
    {
        SelectBuilding();

        if (Input.GetMouseButtonDown(buildBuildingMouseButton) && selectedBuilding != null) 
        {
            // Get selected buildings width and height
            IBuildable selectedBuildingScript = selectedBuilding.GetComponent<IBuildable>();
            int buildingWidth = selectedBuildingScript.Width;
            int buildingLength = selectedBuildingScript.Length;

            Vector3 selectedTileLocationInWorld = GetTileLocationUnderMouse();
            Debug.Log($"tileLocationInWorld: {selectedTileLocationInWorld}");

            // Calculate tile coordinates that the building will occupy based on selected buildings width and selected building script length
            // Currently, moving NW will modify X by -0.50 and Y by +0.25
            // Moving NE will modify X by +0.50 and Y by +0.25

            // Loop through width and height and add these tiles to tilesOccupiedByBuilding
            // TODO: First check if tiles are already occupied

            float widthX;
            float widthY;

            for (int width = 0; width < buildingWidth; width++) 
            {
                
                widthX = selectedTileLocationInWorld.x - 0.50f * width;
                widthY = selectedTileLocationInWorld.y + 0.25f * width;

                for (int length = 0; length < buildingLength; length++) 
                {
                    widthX += 0.50f;
                    widthY += 0.25f;

                    Vector3 result = new Vector3(widthX, widthY, buildingZ);
                    selectedBuildingOccupiedTiles.Add(result);
                    Debug.Log($"Vector3 added: {result}");
                }
            }

            // Add tiles to occupiedTiles
            occupiedTiles.OccupyTiles(selectedBuildingOccupiedTiles);

            // Instantiate building on tileLocation
            // TODO: Fix instantiating building on the center point of the buildings occupied tiles
            Transform buildingInstance = Instantiate(selectedBuilding, calculateBuildingLocation(selectedBuildingOccupiedTiles), Quaternion.identity);
            
            // Debug.Log(buildingInstance);

            IBuildable buildingInstanceScript = buildingInstance.GetComponent<IBuildable>();
            // Debug.Log(buildingInstanceScript.OccupiedTiles);

            // Add occupiedTiles to the building instance
            for (int i = 0; i < selectedBuildingOccupiedTiles.Count; i++) 
            {
                buildingInstanceScript.AddToOccupiedTiles(selectedBuildingOccupiedTiles[i]);
                //Debug.Log(selectedBuildingOccupiedTiles[i]);
            }

            //Debug.Log($"Average: {calculateBuildingLocation(selectedBuildingOccupiedTiles)}");

            // Debug.Log(buildingInstanceScript.OccupiedTiles.Count);

            GameStats.BuildingsBuilt++;  // increase GameStats record of finished buildings

            
            // Testing which tiles are occupied by instancing a circle sprite on them
            for (int i = 0; i < selectedBuildingOccupiedTiles.Count; i++) 
            {
                Instantiate(cubePrefab, selectedBuildingOccupiedTiles[i], Quaternion.identity);
            }

            selectedBuildingOccupiedTiles.Clear();

        }
    }

    public Vector3 GetTileLocationUnderMouse()
    {
        Vector3Int tileLocation = GetTileLocation();
        //Debug.Log($"Tilelocation: {tileLocation}");

        // Get the center of the tile under the mouse?
        // Perhaps a bit redundant to first get the tile location in the tilemap and then turn it back to world position?
        Vector3 tileLocationInWorld = tilemap.GetCellCenterWorld(tileLocation);

        /* TODO: Find a cleaner/more correct way to get correct tile.
         * Currently, clicking a tile returns the next tile towards NE, but subtracting
         * 0.50 from x and 0.25 from y fixes it.
         */
        tileLocationInWorld.x -= 0.50f;
        tileLocationInWorld.y -= 0.25f;

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
        Vector3Int tileLocation = grid.WorldToCell(GetMousePosition());
        return tileLocation;
    }

    public Vector3 GetMousePosition() 
    {
        // Get the mouse position and store it in a variable
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePosition;
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
