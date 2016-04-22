using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameNetworkManager : NetworkManager {
    
    public override void OnClientDisconnect(NetworkConnection conn) {
        Debug.Log("Doe");
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Moi");
    }
}
