using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSpawner : MonoBehaviour
{    
    Tilemap map;
    MapGenerator generator;

     public void Init(Tilemap map, MapGenerator generator)
    {
        this.map = map;
        this.generator = generator;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player")) {
            RevealTiles();
            Destroy(transform.parent.gameObject);
        }
        
    }

    void RevealTiles() {
        Vector3Int cellpos = map.WorldToCell(transform.parent.position);
        List<Vector3Int> neighbors = new List<Vector3Int>();

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                neighbors.Add(new Vector3Int(cellpos.x+i, cellpos.y+j, 0));
            }
        }

        foreach (Vector3Int neighbor in neighbors) {
            if (generator.Generate(neighbor)) { 
                GameObject go = Instantiate(transform.parent.gameObject, map.CellToWorld(neighbor), Quaternion.identity);
                go.name = "FOW";
                go.GetComponentInChildren<TileSpawner>().Init(map, generator);
            }
        }
    }

}