using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkSpawn : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (IsServer || NetworkManager.Singleton.IsHost)
        {
            GetComponent<NetworkObject>().Spawn();
        }
        else {
            BuildServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void BuildServerRpc() {
        GetComponent<NetworkObject>().Spawn();
    }
}
