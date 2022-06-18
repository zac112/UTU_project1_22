using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class NetworkMovement : WASDMovement
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        Camera.main.GetComponent<MainCameraController>().target = gameObject.transform;
    }

    void FixedUpdate()
    {
        if (!canMove) return;
        if (gameObject == NetworkManager.LocalClient.PlayerObject.gameObject)
        {

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 position = transform.position;
            position.x = position.x + speed * horizontal * Time.deltaTime;
            position.y = position.y + speed * vertical * Time.deltaTime;

            SubmitPositionRequestServerRpc(position);
            rigidbody2d.MovePosition(position);
        }
        else
        {
            transform.position = Position.Value;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitPositionRequestServerRpc(Vector3 position, ServerRpcParams rpcParams = default)
    {
        Position.Value = position;
    }

    void Update()
    {
    }
}
