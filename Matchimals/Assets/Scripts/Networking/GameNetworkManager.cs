using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameNetworkManager : NetworkManager {
    
    public override void OnClientDisconnect(NetworkConnection conn) {
        Lobby lobby = Lobby.FindObjectOfType<Lobby>() as Lobby;
        if (lobby != null) {
            lobby.GoBack();
        }
    }
}
