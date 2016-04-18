using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameDiscovery : NetworkDiscovery {

    private MainMenu menu;

	// Use this for initialization
	public void Start () {
        this.menu = GameObject.FindObjectOfType(typeof(MainMenu)) as MainMenu;
        Debug.Assert(menu != null);
        this.Initialize();
        this.StartAsClient();
	}
	
	// Called everytime a host is found.
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        Debug.Log("I am not lone! :D");
        menu.SetHostIP(fromAddress);
    }

    // Starts broadcasting as a Server.
    public void StartHosting()
    {
        this.StopBroadcast();
        this.StartAsServer();
    }
}
