using System;
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

    public List<KeyValuePair> rotationList;

    void Awake()
    {
        _occupiedTiles = new List<Vector3>();
        _rotations = new Dictionary<Sprite, bool>();
        rotationList = new List<KeyValuePair>();
    }

    void Start() 
    {
        foreach (var kvp in rotationList)
        {
            Rotations[kvp.sprite] = kvp.rotated;
        }
    }
}

[Serializable]
public class KeyValuePair
{
    public Sprite sprite;
    public bool rotated;
}
