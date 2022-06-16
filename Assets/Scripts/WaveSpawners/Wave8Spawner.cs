using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave8Spawner : WaveSpawner
{
    protected override void PopulateSpawns(List<UnitBase> spawns){
       float diff = GetEnemyDifficulty(8);

        for(int i=0; i<UnityEngine.Random.Range(0,diff/UnitBase.Knight().difficultyValue);i++){
            spawns.Add(UnitBase.Knight());
            diff = UnitBase.Knight().difficultyValue;
        }

        for(int i=0; i<UnityEngine.Random.Range(0,diff/UnitBase.GoblinBrute().difficultyValue);i++){
            spawns.Add(UnitBase.GoblinBrute());
            diff = UnitBase.GoblinBrute().difficultyValue;
        }
                
        for (int i=0; i<diff;i++){
            spawns.Add(UnitBase.Goblin());
        }
    }

    public override void StartNextWave()
    {
        gameObject.AddComponent<Wave9Spawner>();
        Destroy(this);
    }
}
