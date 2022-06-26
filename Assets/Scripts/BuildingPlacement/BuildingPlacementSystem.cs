using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;


public class BuildingPlacementSystem : NetworkBehaviour
{   
    [SerializeField] List<GameObject> buildableBuildings;
    public float BuildingZ = 10;

    List<GameObject> buildings;  // instantiated GameObjects (not abstract prefabs)
    public Vector3 currentMousePositionInWorld;    
    List<Vector3> selectedBuildingOccupiedTiles;
    List<Vector3> goldMineRangeTiles;
    
    public GameObject SelectedBuilding;
    public IBuildable selectedBuildingScript;

    [Range(0,2)]
    [SerializeField] int buildBuildingMouseButton = 1;
    [SerializeField] KeyCode buildingDeselect;
    [SerializeField] KeyCode building1Hotkey;
    [SerializeField] KeyCode building2Hotkey;
    [SerializeField] KeyCode rotationHotkey = KeyCode.R;

    [SerializeField] BuildingGhost buildingGhost;    
    [SerializeField] BuildingVisualizer buildingVisualizer;
    [SerializeField] DragWallBuilding buildingDrag;
    Tilemap tilemap;
    PlayerStats playerStats;
    GameObject buildingInstance;

    [Tooltip("Width and height of gold mine mining range")] [SerializeField] int goldMineMiningRange; // set to 5, generating a 5 x 5 area
    bool goldNodeWithinRange;

    public void Start()
    {        
        selectedBuildingOccupiedTiles = new List<Vector3>();        
        buildings = new List<GameObject>();
        goldMineRangeTiles = new List<Vector3>();

        tilemap = GameObject.FindObjectOfType<Tilemap>();

        GameEvents.current.BuildingSelectedForBuilding += SelectBuilding;
        GameEvents.current.BuildingSelectedCancelled += DeselectBuilding;
    }
    
    void SelectBuilding (GameObject building) 
    {
        SelectedBuilding = building.gameObject;
        if (SelectedBuilding.name.Contains("Wall")) 
            buildingDrag.gameObject.SetActive(true);
        else 
            buildingGhost.InstantiateGhost(SelectedBuilding, ref buildingGhost.Ghost, buildingGhost.GhostOccupiedTiles);
    }

    void Update()
    {
        if (playerStats == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go  != null){
                playerStats = go.GetComponent<PlayerStats>();
                return; 
            }
            
        }

        if (SelectBuildingHotkey()) buildingGhost.InstantiateGhost(SelectedBuilding, ref buildingGhost.Ghost, buildingGhost.GhostOccupiedTiles);

