using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSpawner : MonoBehaviour
{    
    Tilemap map;
    MapGenerator generator;
    GameObject parentGO;
    ParticleSystem.EmissionModule em;

    public void Init(Tilemap map, MapGenerator generator, GameObject parentGO )
    {
        this.map = map;
        this.generator = generator;
        this.parentGO = parentGO;
        em = transform.parent.gameObject.GetComponent<ParticleSystem>().emission;
        StartCoroutine(SpawnMore());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<FogRevealer>()) {
            RevealTiles();
            Destroy(transform.parent.gameObject);            
        }        
    }

    IEnumerator SpawnMore()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(60,120));

        GetNeighbors().ForEach((neighbor) => {
            GameObject tile = map.GetInstantiatedObject(neighbor);
            if (!tile) return;
            if (tile.GetComponent<GroundTileData>().fog) return;

            CreateFog(neighbor);
        });

        Vector3Int thisCell = map.WorldToCell(transform.transform.position);
        map.SetTile(thisCell, null);
    }

    void RevealTiles() {        
        foreach (Vector3Int neighbor in GetNeighbors()) {
            if (generator.Generate(neighbor)) {
                CreateFog(neighbor);
            }
        }
    }

    private List<Vector3Int> GetNeighbors()
    {
        Vector3Int cellpos = map.WorldToCell(transform.transform.position);
        List<Vector3Int> neighbors = new List<Vector3Int>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                neighbors.Add(new Vector3Int(cellpos.x + i, cellpos.y + j, 0));
            }
        }
        return neighbors;
    }
    private void CreateFog(Vector3Int cellPos) 
    {
        GameObject g = map.GetInstantiatedObject(cellPos);
        GameObject go = Instantiate(transform.parent.gameObject, map.CellToWorld(cellPos), Quaternion.identity, g.transform);
        g.GetComponent<GroundTileData>().fog = go;
        go.name = "FOW";
        go.GetComponentInChildren<TileSpawner>().Init(map, generator, parentGO);
    }
}
