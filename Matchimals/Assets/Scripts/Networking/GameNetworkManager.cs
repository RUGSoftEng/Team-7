using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameNetworkManager : NetworkManager {
    public bool connected = false;
    public bool isHosting = false;

    public override void OnStartHost() {
        this.isHosting = true;
    }

    public override void OnClientConnect(NetworkConnection conn) {
        this.connected = true;
        base.OnClientConnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn) {
        if (SceneManager.GetActiveScene().name == "LobbyScene") {
            this.connected = false;
        }
        base.OnClientDisconnect(conn);
    }
}