        if (SelectedBuilding != null) 
        {
            if (Input.GetKeyDown(rotationHotkey))
            {
                buildingGhost.RotateGhost();
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
        //tileLocationInWorld.x -= 0.50f;
        //tileLocationInWorld.y -= 0.25f;

        return tileLocationInWorld;
    }

    // TODO: Move to input manager. Struct with KeyCode and prefab.
    private bool SelectBuildingHotkey() 
    {
        if (Input.GetKeyDown(buildingDeselect))
        {
            DeselectBuilding();
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
        return tilemap.WorldToCell(GetMousePosition());
    }

    public Vector3 GetMousePosition() 
    {
        // Get the mouse position and store it in a variable
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public Vector3 CalculateBuildingLocation(List<Vector3> occupiedTiles) 
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


    private void Build() 
    {
        // Empty the list of tiles
        selectedBuildingOccupiedTiles.Clear();
        AddToOccupiedTiles();

        bool buildingPlacementAllowed = true;
        buildingPlacementAllowed &= CheckIfTilesOccupied();
        buildingPlacementAllowed &= CheckGoldMine();

        BuildBuilding(buildingPlacementAllowed, SelectedBuilding.GetComponent<BuildCost>());

        buildingGhost.DestroyGhost(buildingGhost.Ghost);
        buildingVisualizer.DeactivateVisualizers();
        SelectedBuilding = null;
        goldMineRangeTiles.Clear();
    }

    private void AddToOccupiedTiles()
    {
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
    }

    private bool CheckIfTilesOccupied()
    {
        bool buildingPlacementAllowed = true;

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
            else
            {
                buildingPlacementAllowed = false;
            }
        }
        return buildingPlacementAllowed;
    }
    private bool CheckGoldMine()
    {
        bool buildingPlacementAllowed = true;
        Vector3 selectedTileLocationInWorld = GetTileLocationInWorld();

        // for gold mines, check whether gold nodes exist within range
        if (SelectedBuilding.CompareTag("GoldMine"))
        {
            goldNodeWithinRange = CheckGoldNodes(selectedTileLocationInWorld);
            if (!goldNodeWithinRange) buildingPlacementAllowed = false;
        }
        return buildingPlacementAllowed;
    }
    public void BuildBuilding(bool buildingPlacementAllowed, BuildCost buildCost) 
    {
        if (buildingPlacementAllowed && playerStats.GetGold() >= buildCost.Cost && playerStats.GetWood() >= buildCost.Wood)
        {
            // Instantiate building on tileLocation
            BuildCompleteServerRpc(SelectedBuilding.name, CalculateBuildingLocation(selectedBuildingOccupiedTiles));                        
            playerStats.RemoveGold(buildCost.Cost);          
            playerStats.RemoveWood(buildCost.Wood);          
            GameStats.BuildingsBuilt++;  
            buildings.Add(SelectedBuilding);

        }
        DeselectBuilding();
    }
    private void DestroyBuilding(GameObject building, List<GameObject> buildings) {
        Destroy(building);
        buildings.Remove(building);
    }

    public void DeselectBuilding() {
        buildingGhost.DestroyGhost(buildingGhost.Ghost);
        buildingVisualizer.DeactivateVisualizers();
        SelectedBuilding = null;
        goldMineRangeTiles.Clear();
    }

    
    [ServerRpc(RequireOwnership = false)]
    public void BuildCompleteServerRpc(string prefabName, Vector3 pos, ServerRpcParams rpcParams = default)
    {
        GameObject[] buildings_prefabs = Resources.LoadAll<GameObject>("Buildings");
        foreach (GameObject go in buildings_prefabs) {
            if (go.name.Equals(prefabName)){
                buildingInstance = Instantiate(go, pos, Quaternion.identity);
                buildingInstance.GetComponent<NetworkObject>().Spawn();

                // start generating gold when the gold mine is instantiated
                if (buildingInstance.CompareTag("GoldMine"))
                {
                    GoldMineScript minescript = buildingInstance.GetComponent<GoldMineScript>();
                    minescript.enabled = true;

                    AddToGoldNodesWithinRange(goldMineRangeTiles, buildingInstance);
                }

                buildings.Add(buildingInstance);

                break;
            }
        }
    }

    /// <summary>
    /// For gold mines, check whether gold nodes exist within range 
    /// </summary>
    bool CheckGoldNodes(Vector3 selectedTileLocationInWorld)
    {
        goldNodeWithinRange = false;

        float goldRangeWidthX;
        float goldRangeWidthY;

        for (int width = 0; width < goldMineMiningRange; width++)
        {
            goldRangeWidthX = selectedTileLocationInWorld.x - 0.50f * width;
            goldRangeWidthY = selectedTileLocationInWorld.y + 0.25f * width;

            for (int length = 0; length < goldMineMiningRange; length++)
            {
                goldRangeWidthX += 0.50f;
                goldRangeWidthY += 0.25f;

                goldMineRangeTiles.Add(new Vector3(goldRangeWidthX, goldRangeWidthY, BuildingZ));
            }
        }

        // check for all tiles within mining range whether there is a gold node
        foreach (Vector3 tileposition in goldMineRangeTiles)
        {
            Vector3Int cellPosition = GetGoldNodeCellPosition(tileposition);

            GameObject tile = tilemap.GetInstantiatedObject(cellPosition);

            if (tile != null)
            {
                GroundTileData tileScript = tile.GetComponent<GroundTileData>();

                if (tileScript.isGoldNode) goldNodeWithinRange = true;
            }
        }
        return goldNodeWithinRange;
    }

    public int GetGoldMineMiningRange() { return goldMineMiningRange; }

    /// <summary>
    /// Gold nodes keep record of which gold mines use them. Needed for calculating gold mining efficiency.
    /// Also updates the mining efficiencies of gold mines that are linked to nearby gold nodes.
    /// </summary>
    void AddToGoldNodesWithinRange (List<Vector3> tileList, GameObject building)
    {
        List<GameObject> goldNodesToUpdate = new();

        foreach (Vector3 tileposition in tileList)
        {
            Vector3Int cellPosition = GetGoldNodeCellPosition(tileposition);
            GameObject tile = tilemap.GetInstantiatedObject(cellPosition);

            if (tile != null)
            {
                GroundTileData tileScript = tile.GetComponent<GroundTileData>();
                if (tileScript.isGoldNode)
                {                 
                    GoldNodeScript goldNodeScript = tile.GetComponent<GoldNodeScript>();
                    goldNodeScript.AddToAttachedGoldMines(building);
                    GoldMineScript minescript = building.GetComponent<GoldMineScript>();
                    minescript.AddToAttachedGoldNodes(tile);  // also add to gold mine record of attached gold nodes
                    goldNodesToUpdate.Add(tile);  // update mining efficiencies for gold mines attached to nearby gold nodes
                }
            }
        }

        foreach (GameObject node in goldNodesToUpdate)
        {
            GoldNodeScript gns = node.GetComponent<GoldNodeScript>();
            gns.UpdateAttachedGoldMinesEfficiency();
        }
    }

    Vector3Int GetGoldNodeCellPosition(Vector3 pos)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(pos);
        cellPosition.x += 5;
        cellPosition.y += 5;
        cellPosition.z = 0;

        // had to correct x and y offsets here for some reason, left the default x and y assignments above as reference points
        cellPosition.x -= 2;
        cellPosition.y -= 1;

        return cellPosition;
    }

}
