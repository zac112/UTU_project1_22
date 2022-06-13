using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

 
public class TileChanger : MonoBehaviour
{
    [SerializeField] public Tile selectedTile;
    [SerializeField] public Tile FarmTile;
    [SerializeField] public Tile RoadTile;
    
    Vector3Int location;
    Tilemap tilemap;
    [SerializeField] GameObject plot;
    PlayerStats player;

    private int hoeToolPrice = 50;
    private int roadTilePrice = 100;

    [SerializeField] BuildingPlacementSystem bps;
    [Range(0, 1)][SerializeField] float buildingGhostOpacity = 0.5f;
    GameObject ghost;
    List<Vector3> ghostOccupiedTiles;
    Coroutine ghostCoroutine;
    Vector3 currentMousePositionInWorld;

    [SerializeField] GameObject occupiedVisualizer;
    [SerializeField] GameObject availableVisualizer;

    List<GameObject> occupiedVisualizerList;

    void Start()
    {
        updateReferences();
    }

    void Update()
    {
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        location = tilemap.WorldToCell(mp);
        location.z = 0;
        location.x -= 2;
        location.y -= 2;

        // Get instantiated tile GameObject
        // This gets called every update, maybe change to on mouseclick
        GameObject groundTile = tilemap.GetInstantiatedObject(location);
        
        if (groundTile != null) {
            // Get the script attached to the GameObject
            GroundTileData tileScript = groundTile.GetComponent<GroundTileData>();

            // Farmtiles can be placed anywhere else other than water and on itself
            if (Input.GetMouseButtonDown(1) && selectedTile.name.Equals("FarmTile") && tilemap.GetTile(location).name != ("WaterTile") && tilemap.GetTile(location).name != ("FarmTile") && player.GetGold() >= hoeToolPrice && !tileScript.isOccupied)
            {
                tilemap.SetTile(location, selectedTile);
                player.RemoveGold(hoeToolPrice);
                tileScript.isOccupied = true;
            }
            
            // If player is building road --> can also build on water to make bridges but not on placed FarmTiles
            // Remove gold from player
            else if (Input.GetMouseButtonDown(1) && selectedTile.name.Equals("RoadTile") && tilemap.GetTile(location).name != ("FarmTile") && player.GetGold() >= roadTilePrice && !tileScript.isOccupied)
            {
                tilemap.SetTile(location, selectedTile);
                player.RemoveGold(roadTilePrice);
                tileScript.isOccupied = true;
            }
        } 
    }

    // This will change the selected tile when switching the tool on the UI
    public void Reset(string TileType)
    {
        if(TileType == "FarmTile")
        {
            selectedTile = FarmTile;
        }

        if(TileType == "RoadTile")
        {
            selectedTile = RoadTile;
        }
    }


    private void OnEnable()
    {
        if (ghost == null) {
            updateReferences();
            instantiateGhost(plot, ref ghost);
        } 
    }

    private void OnDisable()
    {
        if (ghost != null) 
        {
            destroyGhost(ghost);
        }
    }

    private void destroyGhost(GameObject ghost)
    {
        if (ghost != null)
        {
            Destroy(ghost);
            StopCoroutine(ghostCoroutine);
        }
    }

    public void instantiateGhost(GameObject selectedBuilding, ref GameObject ghost)
    {
        destroyGhost(ghost);

        Vector3 position = bps.GetTileLocationInWorld();
        position.z = bps.buildingZ;
        ghost = Instantiate(selectedBuilding, position, Quaternion.identity);

        // Turn opacity down
        SpriteRenderer spriteComponent = ghost.GetComponentInChildren<SpriteRenderer>();
        spriteComponent.color = new Color(spriteComponent.color.r, spriteComponent.color.g, spriteComponent.color.b, buildingGhostOpacity);

        ghostCoroutine = StartCoroutine(updateGhostPosition());
    }

    public IEnumerator updateGhostPosition()
    {
        while (true)
        {
            // Move the ghost when mouse moves
            Vector3Int tileLocation = bps.GetTileCellLocation();

            // Get the center of the tile under the mouse?
            // Perhaps a bit redundant to first get the tile location in the tilemap and then turn it back to world position?
            Vector3 position = tilemap.GetCellCenterWorld(tileLocation);

            position.z = bps.buildingZ;
            ghost.transform.position = position;

            // Change to Vector3Int and add to list
            Vector3 mousePosition = bps.GetTileLocationInWorld();
            mousePosition.z = 10;

            if (currentMousePositionInWorld != mousePosition)
            {
                EmptyOccupiedVisualizerList();

                currentMousePositionInWorld = mousePosition;

                Vector3Int cellPosition = tilemap.WorldToCell(position);
                cellPosition.x += 5;
                cellPosition.y += 5;
                cellPosition.z = 0;

                // Get instantiated tile GameObject
                GameObject tile = tilemap.GetInstantiatedObject(cellPosition);

                if (tile != null)
                {
                    // Get the script attached to the GameObject
                    GroundTileData tileScript = tile.GetComponent<GroundTileData>();

                    if (tileScript.isOccupied || !tileScript.isWalkable)
                    {
                        GameObject visualizer = Instantiate(occupiedVisualizer, position, Quaternion.identity);
                        occupiedVisualizerList.Add(visualizer);
                    }
                    else
                    {
                        GameObject visualizer = Instantiate(availableVisualizer, position, Quaternion.identity);
                        occupiedVisualizerList.Add(visualizer);
                    }
                }
            }

            ghostOccupiedTiles.Clear();
            yield return null;
        }
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

    /// <summary>
    /// Called because OnEnable() is called before start, thus instantiating ghost in OnEnalbe without getting references lead to error
    /// </summary>
    private void updateReferences() {
        tilemap = GameObject.Find("Grid(Clone)").GetComponentInChildren<Tilemap>();
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
        occupiedVisualizerList = new List<GameObject>();
        ghostOccupiedTiles = new List<Vector3>();
    }
}

