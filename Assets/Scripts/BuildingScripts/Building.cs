using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IBuildable
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
    public List<Vector3> OccupiedTiles { get => _occupiedTiles; }

    private Dictionary<Sprite, bool> _rotations;
    public Dictionary<Sprite, bool> Rotations { get => _rotations; }

    void Awake()
    {
        _occupiedTiles = new List<Vector3>();
    }
}
