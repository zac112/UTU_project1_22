using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetworkInitializer : NetworkBehaviour
{    
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) {
            Destroy(GetComponent<AudioListener>());
            Destroy(GetComponent<PlayerMovement>());
            Destroy(transform.Find("AudioPlayer").gameObject);
            gameObject.tag = "Untagged";
        }
    }
     
}
