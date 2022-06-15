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

    public List<GameObject> occupiedVisualizers;
    public List<GameObject> availableVisualizers;
    List<GameObject> buildings;
    GameObject visualizersParent;

    [SerializeField] List<GameObject> buildableBuildings;
    public float BuildingZ = 10;
    [Range(0,1)][SerializeField] float buildingGhostOpacity = 0.5f;

    public Vector3 currentMousePositionInWorld;
    
    List<Vector3> selectedBuildingOccupiedTiles;
    List<Vector3> ghostOccupiedTiles;
    public GameObject BuildingGhost;
    public GameObject SelectedBuilding;
    IBuildable selectedBuildingScript;

    Coroutine ghostCoroutine;

    [Range(0,2)]
    [SerializeField] int buildBuildingMouseButton = 1;
    [SerializeField] KeyCode buildingDeselect;
    [SerializeField] KeyCode building1Hotkey;
    [SerializeField] KeyCode building2Hotkey;
    [SerializeField] KeyCode rotationHotkey = KeyCode.R;

    Grid grid;
    Tilemap tilemap;
    PlayerStats playerStats;
    BuildCost buildCost;

    void Start()
    {        
        grid = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Grid>();
        tilemap = grid.GetComponentInChildren<Tilemap>();
        selectedBuildingOccupiedTiles = new List<Vector3>();
        ghostOccupiedTiles = new List<Vector3>();        
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        visualizersParent = new GameObject("Visualizers");
        occupiedVisualizers = InstantiateVisualizers(occupiedVisualizer, 1);
        availableVisualizers = InstantiateVisualizers(availableVisualizer, 1);
        buildings = new List<GameObject>();

        GameEvents.current.BuildingSelectedForBuilding += SelectBuilding;
    }

    void SelectBuilding (GameObject building) 
    {
        SelectedBuilding = building.gameObject;
        InstantiateGhost(SelectedBuilding, ref BuildingGhost, ghostOccupiedTiles);
    }


    void Update()
    {
        if (SelectBuildingHotkey()) InstantiateGhost(SelectedBuilding, ref BuildingGhost, ghostOccupiedTiles);

        if (SelectedBuilding != null) 
        {
            if (Input.GetKeyDown(rotationHotkey))
            {
                RotateBuilding();
            }

            if (Input.GetMouseButtonDown(buildBuildingMouseButton))
            {
                Build();
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
    private bool SelectBuildingHotkey() 
    {
        if (Input.GetKeyDown(buildingDeselect))
        {
            SelectedBuilding = null;
            DestroyGhost(BuildingGhost);
            return false;
        }

        else if (Input.GetKeyDown(building1Hotkey))
        {
            SelectedBuilding = buildableBuildings[0];
            return true;
        }

        else if (Input.GetKeyDown(building2Hotkey))
        {
            SelectedBuilding = buildableBuildings[1];
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

    private Vector3 CalculateBuildingLocation(List<Vector3> occupiedTiles) 
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

        return new Vector3(xPosition, yPosition, 0);
    }

    private void DestroyGhost(GameObject ghost) 
    {
        if (ghost != null)
        {
            Destroy(ghost);
            StopCoroutine(ghostCoroutine);
        }
    }

    public void InstantiateGhost(GameObject selectedBuilding, ref GameObject ghost, List<Vector3> ghostOccupiedTiles) 
    {
        DestroyGhost(ghost);

        Vector3 position = GetTileLocationInWorld();
        position.z = BuildingZ;
        ghost = Instantiate(selectedBuilding, position, Quaternion.identity);

        // Turn off collider
        PolygonCollider2D collider = ghost.GetComponent<PolygonCollider2D>();
        collider.enabled = !collider.enabled;

        // Turn opacity down
        SpriteRenderer spriteComponent = ghost.GetComponentInChildren<SpriteRenderer>();
        spriteComponent.color = new Color(spriteComponent.color.r, spriteComponent.color.g, spriteComponent.color.b, buildingGhostOpacity);

        ghostCoroutine = StartCoroutine(UpdateGhostPosition(ghost, ghostOccupiedTiles));
    }

    public IEnumerator UpdateGhostPosition(GameObject selectedBuilding, List<Vector3> ghostOccupiedTiles) 
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

                    ghostOccupiedTiles.Add(new Vector3(widthX, widthY, BuildingZ));
                }
            }

            // Move the ghost when mouse moves
            Vector3 position = CalculateBuildingLocation(ghostOccupiedTiles);
            position.z = BuildingZ;
            BuildingGhost.transform.position = position;

            // Update allowed to build vizualization
            // Change to Vector3Int and add to list
            Vector3 mousePosition = GetTileLocationInWorld();
            mousePosition.z = 10;

            if (currentMousePositionInWorld != mousePosition) {
                DeactivateVisualizers(availableVisualizers, occupiedVisualizers);

                currentMousePositionInWorld = mousePosition;
                MoveVisualizers(ghostOccupiedTiles, availableVisualizers, occupiedVisualizers);
            }

            ghostOccupiedTiles.Clear();
            yield return null;
        }
    }

    private void RotateBuilding() 
    {
        selectedBuildingScript = SelectedBuilding.GetComponent<IBuildable>();

        GameObject nextRotation = selectedBuildingScript.NextRotation;
        if (nextRotation != null)
        {
            // TODO: Same code as in instantiating ghost, except that we are instantiating nextRotation. Make it into a method.            
            Destroy(BuildingGhost);

            Vector3 position = GetTileLocationInWorld();
            position.z = BuildingZ;
            BuildingGhost = Instantiate(nextRotation, position, Quaternion.identity);

            // Turn opacity down
            SpriteRenderer spriteComponent = BuildingGhost.GetComponentInChildren<SpriteRenderer>();
            spriteComponent.color = new Color(spriteComponent.color.r, spriteComponent.color.g, spriteComponent.color.b, buildingGhostOpacity);

            // Not a part of the repeated code
            SelectedBuilding = nextRotation;
        }
        else
        {
            Debug.Log("Next rotation was null (Next rotation has not been set on the prefab)");
        }
    }

    private void Build() 
    {
        // Empty the list of tiles
        selectedBuildingOccupiedTiles.Clear();

        // Used to check if building placement should be aborted
        bool buildingPlacementAllowed = true;

        // Get selected buildings width and height
        selectedBuildingScript = SelectedBuilding.GetComponent<IBuildable>();
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

                selectedBuildingOccupiedTiles.Add(new Vector3(widthX, widthY, BuildingZ));
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
            buildCost = SelectedBuilding.GetComponent<BuildCost>();

            // Instantiate building on tileLocation
            GameObject buildingInstance = Instantiate(SelectedBuilding, CalculateBuildingLocation(selectedBuildingOccupiedTiles), Quaternion.identity);
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

            buildings.Add(SelectedBuilding);

            // Remove gold from player
            playerStats.RemoveGold(buildCost.Cost);

            GameStats.BuildingsBuilt++;  // increase GameStats record of finished buildings
            GameEvents.current.OnMapChanged(buildingInstance.transform.position, selectedBuildingOccupiedTiles.Count); 
        }

        DestroyGhost(BuildingGhost);
        DeactivateVisualizers(availableVisualizers, occupiedVisualizers);
        SelectedBuilding = null;
        buildCost = null;
    }

    private void DestroyBuilding(GameObject building, List<GameObject> buildings) {
        Destroy(building);
        buildings.Remove(building);
    }

    private List<GameObject> InstantiateVisualizers(GameObject visualizer, int amount) {
        List<GameObject> visualizersList = new List<GameObject>();

        for (int i = 0; i < amount; i++) {
            AddVisualizer(visualizersList, visualizer);
        }
        return visualizersList;
    }

    public void MoveVisualizers(List<Vector3> ghostOccupiedTiles, List<GameObject> availableVisualizers, List<GameObject> occupiedVisualizers) {

        int availableIndex = 0;
        int occupiedIndex = 0;
        
        if (ghostOccupiedTiles.Count > availableVisualizers.Count) {
            int amount = ghostOccupiedTiles.Count - availableVisualizers.Count;
            AddToVisualizers(availableVisualizers, availableVisualizer, amount);
        }

        if (ghostOccupiedTiles.Count > occupiedVisualizers.Count) {
            int amount = ghostOccupiedTiles.Count - occupiedVisualizers.Count;
            AddToVisualizers(occupiedVisualizers, occupiedVisualizer, amount);
        }          

        for (int i = 0; i < ghostOccupiedTiles.Count; i++) {

            Vector3 occupiedTile = ghostOccupiedTiles[i];

            Vector3Int cellPosition = tilemap.WorldToCell(occupiedTile);
            cellPosition.x += 5;
            cellPosition.y += 5;
            cellPosition.z = 0;

            GameObject tile = tilemap.GetInstantiatedObject(cellPosition);

            if (tile != null) {
                GroundTileData tileScript = tile.GetComponent<GroundTileData>();

                if (tileScript.isOccupied || !tileScript.isWalkable) {
                    GameObject visualizer = occupiedVisualizers[occupiedIndex];
                    visualizer.transform.position = ghostOccupiedTiles[i];
                    visualizer.SetActive(true);
                    occupiedIndex += 1;

                }
                else {
                    GameObject visualizer = availableVisualizers[availableIndex];
                    visualizer.transform.position = ghostOccupiedTiles[i];
                    visualizer.SetActive(true);
                    availableIndex += 1;
                }
            }
        }
    }

    public void DeactivateVisualizers(List<GameObject> occupiedVisualizers, List<GameObject> availableVisualizers) {
        for (int i = 0; i < occupiedVisualizers.Count; i++) {

            GameObject visualizer = occupiedVisualizers[i];

            if (visualizer.activeSelf == true) {
                visualizer.SetActive(false);
            }
        }

        for (int i = 0; i < availableVisualizers.Count; i++) {

            GameObject visualizer = availableVisualizers[i];

            if (visualizer.activeSelf == true) {
                visualizer.SetActive(false);
            }
        }
    }

    private void AddVisualizer(List<GameObject> visualizersList, GameObject visualizer) {
        GameObject go = Instantiate(visualizer, Vector3.zero, Quaternion.identity);
        go.SetActive(false);
        go.transform.SetParent(visualizersParent.transform);
        visualizersList.Add(go);    
    }

    private void AddToVisualizers(List<GameObject> visualizersList, GameObject visualizer, int amount) {
        for (int i = 0; i < amount; i++) {
            AddVisualizer(visualizersList, visualizer);
        }  
    }
}
