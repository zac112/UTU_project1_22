using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "GroundTile", menuName = "ScriptableObjects/GroundTile", order = 0)]
public class GroundTile : Tile
{
    /// <summary>
    /// Determines if a building can be built on this tile
    /// </summary>
    public bool isOccupied = false;

    /// <summary>
    /// Determines if a tile is walkable
    /// </summary>
    public bool isWalkable = true;

    /// <summary>
    /// Determines the movement speed multiplier of the tile
    /// </summary>
    public float moveSpeedMultiplier = 1;

    public LayerMask layer;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        tileData.gameObject.layer = ToLayer(layer);
    }

    /// Source: https://forum.unity.com/threads/get-the-layernumber-from-a-layermask.114553/
    /// <summary> Converts given bitmask to layer number </summary>
    /// <returns> layer number </returns>
    public static int ToLayer(int bitmask)
    {
        int result = bitmask > 0 ? 0 : 31;
        while (bitmask > 1)
        {
            bitmask = bitmask >> 1;
            result++;
        }
        return result;
    }
}
