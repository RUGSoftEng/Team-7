using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameDiscovery : NetworkDiscovery {
    private MainMenu menu;

    public void Start() {
        this.menu = GameObject.FindObjectOfType(typeof(MainMenu)) as MainMenu;
        Debug.Assert(menu != null);
    }

    // Called everytime a host is found.
    public override void OnReceivedBroadcast(string fromAddress, string data) {
        menu.SetHostIP(fromAddress);
    }

    // Starts broadcasting as a Server.
    public void StartHosting() {
        GameDiscovery newGameDiscovery = Instantiate(this);
        newGameDiscovery.name = this.gameObject.name;
        newGameDiscovery.Initialize();
        newGameDiscovery.StartAsServer();
        Destroy(this.gameObject);
    }

    // Stops broadcasting as a Server and starts searching again.
    public void StartListening() {
        GameDiscovery newGameDiscovery = Instantiate(this);
        newGameDiscovery.name = this.gameObject.name;
        newGameDiscovery.Initialize();
        newGameDiscovery.StartAsClient();
        Destroy(this.gameObject);
    }

    public void Die()
    {
        this.StopBroadcast();
    }
}
