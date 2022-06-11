using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDifficulty : MonoBehaviour
{

    public int DifficultyLevel;

    void Start()
    {
        DifficultyLevel=1;
    }

    //call this when you want to increase the difficulty level
    void IncreaseDifficulty(int x){
        DifficultyLevel++;
    }
}
