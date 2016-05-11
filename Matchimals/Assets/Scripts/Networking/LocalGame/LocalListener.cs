using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.SceneManagement;

public class LocalListener : MonoBehaviour {
    public int port = 5000;
    public bool listen = false;
    private UdpClient udp = null;

    // When the game scene is loaded, this is triggered.
    public void OnLevelWasLoaded(int level) {
        if (udp == null) this.udp = new UdpClient(port);
        this.listen = true;
        if (IsListening() && SceneManager.GetActiveScene().name == "MainMenuScene") {
            Debug.Log("LUISTER!");
            Listen();
        }
    }

    public void StopListening()
    {
        udp.Close();
        this.listen = false;
    }

    public bool IsListening() {
        return listen;
    }

    private void Listen() {
        if (IsListening()) {
            this.udp.BeginReceive(PacketHandler, null);
        }
    }

    private void PacketHandler(IAsyncResult ar) {
        IPEndPoint ip = new IPEndPoint(IPAddress.Any, port);
        byte[] data = udp.EndReceive(ar, ref ip);
        string message = Encoding.ASCII.GetString(data);
        string hostIP = ip.Address.ToString();
        if (message.Equals("Matchimals") && !hostIP.Equals(GetMyIP())) {
            MainMenu menu = GameObject.FindObjectOfType<MainMenu>();
            Debug.Assert(menu != null);
            menu.hostIP = ip.Address.ToString();
        }
        Listen();
    }

    private string GetMyIP() {
        IPHostEntry host;
        string localIP = "?";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList) {
            if (ip.AddressFamily.ToString() == "InterNetwork") {
                localIP = ip.ToString();
            }
        }
        return localIP;
    }
}
