using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WaveSpawner : MonoBehaviour
{
    private Spawnpoint[] spawnpoints;
    private List<UnitBase> spawns = new List<UnitBase>();

    // Start is called before the first frame update
    void Start()
    {
        spawnpoints = GameObject.FindObjectsOfType<Spawnpoint>();    
        PopulateSpawns(spawns);

        StartCoroutine("Spawn");
    }

    //Fills the list with units to spawn during the wave
    protected abstract void PopulateSpawns(List<UnitBase> spawns);

    public abstract void StartNextWave();

    // Update is called once per frame
    void Spawn()
    {
        while(spawns.Count > 0){
            UnitBase unit = spawns[0];
            spawns.RemoveAt(0);

            spawnpoints[UnityEngine.Random.Range(0,spawnpoints.Length)].SpawnUnit(unit);
        }
    }
}