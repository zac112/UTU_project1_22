using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyStats : Stats
{   
    protected override void PostStart()
    {
        
    }

    protected override void ActOnDeath()
    {
        GameStats.EnemiesKilled++;
    }

}
