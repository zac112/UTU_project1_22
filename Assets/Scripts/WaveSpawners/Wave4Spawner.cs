using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave4Spawner : WaveSpawner
{
    protected override void PopulateSpawns(List<UnitBase> spawns){
        float diff = GetEnemyDifficulty(4);
        
        spawns.Add(UnitBase.GoblinBrute());
        spawns.Add(UnitBase.GoblinBrute());
        diff -= 2*UnitBase.GoblinBrute().difficultyValue;
        
        for (int i=0; i<diff;i++){
            spawns.Add(UnitBase.Goblin());
        }
    }

    public override void StartNextWave()
    {
        gameObject.AddComponent<Wave5Spawner>();
        Destroy(this);
    }
}
