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
		(this.card = (Card)Instantiate (cardPrefab)).Constructor (this.transform);
		if (isLocalPlayer) {

			if (isServer) {
				Debug.Log ("is server");
				this.server = (Server)Instantiate (serverPrefab);
				this.server.transform.SetParent (this.transform);
				//this.server.transform.localPosition = Vector3.right*5;
			}
		}
	}

	[Command]
	public void CmdUpdate(int symbol) {
		if (isServer) {
			Debug.Log ("cmdupdate called");
			//this.GetComponentInChildren<Server>().setCard (new int[] {0,0,0,0,0,0,0,0});
			GameObject.FindObjectOfType<Server> ().setCard (new int[] {0,0,0,0,0,0,0,0});
		}
	}

	[ClientRpc]
	public void RpcUpdate(int[] card) {
		Debug.Log ("rpc update started");
		Debug.Log (isClient);
		if (isClient) {
			this.card.SetCard (card);
			Debug.Log ("rpc update succesful");
		}
	}

	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			if (Input.GetMouseButtonDown(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100)) {
					int id = 0;
					if (int.TryParse(hit.transform.gameObject.name, out id)) {
						//this.card.SetCard (new int[] {0,3,2,4,6,7,8,2});
						Debug.Log("clicked:"+id);
					}
					CmdUpdate(id);
				}
			}
		}
	}
}
