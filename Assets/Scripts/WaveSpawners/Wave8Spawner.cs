using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave8Spawner : WaveSpawner
{
    protected override void PopulateSpawns(List<UnitBase> spawns){
        spawns.Add(UnitBase.Goblin());
        spawns.Add(UnitBase.Goblin());
        spawns.Add(UnitBase.Goblin());
        spawns.Add(UnitBase.GoblinBrute());
        spawns.Add(UnitBase.Goblin());
    }

    public override void StartNextWave()
    {
        gameObject.AddComponent<Wave9Spawner>();
        Destroy(this);
    }
}