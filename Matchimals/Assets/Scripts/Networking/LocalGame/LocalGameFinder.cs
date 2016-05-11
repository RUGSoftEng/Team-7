using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LocalGameFinder : MonoBehaviour {
    private LocalBroadcaster broadcaster = null;
    private LocalListener listener = null;

    public void Start() {
        this.broadcaster = gameObject.AddComponent<LocalBroadcaster>();
        this.listener = gameObject.AddComponent<LocalListener>();
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
