// The Lobby script is created when a player is hosting or joined a game, 
// it shows the waiting players.
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;
using Google.Cast.RemoteDisplay;

public class Lobby : Menu {
    private static float CONNECTION_CHECK_INTERVAL = 0.5f;
    private GameNetworkManager gameNetworkManager;

    public new void Start() {
        base.Start();
        CastRemoteDisplayManager castDisplayManager = CastRemoteDisplayManager.GetInstance();
        if (castDisplayManager.IsCasting())
        {
            castDisplayManager.RemoteDisplayErrorEvent.AddListener(OnRemoteDisplayError);
            castDisplayManager.RemoteDisplaySessionEndEvent.AddListener(OnRemoteDisplaySessionEnd);
        }
        this.gameNetworkManager = GameObject.FindObjectOfType<GameNetworkManager>();
        InvokeRepeating("CheckConnection", CONNECTION_CHECK_INTERVAL, CONNECTION_CHECK_INTERVAL);
    }

    public void CheckConnection()
    {
        if (!gameNetworkManager.connected)
            CloseLobby();
    }

    public void OnRemoteDisplayError(CastRemoteDisplayManager manager)
    {
        Debug.LogError("Casting failed: " + manager.GetLastError());
        manager.StopRemoteDisplaySession();
    }

    public void OnRemoteDisplaySessionEnd(CastRemoteDisplayManager manager)
    {
        CloseLobby();
    }

    // Draws the GUI.
    public void OnGUI () {
		GUI.skin = menuSkin;

        int padding = 120;
        int width = Screen.width - 2*padding;
        int height = Screen.height - 2*padding;

        GUILayout.BeginArea(new Rect((Screen.width-width)/2, (Screen.height-height)/2, width, height));

		if (gameNetworkManager.isHosting && GUILayout.Button("Start Party!", GUILayout.Height(height / 2)))
        {
            ThrowPlayersInGame();
        }
        
        if (GUILayout.Button("Cancel Party...", GUILayout.Height(height/2))) {
            CloseLobby();
        }
        GUILayout.EndArea();
    }
	
	private void ThrowPlayersInGame() {
        foreach (Player p in GameObject.FindObjectsOfType<Player>()) {
            p.RpcGotoGame();
        }
	}

    public void CloseLobby() {
        CastRemoteDisplayManager castDisplayManager = CastRemoteDisplayManager.GetInstance();
        if (castDisplayManager.IsCasting()) {
            castDisplayManager.StopRemoteDisplaySession();
        }
        gameNetworkManager.StopHost();
        LocalGameFinder localGameFinder = GameObject.FindObjectOfType<LocalGameFinder>();
        localGameFinder.StopBroadCasting();
        SceneManager.LoadScene("MainMenuScene");
    }
}
