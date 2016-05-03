// The Lobby script is created when a player is hosting or joined a game, 
// it shows the waiting players.
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;
using Google.Cast.RemoteDisplay;

public class Lobby : Returnable {
    public float connectionTestDelay = 1f, connectionTestRepeat = 0.5f;
    private bool connected = false;

    public new void Start() {
        base.Start();
        InvokeRepeating("CheckConnection", connectionTestDelay, connectionTestRepeat);
    }

    // Draws the GUI.
    public void OnGUI () {
		GUI.skin = menuSkin;

        int padding = 120;
        int width = Screen.width - 2*padding;
        int height = Screen.height - 2*padding;

        GUILayout.BeginArea(new Rect((Screen.width-width)/2, (Screen.height-height)/2, width, height));
        Player[] players = GetPlayers();

		foreach (Player player in players) {
            //Draw the waiting players.
            //Debug.Log("Player!");
		}
        if (GUILayout.Button("Start Party!", GUILayout.Height(height/2))) {
            StartGame();
        }
        if (GUILayout.Button("Cancel Party...", GUILayout.Height(height/2))) {
            GoBack();
        }
        GUILayout.EndArea();
    }
	
	private Player[] GetPlayers() {
        return GameObject.FindObjectsOfType<Player>();
	}
	
	// Checks if all players are ready.
	private bool AllReady(Player[] players) {
		bool ready = true;
		foreach (Player player in players) {
			// TODO: create ready function in Player. 
			//ready = ready && player.isReady();
		}
		return ready;
	}
	
	// Start the game with all the players in it.
	private void StartGame() {
        SceneManager.LoadScene("GameScene");
    }

    // Checks if the clients/server is still connected, otherwise close the lobby.
    private void CheckConnection() {
        if (!IsConnected()) {
            GoBack();
        }
    }

    protected new void GoBack() {
        CastRemoteDisplayManager castDisplayManager = CastRemoteDisplayManager.GetInstance();
        if (castDisplayManager.IsCasting()) {
            castDisplayManager.StopRemoteDisplaySession();
        }
        GameNetworkManager networkManager = GameObject.FindObjectOfType<GameNetworkManager>() as GameNetworkManager;
        networkManager.StopHost();
        LocalGameFinder localGameFinder = GameObject.FindObjectOfType<LocalGameFinder>();
        localGameFinder.StopBroadCasting();
        base.GoBack();
    }

    public bool IsConnected() {
        return this.connected;
    }

    public void SetConnected(bool connected) {
        this.connected = connected;
    }
}
