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
        if (IsServer || NetworkManager.Singleton.IsHost) GetComponent<NetworkObject>().Despawn();
        GameStats.EnemiesKilled++;
    }

}
