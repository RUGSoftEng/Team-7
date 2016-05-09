using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameNetworkManager : NetworkManager {

    public override void OnClientConnect(NetworkConnection conn)
    {
        GetLobby().SetConnected(true);
        base.OnClientConnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn) {
        if (SceneManager.GetActiveScene().name == "MainMenuScene") {
            GetLobby().SetConnected(false);
        } else {
            SceneManager.LoadScene("MainMenuScene");
        }
        base.OnClientDisconnect(conn);
    }

    private Lobby GetLobby() {
        Lobby lobby = FindObjectOfType<Lobby>() as Lobby;
        Debug.Assert(lobby != null);
        return lobby;
    }
}
