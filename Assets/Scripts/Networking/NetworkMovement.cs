using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class NetworkMovement : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    private delegate void UpdateAction();
    UpdateAction OnUpdate;

    public override void OnNetworkSpawn() 
    {
        if (IsOwner)
        {
            OnUpdate += OwnerUpdate;
        }
        else 
        {
            OnUpdate += NotOwnerUpdate;
        }
    }

    private void Start()
    {
        OnUpdate += () => { };
    }

    void Update()
    {
        OnUpdate();
    }

    void OwnerUpdate() 
    {
        SubmitPositionRequestServerRpc(transform.position);
    }

    void NotOwnerUpdate()
    {
        transform.position = Position.Value;
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(Vector3 position, ServerRpcParams rpcParams = default)
    {
        Position.Value = position;
    }

}
