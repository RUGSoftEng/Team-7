using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameNetworkManager : NetworkManager {
    
    public new void OnClientDisconnect(NetworkConnection conn) {
        Debug.Log("Doe");
    }
}
