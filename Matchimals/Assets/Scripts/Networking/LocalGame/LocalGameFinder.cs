using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LocalGameFinder : MonoBehaviour {
    private LocalBroadcaster broadcaster = null;
    private LocalListener listener = null;
    private bool initialized = false;

    public void Start() {
        this.broadcaster = gameObject.AddComponent<LocalBroadcaster>();
        this.listener = gameObject.AddComponent<LocalListener>();
    }

    // When the game scene is loaded, this is triggered.
    public void OnLevelWasLoaded(int level) {
        if (!initialized && SceneManager.GetActiveScene().name == "MainMenuScene") {
            initialized = true;
        }
    }

    public void StartBroadCasting() {
        this.broadcaster.SetBroadcasting(true);
    }

    public void StopBroadCasting() {
        this.broadcaster.SetBroadcasting(false);
    }

    public void StopListening() {
        this.listener.StopListening();
    }
}
