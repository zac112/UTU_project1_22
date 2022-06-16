using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave10Spawner : WaveSpawner
{
    protected override void PopulateSpawns(List<UnitBase> spawns){
        float diff = GetEnemyDifficulty(10);
        int knightsMax = (int)(diff/UnitBase.Knight().difficultyValue);
        int brutesMax = (int)(diff/UnitBase.GoblinBrute().difficultyValue);

        for(int i=0; i<UnityEngine.Random.Range(knightsMax/2,diff/UnitBase.Knight().difficultyValue);i++){
            spawns.Add(UnitBase.Knight());
            diff = UnitBase.Knight().difficultyValue;
        }

        for(int i=0; i<UnityEngine.Random.Range(brutesMax,diff/UnitBase.GoblinBrute().difficultyValue);i++){
            spawns.Add(UnitBase.GoblinBrute());
            diff = UnitBase.GoblinBrute().difficultyValue;
        }
                
        for (int i=0; i<diff;i++){
            spawns.Add(UnitBase.Goblin());
        }
    }

    public override void StartNextWave()
    {
        // Game won here
        Debug.Log("You won the game!");
        Destroy(this);
    }
}
