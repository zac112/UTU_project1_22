using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BuildingPlacementSystem : MonoBehaviour
{
    [SerializeField] GameObject buildingToPlace;
    [SerializeField] GameObject cubePrefab;
    [SerializeField] List<GameObject> buildableBuildings;
    [SerializeField] float buildingZ = 10;
    [Range(0,1)][SerializeField] float buildingGhostOpacity = 0.5f;
    bool buildingGhostInstantiated;
    List<Vector3> selectedBuildingOccupiedTiles;
    List<Vector3> buildingGhostOccupiedTiles;
    GameObject buildingGhost;

    [Range(0,2)]
    [SerializeField] int buildBuildingMouseButton = 1;
    [SerializeField] KeyCode buildingDeselect;
    [SerializeField] KeyCode building1Hotkey;
    [SerializeField] KeyCode building2Hotkey;
    [SerializeField] KeyCode rotationHotkey = KeyCode.R;

    Grid grid;
    Tilemap tilemap;
    OccupiedTiles occupiedTiles;
    GameObject selectedBuilding;

    void Start()
    {        
        grid = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Grid>();
        tilemap = grid.GetComponentInChildren<Tilemap>();
        occupiedTiles = GameObject.FindObjectOfType<OccupiedTiles>();
        selectedBuildingOccupiedTiles = new List<Vector3>();
        buildingGhostOccupiedTiles = new List<Vector3>();
        buildingGhostInstantiated = false;
    }

    void Update()
    {
        selectBuildingHotkey();

        if (selectedBuilding != null) 
        {
            instantiateGhost();

            if (buildingGhostInstantiated)
            {
                updateGhostPosition();
            }

            if (Input.GetKeyDown(rotationHotkey))
            {
                rotateBuilding();
            }

            if (Input.GetMouseButtonDown(buildBuildingMouseButton))
            {
                buildBuilding();
            }
        }
        
    }

    public Vector3 GetTileLocationInWorld()
    {
        Vector3Int tileLocation = GetTileCellLocation();
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

    // TODO: Move to input manager. Struct with KeyCode and prefab.
    private bool selectBuildingHotkey() 
    {
        if (Input.GetKeyDown(buildingDeselect))
        {
            selectedBuilding = null;
            destroyGhost();
            return false;
        }

        else if (Input.GetKeyDown(building1Hotkey))
        {
            selectedBuilding = buildableBuildings[0];
            return true;
        }

        else if (Input.GetKeyDown(building2Hotkey))
        {
            selectedBuilding = buildableBuildings[1];
            return true;
        }
        return false;       
    }

    // TODO: Make private and expose it from an interface?
    public void selectBuildingGUI(GameObject building) 
    {
        selectedBuilding = building.gameObject;
    }

    public Vector3Int GetTileCellLocation() 
    {
        // Get the location of the tile under the mouse
        Vector3Int tileLocation = grid.WorldToCell(GetMousePosition());
        //tileLocation.x -= 1;
        //tileLocation.y -= 1;

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

    private void destroyGhost() 
    {
        if (buildingGhost != null)
        {
            Destroy(buildingGhost);
            buildingGhostInstantiated = false;
        }
    }

    private void instantiateGhost() 
    {
        destroyGhost();

        // Building ghost
        // Instantiate building
        // Turn it's opacity down
        // Make it follow mouse
        // Destroy ghost when building is placed

        Vector3 position = GetTileLocationInWorld();
        position.z = buildingZ;
        buildingGhost = Instantiate(selectedBuilding, position, Quaternion.identity);

        // Turn opacity down
        SpriteRenderer spriteComponent = buildingGhost.GetComponentInChildren<SpriteRenderer>();
        spriteComponent.color = new Color(spriteComponent.color.r, spriteComponent.color.g, spriteComponent.color.b, buildingGhostOpacity);

        buildingGhostInstantiated = true;
    }

    private void updateGhostPosition() 
    {      
        {
            // Repeating code that exists in the if statement below
            // Find a way to avoid repeating it.
            IBuildable selectedBuildingScript = selectedBuilding.GetComponent<IBuildable>();
            int buildingWidth = selectedBuildingScript.Width;
            int buildingLength = selectedBuildingScript.Length;

            Vector3 selectedTileLocationInWorld = GetTileLocationInWorld();

            // If building is rotated, switch length and width with eachother
            if (selectedBuildingScript.IsRotated)
            {
                int temp = buildingWidth;
                buildingWidth = buildingLength;
                buildingLength = temp;
            }

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

                    buildingGhostOccupiedTiles.Add(new Vector3(widthX, widthY, buildingZ));
                    //Debug.Log($"Vector3 added: {result}");
                }
            }

            // Move the ghost when mouse moves
            Vector3 position = calculateBuildingLocation(buildingGhostOccupiedTiles);
            position.z = buildingZ;
            buildingGhost.transform.position = position;

            buildingGhostOccupiedTiles.Clear();
        }
    }

    private void rotateBuilding() 
    {
        IBuildable selectedBuildingScript = selectedBuilding.GetComponent<IBuildable>();

        GameObject nextRotation = selectedBuildingScript.NextRotation;
        if (nextRotation != null)
        {
            // TODO: Same code as in instantiating ghost, except that we are instantiating nextRotation. Make it into a method.
            destroyGhost();

            Vector3 position = GetTileLocationInWorld();
            position.z = buildingZ;
            buildingGhost = Instantiate(nextRotation, position, Quaternion.identity);

            // Turn opacity down
            SpriteRenderer spriteComponent = buildingGhost.GetComponentInChildren<SpriteRenderer>();
            spriteComponent.color = new Color(spriteComponent.color.r, spriteComponent.color.g, spriteComponent.color.b, buildingGhostOpacity);

            buildingGhostInstantiated = true;

            // Not a part of the repeated code
            selectedBuilding = nextRotation;
        }
        else
        {
            Debug.Log("Next rotation was null (Next rotation has not been set on the prefab)");
        }
    }

    private void buildBuilding() 
    {
        // Empty the list of tiles
        selectedBuildingOccupiedTiles.Clear();

        // Used to check if building placement should be aborted
        bool buildingPlacementAllowed = true;

        // Get selected buildings width and height
        IBuildable selectedBuildingScript = selectedBuilding.GetComponent<IBuildable>();
        int buildingWidth = selectedBuildingScript.Width;
        int buildingLength = selectedBuildingScript.Length;

        // If building is rotated, switch length and width with eachother
        if (selectedBuildingScript.IsRotated)
        {
            int temp = buildingWidth;
            buildingWidth = buildingLength;
            buildingLength = temp;
        }

        Vector3 selectedTileLocationInWorld = GetTileLocationInWorld();

        // Calculate tile coordinates that the building will occupy based on selected buildings width and selected building script length
        // Currently, moving NW will modify X by -0.50 and Y by +0.25
        // Moving NE will modify X by +0.50 and Y by +0.25

        // Loop through width and height and add these tiles to tilesOccupiedByBuilding
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

                selectedBuildingOccupiedTiles.Add(new Vector3(widthX, widthY, buildingZ));
                //Debug.Log($"Vector3 added: {result}");
            }
        }

        // Check for each tile, if tile is in occupied tiles list
        for (int i = 0; i < selectedBuildingOccupiedTiles.Count; i++)
        {
            if (occupiedTiles.IsTileOccupied(selectedBuildingOccupiedTiles[i]))
            {
                buildingPlacementAllowed = false;
            }
        }

        if (buildingPlacementAllowed)
        {
            // Add tiles to occupiedTiles
            occupiedTiles.OccupyTiles(selectedBuildingOccupiedTiles);

            // Instantiate building on tileLocation
            GameObject buildingInstance = Instantiate(selectedBuilding, calculateBuildingLocation(selectedBuildingOccupiedTiles), Quaternion.identity);
            buildingInstance.layer = LayerMask.NameToLayer("Buildings");

            IBuildable buildingInstanceScript = buildingInstance.GetComponent<IBuildable>();

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
        }
        else
        {
            Debug.Log("A tile was occupied. Aborting building placement.");
        }
    }

    public void PrintHello() 
    {
        Debug.Log("Hello");
    }
}
