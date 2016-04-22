using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameDiscovery : NetworkDiscovery {
    private MainMenu menu;

    public void Start() {
        this.menu = GameObject.FindObjectOfType(typeof(MainMenu)) as MainMenu;
        Debug.Assert(menu != null);
        this.Initialize();
        StartListening();
    }

    // Called everytime a host is found.
    public override void OnReceivedBroadcast(string fromAddress, string data) {
        menu.SetHostIP(fromAddress);
    }

    // Starts broadcasting as a Server.
    public void StartHosting() {
        Reset();
        this.StartAsServer();
    }

    // Stops broadcasting as a Server and starts searching again.
    public void StartListening() {
        Reset();
        this.StartAsClient();
    }

    private void Reset() {
        if (this.running) {
            this.StopBroadcast();
        }
    }
}
