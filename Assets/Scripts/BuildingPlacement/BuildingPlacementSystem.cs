using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class BuildingPlacementSystem : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab;
    [SerializeField] GameObject occupiedVisualizer;
    [SerializeField] GameObject availableVisualizer;

    [SerializeField] List<GameObject> buildableBuildings;
    public float buildingZ = 10;
    [Range(0,1)][SerializeField] float buildingGhostOpacity = 0.5f;

    Vector3 currentMousePositionInWorld;
    
    List<Vector3> selectedBuildingOccupiedTiles;
    List<Vector3> ghostOccupiedTiles;
    public GameObject buildingGhost;
    public GameObject selectedBuilding;
    IBuildable selectedBuildingScript;
    List<GameObject> buildingOccupiedOverlay;
    List<GameObject> occupiedVisualizerList;

    Coroutine ghostCoroutine;

    [Range(0,2)]
    [SerializeField] int buildBuildingMouseButton = 1;
    [SerializeField] KeyCode buildingDeselect;
    [SerializeField] KeyCode building1Hotkey;
    [SerializeField] KeyCode building2Hotkey;
    [SerializeField] KeyCode rotationHotkey = KeyCode.R;

    Grid grid;
    public Tilemap tilemap;
    OccupiedTiles occupiedTiles;
    PlayerStats playerStats;

    void Start()
    {        
        grid = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Grid>();
        tilemap = grid.GetComponentInChildren<Tilemap>();
        occupiedTiles = GameObject.FindObjectOfType<OccupiedTiles>();
        selectedBuildingOccupiedTiles = new List<Vector3>();
        ghostOccupiedTiles = new List<Vector3>();        
        buildingOccupiedOverlay = new List<GameObject>();
        occupiedVisualizerList = new List<GameObject>();
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

        GameEvents.current.BuildingSelectedForBuilding += selectBuilding;
    }

    void selectBuilding(GameObject building) 
    {
        selectedBuilding = building.gameObject;
        instantiateGhost(selectedBuilding, ref buildingGhost, ghostOccupiedTiles);
    }


    void Update()
    {
        selectBuildingHotkey();

        if (selectedBuilding != null) 
        {
            instantiateGhost(selectedBuilding, ref buildingGhost, ghostOccupiedTiles);

            if (Input.GetKeyDown(rotationHotkey))
            {
                rotateBuilding();
            }

            if (Input.GetMouseButtonDown(buildBuildingMouseButton))
            {
                build();
            }
        }
        
    }

    public Vector3 GetTileLocationInWorld()
    {
        Vector3Int tileLocation = GetTileCellLocation();

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
            destroyGhost(buildingGhost);
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

    public Vector3Int GetTileCellLocation() 
    {
        // Get the location of the tile under the mouse
        return grid.WorldToCell(GetMousePosition());
    }

    public Vector3 GetMousePosition() 
    {
        // Get the mouse position and store it in a variable
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

    private void destroyGhost(GameObject ghost) 
    {
        if (ghost != null)
        {
            Destroy(ghost);
            StopCoroutine(ghostCoroutine);
        }
    }

    public void instantiateGhost(GameObject selectedBuilding, ref GameObject ghost, List<Vector3> ghostOccupiedTiles) 
    {
        destroyGhost(ghost);

        Vector3 position = GetTileLocationInWorld();
        position.z = buildingZ;
        ghost = Instantiate(selectedBuilding, position, Quaternion.identity);

        // Turn off collider
        PolygonCollider2D collider = ghost.GetComponent<PolygonCollider2D>();
        collider.enabled = !collider.enabled;

        // Turn opacity down
        SpriteRenderer spriteComponent = ghost.GetComponentInChildren<SpriteRenderer>();
        spriteComponent.color = new Color(spriteComponent.color.r, spriteComponent.color.g, spriteComponent.color.b, buildingGhostOpacity);

        ghostCoroutine = StartCoroutine(updateGhostPosition(ghost, ghostOccupiedTiles));
    }

    public IEnumerator updateGhostPosition(GameObject selectedBuilding, List<Vector3> ghostOccupiedTiles) 
    {      
        while (true){
            // Repeating code that exists in the if statement below
            // Make into a method
            selectedBuildingScript = selectedBuilding.GetComponent<IBuildable>();
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

                    ghostOccupiedTiles.Add(new Vector3(widthX, widthY, buildingZ));
                }
            }

            // Move the ghost when mouse moves
            Vector3 position = calculateBuildingLocation(ghostOccupiedTiles);
            position.z = buildingZ;
            buildingGhost.transform.position = position;

            // Update allowed to build vizualization
            // Change to Vector3Int and add to list
            Vector3 mousePosition = GetTileLocationInWorld();
            mousePosition.z = 10;
            
            if (currentMousePositionInWorld != mousePosition) 
            {
                EmptyOccupiedVisualizerList();

                currentMousePositionInWorld = mousePosition;
                for (int i = 0; i < ghostOccupiedTiles.Count; i++)
                {
                    Vector3 tileWorldCoordinates = ghostOccupiedTiles[i];

                    Vector3Int cellPosition = tilemap.WorldToCell(tileWorldCoordinates);
                    cellPosition.x += 5;
                    cellPosition.y += 5;
                    cellPosition.z = 0;

                    // Get instantiated tile GameObject
                    GameObject tile = tilemap.GetInstantiatedObject(cellPosition);

                    if (tile != null) {
                        // Get the script attached to the GameObject
                        GroundTileData tileScript = tile.GetComponent<GroundTileData>();

                        if (tileScript.isOccupied || !tileScript.isWalkable)
                        {
                            GameObject visualizer = Instantiate(occupiedVisualizer, tileWorldCoordinates, Quaternion.identity);
                            occupiedVisualizerList.Add(visualizer);
                        }
                        else
                        {
                            GameObject visualizer = Instantiate(availableVisualizer, tileWorldCoordinates, Quaternion.identity);
                            occupiedVisualizerList.Add(visualizer);
                        }
                    }
                }
            }

            ghostOccupiedTiles.Clear();
            yield return null;
        }
    }

    private void rotateBuilding() 
    {
        selectedBuildingScript = selectedBuilding.GetComponent<IBuildable>();

        GameObject nextRotation = selectedBuildingScript.NextRotation;
        if (nextRotation != null)
        {
            // TODO: Same code as in instantiating ghost, except that we are instantiating nextRotation. Make it into a method.            
            Destroy(buildingGhost);

            Vector3 position = GetTileLocationInWorld();
            position.z = buildingZ;
            buildingGhost = Instantiate(nextRotation, position, Quaternion.identity);

            // Turn opacity down
            SpriteRenderer spriteComponent = buildingGhost.GetComponentInChildren<SpriteRenderer>();
            spriteComponent.color = new Color(spriteComponent.color.r, spriteComponent.color.g, spriteComponent.color.b, buildingGhostOpacity);

            // Not a part of the repeated code
            selectedBuilding = nextRotation;
        }
        else
        {
            Debug.Log("Next rotation was null (Next rotation has not been set on the prefab)");
        }
    }

    private void build() 
    {
        // Empty the list of tiles
        selectedBuildingOccupiedTiles.Clear();

        // Used to check if building placement should be aborted
        bool buildingPlacementAllowed = true;

        // Get selected buildings width and height
        selectedBuildingScript = selectedBuilding.GetComponent<IBuildable>();
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
            }
        }

        // Check for each tile, if tile is occupied
        for (int i = 0; i < selectedBuildingOccupiedTiles.Count; i++)
        {
            // Turn from world coordinate to cell coordinate
            Vector3Int cellPosition = tilemap.WorldToCell(selectedBuildingOccupiedTiles[i]);
            cellPosition.x += 5;
            cellPosition.y += 5;
            cellPosition.z = 0;

            // Get instantiated tile GameObject
            GameObject tile = tilemap.GetInstantiatedObject(cellPosition);

            if (tile != null)
            {
                // Get the script attached to the GameObject
                GroundTileData tileScript = tile.GetComponent<GroundTileData>();

                if (tileScript.isOccupied == true || tileScript.isWalkable == false)
                {
                    buildingPlacementAllowed = false;
                }
            }
            else {
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

            selectedBuildingScript = buildingInstance.GetComponent<IBuildable>();

            // Add occupiedTiles to the building instance
            for (int i = 0; i < selectedBuildingOccupiedTiles.Count; i++)
            {
                selectedBuildingScript.AddToOccupiedTiles(selectedBuildingOccupiedTiles[i]);

                Vector3Int cellPosition = tilemap.WorldToCell(selectedBuildingOccupiedTiles[i]);
                cellPosition.x += 5;
                cellPosition.y += 5;
                cellPosition.z = 0;

                // Tile script
                GameObject tile = tilemap.GetInstantiatedObject(cellPosition);

                if (tile != null) 
                {
                    GroundTileData tileScript = tile.GetComponent<GroundTileData>();
                    tileScript.isOccupied = true;
                }
            }

            // Remove gold from player
            //playerStats.RemoveGold(selectedBuilding.BuildCost);

            GameStats.BuildingsBuilt++;  // increase GameStats record of finished buildings
            GameEvents.current.OnMapChanged(buildingInstance.transform.position, selectedBuildingOccupiedTiles.Count); 
        }

        destroyGhost(buildingGhost);
        selectedBuilding = null;
        EmptyOccupiedVisualizerList();
    }

    private void instantiateTestCircle(Vector3Int position) 
    {
        Vector3 worldPosition = tilemap.CellToWorld(position);
        GameObject prefab = Instantiate(cubePrefab, worldPosition, Quaternion.identity);

        SpriteRenderer spriteComponent = prefab.GetComponentInChildren<SpriteRenderer>();

        spriteComponent.color = new Color(1, 0, 0);
    }

    private void instantiateTestCircle(Vector3 position)
    {
        GameObject prefab = Instantiate(cubePrefab, position, Quaternion.identity);

        SpriteRenderer spriteComponent = prefab.GetComponentInChildren<SpriteRenderer>();

        spriteComponent.color = new Color(1, 0, 0);

    }

    private List<Vector3Int> ConvertWorldToCell(List<Vector3> list) 
    {
        List<Vector3Int> newList = new List<Vector3Int>();

        for (int i = 0; i < ghostOccupiedTiles.Count; i++)
        {
            // Turn from world coordinate to cell coordinate
            Vector3Int cellPosition = tilemap.WorldToCell(ghostOccupiedTiles[i]);
            cellPosition.x += 5;
            cellPosition.y += 5;
            cellPosition.z = 0;

            newList.Add(cellPosition);
        }

        return newList;
    }

    private void EmptyOccupiedVisualizerList() 
    {
        //Destroy items in occupiedVisualizerList
        for (int i = 0; i < occupiedVisualizerList.Count; i++)
        {
            Destroy(occupiedVisualizerList[i]);
        }

        occupiedVisualizerList.Clear();
    }
}
