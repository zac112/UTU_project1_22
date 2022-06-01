using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenWallScript : MonoBehaviour, IBuildable
{
    [SerializeField] private int _width;
    public int Width
    {
        get => _width; set => Width = _width;
    }

    [SerializeField] private int _length;
    public int Length
    {
        get => _length; set => Length = _length;
    }

    private List<Vector3> _occupiedTiles;
    public List<Vector3> OccupiedTiles { get => _occupiedTiles; set => OccupiedTiles = _occupiedTiles; }
}
