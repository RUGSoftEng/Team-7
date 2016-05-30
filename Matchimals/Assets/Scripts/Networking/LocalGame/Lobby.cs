// The Lobby script is created when a player is hosting or joined a game, 
// it shows the waiting players.
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;
using Google.Cast.RemoteDisplay;

public class Lobby : Menu {
    private static float CONNECTION_CHECK_INTERVAL = 0.5f;
    private static int DIFFICULTIES = 3;
    private int difficulty = 0;
    private GameNetworkManager gameNetworkManager;
    private Texture arrowIcon, arrowFlippedIcon;
    private Texture[] difficultyIcons;

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

        // Load menu assets.
        arrowIcon = Resources.Load<Texture>("Menu/arrow");
        arrowFlippedIcon = Resources.Load<Texture>("Menu/arrow-flipped");
        difficultyIcons = Resources.LoadAll<Texture>("Menu/Difficulties");
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

        int width = (int)(Screen.width*0.8);
		int height = Screen.height/4;
		int buttonHeight = height / 2;
        int iconSize = height;

        int offCentre = -(int)(Screen.height * 0.08);
        if (gameNetworkManager.isHosting)
        {
            // Draw difficulty;
            float iconWidth = Screen.width * 0.4f;
            float iconHeight = Screen.height * 0.4f;
            Texture diffIcon = difficultyIcons[difficulty];
            GUI.DrawTexture(new Rect((Screen.width - iconWidth) / 2f, (Screen.height - iconHeight) / 2 + offCentre, iconWidth, iconHeight), diffIcon);

            // Difficulty select menu.
            GUI.backgroundColor = Color.clear;
            GUILayout.BeginArea(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2 + offCentre, width, height));
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(arrowFlippedIcon, GUILayout.Height(iconSize), GUILayout.Width(iconSize)))
            {
                ChangeDifficulty(-1);
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(arrowIcon, GUILayout.Height(iconSize), GUILayout.Width(iconSize)))
            {
                ChangeDifficulty(1);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            GUI.backgroundColor = Color.white;
        }

        // Game start/close buttons.
        offCentre = (int)(Screen.height * 0.35);

        GUILayout.BeginArea(new Rect((Screen.width-width)/2, (Screen.height-height)/2+offCentre, width, height));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Cancel Party...", GUILayout.Height(buttonHeight))) {
            CloseLobby();
        }
        GUILayout.FlexibleSpace();
        if (gameNetworkManager.isHosting && GUILayout.Button("Start Party!", GUILayout.Height(buttonHeight)))
            ThrowPlayersInGame();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
	
	private void ThrowPlayersInGame() {
        foreach (Player p in GameObject.FindObjectsOfType<Player>()) {
            p.RpcGotoGame();
        }
	}

    private void ChangeDifficulty(int step) {
        difficulty += step;
        difficulty = (int) Mathf.Repeat(difficulty, DIFFICULTIES);
        Deck deck = GameObject.FindObjectOfType<Deck>();
        switch (difficulty)
        {
            case 0:
                deck.symbolsPerCard = 4;
                break;
            case 1:
                deck.symbolsPerCard = 6;
                break;
            case 2:
                deck.symbolsPerCard = 8;
                break;
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
