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
    Dictionary<VoronoiDiagram.TileType, Tile> tiles;

    [Tooltip("Width of the map in tiles")] [SerializeField]
    private int Width;

    [Tooltip("Height of the map in tiles")] [SerializeField]
    private int Height;

    [Tooltip("Map offset, applied for all tiles")] [SerializeField]
    private Vector3Int Offset;
        
    private GameObject FOWparentGO;

    [SerializeField] GameObject fog;
    public static VoronoiDiagram voronoi;
    
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
                    GameObject go = Instantiate(fog, worldPos, Quaternion.identity, FOWparentGO.transform);
                    go.GetComponentInChildren<TileSpawner>().Init(tilemap, this, FOWparentGO);
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

        Tile tile;
        Vector3 worldPos = tilemap.CellToWorld(position);
        VoronoiDiagram.TileType type = voronoi.GetClosestSeed(worldPos);

        tiles.TryGetValue(type, out tile);
        tilemap.SetTile(position, tile);       

        switch(type){
            case VoronoiDiagram.TileType.Desert: GenerateTree(worldPos, desertTrees); break;
            case VoronoiDiagram.TileType.Grass: GenerateTree(worldPos, grassTrees); break;
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

    private void GenerateTree(Vector3 worldPos, List<GameObject> trees){
        float forest = voronoi.HasForest(worldPos);
        if (forest < 0 ) return;
        int index = (int)(forest*trees.Count);
        GameObject go = Instantiate<GameObject>(trees[index]);
        go.transform.position = worldPos;
        
    }
    public void Awake()
    {
        FOWparentGO = new GameObject("Fog of War");
        tilemap = Instantiate(grid,Vector3.zero,Quaternion.identity).GetComponentInChildren<Tilemap>();
        voronoi = GetComponent<VoronoiDiagram>();

        tiles = new Dictionary<VoronoiDiagram.TileType, Tile>();
        tiles.Add(VoronoiDiagram.TileType.Desert, Resources.Load<Tile>("Tiles/desertTiles1"));
        tiles.Add(VoronoiDiagram.TileType.Water, Resources.Load<Tile>("Tiles/waterTiles1"));
        tiles.Add(VoronoiDiagram.TileType.Grass, Resources.Load<Tile>("Tiles/grassTiles1"));

        Generate(Width, Height, Offset);        

        AstarPath.active.Scan();
    }
}
