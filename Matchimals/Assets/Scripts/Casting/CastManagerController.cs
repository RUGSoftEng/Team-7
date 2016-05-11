using UnityEngine;
using Google.Cast.RemoteDisplay;
using System.Collections;
using UnityEngine.SceneManagement;

public class CastManagerController : MonoBehaviour {
    // The camera to cast to.
    public Camera remoteCamera;
    private CastRemoteDisplayManager castDisplayManager;

    void Start () {
        this.castDisplayManager = CastRemoteDisplayManager.GetInstance();
        castDisplayManager.RemoteDisplayErrorEvent.AddListener(OnRemoteDisplayError);
        castDisplayManager.RemoteDisplaySessionEndEvent.AddListener(OnRemoteDisplaySessionEnd);
        castDisplayManager.RemoteDisplayCamera = remoteCamera;
    }

    public void OnRemoteDisplayError(CastRemoteDisplayManager manager) {
        Debug.LogError("Casting failed: " + manager.GetLastError());
        manager.StopRemoteDisplaySession();
    }

    public void OnRemoteDisplaySessionEnd(CastRemoteDisplayManager manager)
    {
        CloseGame();
    }

    // Close the game and return to the main menu.
    private void CloseGame() {
        GameNetworkManager networkManager = GameObject.FindObjectOfType<GameNetworkManager>();
        if (networkManager.isNetworkActive) { networkManager.StopHost(); }
        SceneManager.LoadScene("MainMenuScene");
    }

}
