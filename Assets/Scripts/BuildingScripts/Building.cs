using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class Building : Stats, IBuildable
{
    public int HealthPoints { 
        get {return currentHealth;}
        set { SetHP(value); }
    }

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
        
    NetworkVariable<Vector3> networkBuildingPos = new NetworkVariable<Vector3>();

    void Awake()
    {
        _occupiedTiles = new List<Vector3>();     
    }
    public void SetActive(bool enabled) { this.enabled = enabled; }
    public void Build(){}
    protected override void PostStart(){
        Tilemap tilemap = GameObject.FindObjectOfType<Tilemap>();
        GetComponent<AudioPlayer>().PlayRandom(AudioType.Build);
        IBuildable selectedBuildingScript = GetComponent<IBuildable>();

        // Add occupiedTiles to the building instance
        for (int i = 0; i < OccupiedTiles.Count; i++)
        {
            selectedBuildingScript.AddToOccupiedTiles(OccupiedTiles[i]);

            Vector3Int cellPosition = tilemap.WorldToCell(OccupiedTiles[i]);
            cellPosition.x += 5;
            cellPosition.y += 5;
            cellPosition.z = 0;

            // Tile script
            GameObject tile = tilemap.GetInstantiatedObject(cellPosition);

            if (tile != null)
            {
                GroundTileData tileScript = tile.GetComponent<GroundTileData>();
                tileScript.isOccupied = true;
            }
        }
        GameEvents.current.OnMapChanged(transform.position, OccupiedTiles.Count);
        GameEvents.current.OnBuildingBuilt(gameObject);

    }

    protected override void ActOnDeath()
    {
        GetComponent<AudioPlayer>().PlayRandom(AudioType.Destroy);
        if (NetworkManager.IsHost)
        {
            NetworkObject.Despawn();
            Destroy(gameObject);
        }
    }
}