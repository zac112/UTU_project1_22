using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] GameObject grid;

    [Tooltip("Tilemap used for terrain tiles")] [SerializeField]
    Tilemap tilemap;

    [Tooltip("Tile used to fill the play area")] [SerializeField]
    Tile tile;

    [Tooltip("Width of the map in tiles")] [SerializeField]
    private int Width;

    [Tooltip("Height of the map in tiles")] [SerializeField]
    private int Height;

    [Tooltip("Map offset, applied for all tiles")] [SerializeField]
    private Vector3Int Offset;

    [SerializeField] GameObject fog;

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
                    Instantiate(fog, worldPos, Quaternion.identity); }

                // Move on to the next tile if there already is a tile at this position
                if (tilemap.GetTile(position))
                {
                    continue;
                }

                tilemap.SetTile(position, tile);

                
            }
            
        }
    }

    public void Start()
    {
        tilemap = Instantiate(grid,Vector3.zero,Quaternion.identity).GetComponentInChildren<Tilemap>();
        Generate(Width, Height, Offset);
    }
}
