using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] GameObject grid;
    
    private Tilemap tilemap;

    [Tooltip("Tile used to fill the play area")] [SerializeField]
    Dictionary<VoronoiDiagram.TileType, List<Tile>> tiles;

    [Tooltip("Width of the map in tiles")] [SerializeField]
    private int Width;

    [Tooltip("Height of the map in tiles")] [SerializeField]
    private int Height;

    [Tooltip("Map offset, applied for all tiles")] [SerializeField]
    private Vector3Int Offset;
        
    private GameObject parentGOfow;
    private GameObject parentGOtree;
    private GameObject parentGOgold;
    private GameObject parentGOrain;
    private GameObject parentGOcactus;

    [SerializeField] GameObject fog;
    public static VoronoiDiagram voronoi;
    
    [SerializeField]GameObject goldNode;
    [SerializeField]GameObject rain;
    [SerializeField]GameObject cactus;

    [SerializeField]List<GameObject> grassTrees = new List<GameObject>();
    [SerializeField]List<GameObject> desertTrees = new List<GameObject>();

    void Generate(int width, int height, Vector3Int offset)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3Int position = new Vector3Int(i, j, 0) + offset;
                
                if (j == height - 1 ||
                    j == 0 ||
                    i == 0 ||
                    i == width - 1 ) {
                    Vector3 worldPos = tilemap.CellToWorld(position);
                    GameObject go = Instantiate(fog, worldPos, Quaternion.identity, parentGOfow.transform);
                    go.GetComponentInChildren<TileSpawner>().Init(tilemap, this, parentGOfow);
                }

                Generate(position);
            }            
        }
    }

    /**
     * Generates a tile to the given tilemap cell position.
     * Returns true if tile was generated, false otherwise.
     */
    public bool Generate(Vector3Int position) {
        if (tilemap.GetTile(position)) return false;

        List<Tile> tileOptions;
        Vector3 worldPos = tilemap.CellToWorld(position);
        VoronoiDiagram.TileType type = voronoi.GetClosestSeed(worldPos);

        tiles.TryGetValue(type, out tileOptions);
        tilemap.SetTile(position, tileOptions[UnityEngine.Random.Range(0,tileOptions.Count)]);       

        switch(type){
            case VoronoiDiagram.TileType.Desert: {
                GenerateGoldNode(worldPos);
                GenerateCactusNode(worldPos);
                GenerateTree(worldPos, desertTrees); 
                break;
                }
            case VoronoiDiagram.TileType.Grass: {
                GenerateTree(worldPos, grassTrees);
                GenerateRain(worldPos);
                break;
                }
            default: break;
        }                
        

        GameEvents.current?.OnMapChanged(worldPos, 1); 
        return true;
    }

    /**
     * Generates a tile to the given world position.
     * Returns true if tile was generated, false otherwise.
     */
    public bool Generate(Vector3 worldpos)
    {
        Vector3Int pos = tilemap.WorldToCell(worldpos);

        return Generate(pos);
    }

    private void GenerateGoldNode(Vector3 worldPos){
        float forest = voronoi.HasForest(worldPos);
        if (!voronoi.HasGoldNode(worldPos) && forest != 0 && forest != 1) return;
        GameObject go = Instantiate<GameObject>(goldNode);
        go.transform.position = worldPos;
        go.transform.parent = parentGOgold.transform;

        // set isGoldNode for the appropriate tile in the tilemap, needed for defining gold mine building range etc
        Vector3Int cellpos = tilemap.WorldToCell(worldPos);
        GameObject tile = tilemap.GetInstantiatedObject(cellpos);
        tile.GetComponent<GroundTileData>().isGoldNode = true;
    }

    private void GenerateCactusNode(Vector3 worldPos){
        float forest = voronoi.HasForest(worldPos);
        if (!voronoi.HasCactus(worldPos)) return;
        GameObject go = Instantiate<GameObject>(cactus);
        go.transform.position = worldPos;
        go.transform.parent = parentGOgold.transform;
    }

    private void GenerateTree(Vector3 worldPos, List<GameObject> trees){
        float forest = voronoi.HasForest(worldPos);
        if (forest < 0) return;
        int index = (int)(forest*trees.Count);
        int x = UnityEngine.Random.Range(-3, 3);
        int y = UnityEngine.Random.Range(-3, 3);
        Vector3 newPos = worldPos + new Vector3(x, y, 0);
        if (!ConfirmPos(newPos)) return;
        GameObject go = Instantiate<GameObject>(trees[index]);
        
        go.transform.position = newPos;
        go.transform.parent = parentGOtree.transform;
        
    }
    private bool ConfirmPos(Vector3 pos) {
        VoronoiDiagram.TileType type = voronoi.GetClosestSeed(pos);
        
        if (type == VoronoiDiagram.TileType.Water) return false;
        if (tilemap.GetTile(tilemap.WorldToCell(pos)) != null) return false;
        return true;
    }

    private void GenerateRain(Vector3 worldPos){
                if (!voronoi.HasRain(worldPos)) return;

        GameObject go = Instantiate<GameObject>(rain);
        go.transform.position = worldPos;
        go.transform.parent = parentGOrain.transform;
        
    }
    public void Awake()
    {
        parentGOfow = new GameObject("Fog of War");
        parentGOtree = new GameObject("Trees");
        parentGOgold = new GameObject("Gold");
        parentGOrain = new GameObject("Rain");

        tilemap = Instantiate(grid,Vector3.zero,Quaternion.identity).GetComponentInChildren<Tilemap>();
        voronoi = GetComponent<VoronoiDiagram>();
        voronoi.CreateDiagram(GameSettings.Instance().gameSeed);

        tiles = new Dictionary<VoronoiDiagram.TileType, List<Tile>>();
        
        tiles.Add(VoronoiDiagram.TileType.Desert, new List<Tile>{
            Resources.Load<Tile>("TilesSO/DesertTile")
        });
        tiles.Add(VoronoiDiagram.TileType.Water, new List<Tile>{
            Resources.Load<Tile>("TilesSO/WaterTile")
        });
        tiles.Add(VoronoiDiagram.TileType.Grass, new List<Tile>{
            Resources.Load<Tile>("TilesSO/GrassTile"),
            Resources.Load<Tile>("TilesSO/GrassTile2")
        });

        Generate(Width, Height, Offset);        

        AstarPath.active.Scan();
    }
}
