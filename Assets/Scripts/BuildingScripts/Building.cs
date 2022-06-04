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

    private Dictionary<Sprite, bool> _rotations = new Dictionary<Sprite, bool>();
    public Dictionary<Sprite, bool> Rotations { get => _rotations; }

    // Serialized so initializing a new list would overwrite?
    [SerializeField] private List<KeyValuePair> rotationList;

    void Awake()
    {
        _occupiedTiles = new List<Vector3>();
        // _rotations = new Dictionary<Sprite, bool>();
        iBuildable = this.GetComponent<IBuildable>();
    }

    void Start() 
    {
        foreach (var kvp in rotationList)
        {
            //Debug.Log($"Sprite: {kvp.Sprite}");
            //Debug.Log($"IsRotated: {kvp.IsRotated}");
            _rotations.Add(kvp.Sprite, kvp.IsRotated);
        }
    }
}

[Serializable]
public class KeyValuePair
{
    public Sprite Sprite;
    public bool IsRotated;
}
