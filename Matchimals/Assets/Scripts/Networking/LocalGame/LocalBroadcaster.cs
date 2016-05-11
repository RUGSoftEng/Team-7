using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.SceneManagement;

public class LocalBroadcaster : MonoBehaviour {
    private static float BROADCAST_INTERVAL = 0.1f;

    public string message = "Matchimals";
    public int port = 5000;
    public bool broadcast = false;

    public void Start() {
        InvokeRepeating("Broadcast", 0, BROADCAST_INTERVAL);
    }

    // When the game scene is loaded, this is triggered.
    public void OnLevelWasLoaded(int level) {
        if (SceneManager.GetActiveScene().name != "MainMenuScene") {
            SetBroadcasting(false);
        }
    }

    public void SetBroadcasting(bool broadcast) {
        this.broadcast = broadcast;
    }

    public bool IsBroadcasting() {
        return broadcast;
    }

    private void Broadcast() {
        if (IsBroadcasting()) {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, port);
            byte[] data = Encoding.ASCII.GetBytes(message);
            client.Send(data, data.Length, ip);
            client.Close();
        }
	}
}
