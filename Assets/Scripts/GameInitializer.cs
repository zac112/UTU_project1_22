using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    void Start()
    {
        print(GameSettings.Instance().networkMode);
        switch (GameSettings.Instance().networkMode) {
            case NetworkMode.Offline: {
                    Instantiate<GameObject>(playerPrefab);
                    break; }
            case NetworkMode.Client: {
                    NetworkManager.Singleton.StartClient();
                    break; }
            case NetworkMode.Host: {                    
                    NetworkManager.Singleton.StartHost();
                    break; }
            default: break;
        }
    }

}
