using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSpawner : MonoBehaviour
{    
    Tilemap map;
    MapGenerator generator;
    GameObject parentGO;

    public void Init(Tilemap map, MapGenerator generator, GameObject parentGO )
    {
        this.map = map;
        this.generator = generator;
        this.parentGO = parentGO;
    }

    private List<Vector3Int> getNeighbors(Vector3Int origin, int radius)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        for (int i = -radius; i < radius+1; i++)
        {
            for (int j = -radius; j < radius+1; j++)
            {
                 neighbors.Add(origin + new Vector3Int(i, j, 0));
            }
        }

        return neighbors;
     }

    private List<Vector3Int> getEdges(Vector3Int origin, int radius)
    {
        List<Vector3Int> edges = new List<Vector3Int>();
        for (int i = -radius; i < radius + 1; i++)
        {
            edges.Add(new Vector3Int(origin.x+i, origin.y+radius, 0));
            edges.Add(new Vector3Int(origin.x+i, origin.y-radius, 0));
            if (i != -radius && i != radius)
            {
                edges.Add(new Vector3Int(origin.x+radius, origin.y+i, 0));
                edges.Add(new Vector3Int(origin.x-radius, origin.y+i, 0));
            }
        }

        return edges;
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


        for (int i = -1; i < 2; i++){
            for (int j = -1; j < 2; j++)
            {
                neighbors.Add(new Vector3Int(cellpos.x+i, cellpos.y+j, 0));
            }
        }

        foreach (Vector3Int neighbor in neighbors) {
            if (generator.Generate(neighbor)) { 
                GameObject go = Instantiate(transform.parent.gameObject, map.CellToWorld(neighbor), Quaternion.identity, parentGO.transform);
                go.name = "FOW";
                go.GetComponentInChildren<TileSpawner>().Init(map, generator, parentGO);
            }
        }
    }

}
