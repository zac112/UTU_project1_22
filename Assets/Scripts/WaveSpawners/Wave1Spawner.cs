using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave1Spawner : WaveSpawner
{
    protected override void PopulateSpawns(List<UnitBase> spawns){
        float diff = GetEnemyDifficulty(1);
        for (int i=0; i<diff;i++){
            spawns.Add(UnitBase.Goblin());
        }
    }

    public override void StartNextWave()
    {
        gameObject.AddComponent<Wave2Spawner>();
        Destroy(this);
    }
}
