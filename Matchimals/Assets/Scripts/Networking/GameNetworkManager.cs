using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameNetworkManager : NetworkManager {

    public override void OnClientConnect(NetworkConnection conn)
    {
        GetLobby().SetConnected(true);
        base.OnClientConnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn) {
        GetLobby().SetConnected(false);
        base.OnClientDisconnect(conn);
    }

    private Lobby GetLobby() {
        Lobby lobby = FindObjectOfType<Lobby>() as Lobby;
        Debug.Assert(lobby != null);
        return lobby;
    }
}
