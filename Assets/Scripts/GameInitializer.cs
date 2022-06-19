using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject spawnpointInitializer;
    [SerializeField] GameObject inputListener;


    void Start()
    {
        switch (GameSettings.Instance().networkMode) {
            case NetworkMode.Offline: {
                    Instantiate<GameObject>(playerPrefab);
                    Instantiate(spawnpointInitializer);
                    Instantiate(inputListener);
                    break; }
            case NetworkMode.Client: {
                    NetworkManager.Singleton.StartClient();
                    break; }
            case NetworkMode.Host: {                    
                    NetworkManager.Singleton.StartHost();
                    Instantiate(spawnpointInitializer);
                    Instantiate(inputListener);
                    break; }
            default: break;
        }
    }

}
