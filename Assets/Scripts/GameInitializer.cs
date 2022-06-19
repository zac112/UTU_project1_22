using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Networking;
using Unity.Netcode.Transports.UNET;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject spawnpointInitializer;


    void Start()
    {
        switch (GameSettings.Instance().networkMode) {
            
            case NetworkMode.Client: {
                    NetworkManager.Singleton.StartClient();
                    return; 
            }
            case NetworkMode.Offline:{
                    NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = "127.0.0.1";
                    break;
            }
            case NetworkMode.Host: {                    
                    break; 
            }
            default: break;
        }

        NetworkManager.Singleton.StartHost();
        Instantiate(spawnpointInitializer);
    }

}
