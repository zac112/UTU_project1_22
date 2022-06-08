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
}
