using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave3Spawner : WaveSpawner
{
    protected override void PopulateSpawns(List<UnitBase> spawns){
        float diff = GetEnemyDifficulty(3);
        
        spawns.Add(UnitBase.GoblinBrute());
        diff -= UnitBase.GoblinBrute().difficultyValue;
        
        for (int i=0; i<diff;i++){
            spawns.Add(UnitBase.Goblin());
        }
    }

    public override void StartNextWave()
    {
        gameObject.AddComponent<Wave4Spawner>();
        Destroy(this);
    }
}
