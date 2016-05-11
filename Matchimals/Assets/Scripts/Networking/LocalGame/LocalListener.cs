using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.SceneManagement;

public class LocalListener : MonoBehaviour {
    private static float LISTEN_INTERVAL = 0.1f;

    public int port = 5000;
    public bool listen = false;
    private UdpClient udp;
    private MainMenu menu;

    public void StartListening() {
        this.menu = GameObject.FindObjectOfType<MainMenu>();
        Debug.Assert(menu != null);
        this.udp = new UdpClient(port);
        this.listen = true;
        StartCoroutine("Listen");
    }

    public void StopListening()
    {
        udp.Close();
        this.listen = false;
    }

    public bool IsListening() {
        return listen;
    }

    private IEnumerator Listen() {
        while (IsListening()) {
            if (SceneManager.GetActiveScene().name == "MainMenuScene") {
                this.udp.BeginReceive(PacketHandler, null);
            }
            yield return new WaitForSeconds(LISTEN_INTERVAL);
        }
        yield return null;
    }

    private void PacketHandler(IAsyncResult ar) {
        IPEndPoint ip = new IPEndPoint(IPAddress.Any, port);
        byte[] data = udp.EndReceive(ar, ref ip);
        string message = Encoding.ASCII.GetString(data);
        string hostIP = ip.Address.ToString();
        if (message.Equals("Matchimals") && !hostIP.Equals(GetMyIP())) {
            menu.hostIP = ip.Address.ToString();
        }
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
