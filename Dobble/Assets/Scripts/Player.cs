using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class Player : NetworkBehaviour {
	
	public Deck deckPrefab;
	Deck deck;

	[SyncVar]
	public string name;

	[SyncVar]
	public int cardcount;

	public FileInfo NameFile;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			if (!isServer) {
				GameObject.FindWithTag("MasterDeck").SetActive(false);
			}
			(this.deck = (Deck)Instantiate (deckPrefab)).Constructor();
			this.deck.transform.SetParent (this.transform);	
			this.name = "LocalPlayer";
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
	
	[ClientRpc]
	public void RpcUpdate(int networkIdentity) {
		UpdatePlayerCard (networkIdentity);
	}
	
	void UpdatePlayerCard(int networkIdentity) {
		if (isLocalPlayer ) {
			if (this.netId.Value == networkIdentity) {
				this.deck.SetNextCard();
			}
		}
	}
	
	[Command]
	public void CmdUpdate(int[] card, int symbol, int networkIdentity) {
		Deck mdeck = GameObject.FindWithTag("MasterDeck").GetComponent<Deck>();
		if (mdeck.ContainsSymbol(symbol)) {
			mdeck.SetTopCard(card);
			UpdatePlayerCard(networkIdentity);
			RpcUpdate(networkIdentity);
		}
	}

	string LoadName(){
		if (NameFile.Exists){
			StreamReader r = File.OpenText(Application.persistentDataPath + "\\" + "NameSave.txt");
     		string info = r.ReadToEnd();
     		r.Close();
     		return info;
     	} else {
     		return GenName();
     	}
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer){
			cardcount = 10;
			this.deck.gameObject.SetActive(cardcount!=0);
			GameObject.Find("UsernameText").GetComponent<Text>().text = "Hello, "+name+"!";
			if (Input.GetMouseButtonDown(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100)) {
					int symbol = 0;
					if (int.TryParse(hit.transform.gameObject.name, out symbol)) {
						CmdUpdate(this.deck.GetTopCard(), symbol, (int) GetComponent<NetworkIdentity>().netId.Value);
					}
				}
			}
		}
	}
}
