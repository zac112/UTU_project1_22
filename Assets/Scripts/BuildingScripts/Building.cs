using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IBuildable
{

    [SerializeField] private int _healthPoints; // building hit points, determines when it gets destroyed
    public int HealthPoints { 
        get {return _healthPoints;}
        set {
        HealthPoints = _healthPoints;
        if (_healthPoints <0){
            GetComponent<AudioPlayer>().PlayRandom(AudioType.Destroy);
        }
        }}

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

    public void Build(){
        GetComponent<AudioPlayer>().PlayRandom(AudioType.Build);        
    }

}