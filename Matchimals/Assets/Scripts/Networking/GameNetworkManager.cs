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
        this.connected = false;
        if (SceneManager.GetActiveScene().name == "GameScene") {
            SceneManager.LoadScene("MainMenuScene");
        }
        base.OnClientDisconnect(conn);
    }
}
