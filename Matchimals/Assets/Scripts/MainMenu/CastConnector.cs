﻿// The Lobby script is created when a player is hosting or joined a game, 
// it shows the waiting players.
using UnityEngine;
using UnityEngine.Networking;
using Google.Cast.RemoteDisplay;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CastConnector : Returnable {
    public float castConnectDelay = 0f, castConnectRepeat = 1f;
    // The cast manager to use.
	private CastRemoteDisplayManager castDisplayManager;
    private IList<CastDevice> castDevices = new List<CastDevice>();
    private GameObject castSearchAnimation;

    public new void Start() {
        base.Start();
        castSearchAnimation = Instantiate(Resources.Load("Menu/CastSearchAnimation", typeof(GameObject))) as GameObject;
        Debug.Assert(castSearchAnimation != null);
        castSearchAnimation.transform.localPosition = new Vector3(0, 0.25f, 6);

        // Setup the castManager.
        this.castDisplayManager = CastRemoteDisplayManager.GetInstance();
        castDisplayManager.RemoteDisplaySessionStartEvent.AddListener(OnRemoteDisplaySessionStart);
        castDisplayManager.RemoteDisplaySessionEndEvent  .AddListener(OnRemoteDisplaySessionEnd);
        castDisplayManager.RemoteDisplayErrorEvent       .AddListener(OnRemoteDisplayError);
        castDisplayManager.CastDevicesUpdatedEvent       .AddListener(OnCastDevicesUpdated);
        this.castDevices = castDisplayManager.GetCastDevices();
        InvokeRepeating("TryConnectCast", castConnectDelay, castConnectRepeat);
    }

    private void TryConnectCast() {
        if (castDevices.Count > 0 && !castDisplayManager.IsCasting()) {
            CastDevice device = castDevices[0];
            castDisplayManager.SelectCastDevice(device.DeviceId);
        }
    }

    public void OnCastDevicesUpdated(CastRemoteDisplayManager manager) {
        this.castDevices = manager.GetCastDevices();
    }

    public void OnRemoteDisplaySessionStart(CastRemoteDisplayManager manager) {
        // Start hosting and join lobby.
        Destroy(castSearchAnimation);
        LocalGameFinder localGameFinder = GameObject.FindObjectOfType<LocalGameFinder>();
        localGameFinder.StartBroadCasting();
        GameNetworkManager networkManager = GameObject.FindObjectOfType<GameNetworkManager>();
        networkManager.StartHost();
        SceneManager.LoadScene("LobbyScene");
    }

    public void OnRemoteDisplayError(CastRemoteDisplayManager manager)
    {
        Debug.LogError("Casting failed: " + manager.GetLastError());
        manager.StopRemoteDisplaySession();
    }

    public void OnRemoteDisplaySessionEnd(CastRemoteDisplayManager manager) {
        GoBack();
    }

    // Draws the GUI.
    public void OnGUI () {
		GUI.skin = menuSkin;

		int width = Screen.width/3;
		int height = Screen.height/4;
		int buttonHeight = height / 2;


		GUILayout.BeginArea(new Rect((Screen.width-width)/2, (Screen.height-height)/2+2.5f*buttonHeight, width, height));
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Cancel Party...", GUILayout.Height(buttonHeight))) {
            GoBack();
        }
        GUILayout.EndArea();
    }

    public new void GoBack() {
        Destroy(castSearchAnimation);
        LocalGameFinder localGameFinder = GameObject.FindObjectOfType<LocalGameFinder>();
        localGameFinder.StopBroadCasting();
        base.GoBack();
    }
}
