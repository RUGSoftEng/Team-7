using UnityEngine;
using System.Collections;

public class LocalGameFinder : MonoBehaviour {
    private LocalBroadcaster broadcaster = null;
    private LocalListener listener = null;

    public void Start() {
        this.broadcaster = gameObject.AddComponent<LocalBroadcaster>();
        this.listener = gameObject.AddComponent<LocalListener>();
        StartListening();
    }

	public void StartBroadCasting() {
        this.broadcaster.SetBroadcasting(true);
    }

    public void StartListening()
    {
        this.listener.StartListening();
    }

    public void StopBroadCasting() {
        this.broadcaster.SetBroadcasting(false);
    }

    public void StopListening() {
        this.listener.StopListening();
    }

    /*
    public void OnGUI() {
        if (broadcaster.IsBroadcasting()) {
            if (GUI.Button(new Rect(10, 10, 150, 100), "Stop broadcast")) {
                StopBroadCasting();
            }
        } else {
            if (GUI.Button(new Rect(10, 10, 150, 100), "Start broadcast"))
            {
                StartBroadCasting();
            }
        }
    }*/
}
