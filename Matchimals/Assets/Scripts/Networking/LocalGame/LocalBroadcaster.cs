using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.SceneManagement;

public class LocalBroadcaster : MonoBehaviour {
    public string message = "Matchimals";
    public int port = 5000;
    public float broadcastInterval = 0.1f;
    public bool broadcast = false;

    public void Start() {
        InvokeRepeating("Broadcast", 0, broadcastInterval);
    }

    public void SetBroadcasting(bool broadcast) {
        this.broadcast = broadcast;
    }

    public bool IsBroadcasting() {
        return broadcast;
    }

    private void Broadcast() {
        if (IsBroadcasting() && SceneManager.GetActiveScene().name == "MainMenuScene") {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, port);
            byte[] data = Encoding.ASCII.GetBytes(message);
            client.Send(data, data.Length, ip);
            client.Close();
        }
	}
}
