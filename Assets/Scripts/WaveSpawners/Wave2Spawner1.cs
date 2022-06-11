using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave2Spawner : WaveSpawner
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
        //Game won here
        Destroy(this);
    }
}
