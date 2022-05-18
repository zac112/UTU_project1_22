using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceGenerator : MonoBehaviour
{
    [Tooltip("Tilemap used for the resource storage")] [SerializeField]
    private Tilemap Tilemap;

    [Tooltip("Tile used for storing each value")] [SerializeField]
    private TileBase Tile;

    [Tooltip("Seed used for resource generation")] [SerializeField]
    private float Seed;

    void Generate(int width, int height, Vector3Int offset)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Create a new tile
                Vector3Int position = new Vector3Int(i, j, 0) + offset;
                
                // Move on to the next tile if there already is a tile at this position
                if (Tilemap.GetTile(position))
                {
                    continue;
                }
                
                Tilemap.SetTile(position, Tile);
                
                // Determine the resource value using 4D simplex noise
                float resourceValue = noise.snoise(new float4(position.x, position.y, position.z, Seed));
                
                // Transform the resourceValue to be between 0 and 1
                resourceValue = (resourceValue + 1) / 2;
                
                // Set the tile color to match the resource value
                Tilemap.SetColor(position, new Color(resourceValue, resourceValue, resourceValue));
            }
        }
    }

    private void Start()
    {
        Generate(50, 50, new Vector3Int(-25, -25, 0));
    }
}