using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IBuildable
{

    public int hp; // building hit points, determines when it gets destroyed


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

    [SerializeField] private bool _isRotated;
    public bool IsRotated { get => _isRotated; set => IsRotated = _isRotated; }

    [SerializeField] private GameObject _nextRotation;
    public GameObject NextRotation { get => _nextRotation; set => NextRotation = _nextRotation; }

    private List<Vector3> _occupiedTiles;
    public List<Vector3> OccupiedTiles { get => _occupiedTiles; }

    void Awake()
    {
        _occupiedTiles = new List<Vector3>();
    }
}