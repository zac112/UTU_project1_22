using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "GroundTile", menuName = "ScriptableObjects/GroundTile", order = 0)]
public class GroundTile : Tile
{
    public bool isOccupied = false;
    public float moveSpeed = 1;
}
