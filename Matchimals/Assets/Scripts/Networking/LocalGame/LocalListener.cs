using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class LocalListener : MonoBehaviour {
    public int port = 5000;
    public bool listen = false;
    private UdpClient udp;
    private MainMenu menu;

    public void StartListening() {
        this.menu = GameObject.FindObjectOfType<MainMenu>();
        Debug.Assert(menu != null);
        this.udp = new UdpClient(port);
        this.listen = true;
        Listen();
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
        if (message.Equals("Matchimals")) {
            menu.hostIP = ip.Address.ToString();
        } else
        {
            Debug.Log("Bitch");
        }
        Listen();
    }
}
