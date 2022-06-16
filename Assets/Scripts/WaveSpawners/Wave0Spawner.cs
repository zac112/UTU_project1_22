using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave0Spawner : WaveSpawner
{
    protected override void PopulateSpawns(List<UnitBase> spawns){}

    public override void StartNextWave()
    {
        gameObject.AddComponent<Wave1Spawner>();
        Destroy(this);
    }
}
