using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

	public Card cardPrefab;
	public Server serverPrefab;
	[SyncVar]
	public string name;
	[SyncVar]
	public int cardcount;
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
	
	string GenName() {
		const int minLen = 2;
		const int maxLen = 12;
		const string consonants = "QWRTYPSDFGHJKLZXCVBNM";
		const string vowels		= "EUIA";
		string str = "";
		for (int i=0; i<Random.Range(minLen,maxLen); i++) {
			if (Random.Range(0,2) == 0) {
				str += consonants[Random.Range(0,consonants.Length)];
			} else {
				str += vowels[Random.Range(0,vowels.Length)];
			}
		}
		return str;
	}
	
	[Command]
	public void CmdInitialize(int networkIdentity) {
		Server s = (Server)GameObject.FindObjectOfType<Server> ();
		RpcUpdate (s.NextCard(), networkIdentity);
		this.name = GenName();
	}

	[Command]
	public void CmdUpdate(int[] card, int symbol, int networkIdentity) {
		Server s = (Server)GameObject.FindObjectOfType<Server> ();
		if (s.ContainsSymbol(symbol)) {
			s.setCard(card);
			RpcUpdate(s.NextCard(), networkIdentity);
			this.cardcount = Mathf.Max(0,cardcount-1);
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
		if (isLocalPlayer){
			card.gameObject.SetActive(cardcount!=0);
			GameObject.Find("UsernameText").GetComponent<Text>().text = "Hello, "+name+"!";
			if (Input.GetMouseButtonDown(0)) {
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
}
