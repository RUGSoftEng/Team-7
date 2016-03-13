using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	public Card cardPrefab;
	public Server serverPrefab;

	Card card;
	Server server;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			if (isServer) {
				(this.server = (Server)Instantiate (serverPrefab)).Constructor(this.transform);
			}
			(this.card = (Card)Instantiate (cardPrefab)).Constructor ();
			this.card.transform.SetParent(this.transform);
			CmdInitialize((int) GetComponent<NetworkIdentity>().netId.Value);
		}
	}

	[Command]
	public void CmdInitialize(int networkIdentity) {
		Server s = (Server)GameObject.FindObjectOfType<Server> ();
		RpcUpdate (s.NextCard(), networkIdentity);
	}

	[Command]
	public void CmdUpdate(int[] card, int symbol, int networkIdentity) {
		Server s = (Server)GameObject.FindObjectOfType<Server> ();
		if (s.ContainsSymbol(symbol)) {
			s.setCard(card);
			RpcUpdate(s.NextCard(), networkIdentity);
		}
	}

	[ClientRpc]
	public void RpcUpdate(int[] card, int networkIdentity) {
		if (isLocalPlayer ) {
			if (GetComponent<NetworkIdentity>().netId.Value == networkIdentity) {
				this.card.SetCard (card);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (isLocalPlayer && Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100)) {
				int symbol = 0;
				if (int.TryParse(hit.transform.gameObject.name, out symbol)) {
					CmdUpdate(this.card.GetCard(), symbol, (int) GetComponent<NetworkIdentity>().netId.Value);
				}
			}
		}
	}
}
