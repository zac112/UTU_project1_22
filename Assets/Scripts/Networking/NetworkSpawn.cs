using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkSpawn : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print(IsServer);
        print(IsHost);
        print(NetworkManager.Singleton.IsHost);
        if (IsServer || NetworkManager.Singleton.IsHost) {
            GetComponent<NetworkObject>().Spawn();
        }
    }

}
