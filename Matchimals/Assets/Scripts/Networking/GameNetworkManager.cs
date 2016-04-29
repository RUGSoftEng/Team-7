using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameNetworkManager : NetworkManager {

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Connected!");
        base.OnClientConnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn) {
        Lobby lobby = FindObjectOfType<Lobby>() as Lobby;
        if (lobby != null) {
            lobby.GoBack();
            MainMenu menu = FindObjectOfType<MainMenu>() as MainMenu;
            Debug.Assert(menu != null);
            menu.hostIP = null;
        }
    }
}
