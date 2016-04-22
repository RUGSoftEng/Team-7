using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameNetworkManager : NetworkManager {
    
    public new void OnClientDisconnect(NetworkConnection conn) {
        Debug.Log("Doe");
    }

    public new void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Moi");
    }
}
