using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class Player : NetworkBehaviour {
	
	public Deck deckPrefab;
	public Card cardPrefab;
	Card card;
	Deck deck;

	[SyncVar]
	public string name;

	[SyncVar]
	public int cardcount;

	public FileInfo NameFile;
	private bool initialized = false;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {			
			(this.card = (Card)Instantiate (cardPrefab)).Constructor ();
			this.card.transform.SetParent(this.transform);
			if (isServer) {
				(this.deck = (Deck)Instantiate (deckPrefab)).Constructor(this.transform);
			} 
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
	
	[Command]
	public void CmdInitialize(uint networkIdentity) {
		Deck deck = (Deck)GameObject.FindObjectOfType<Deck> ();
		RpcUpdate (deck.NextCard(), networkIdentity);
		this.name = GenName();
	}

	void UpdatePlayerCard (int[] card, uint networkIdentity) {
		if (isLocalPlayer ) {
			if (this.netId.Value == networkIdentity) {
				this.card.SetCard (card);
			}
		}
	}

	[ClientRpc]
	public void RpcUpdate(int[] card, uint networkIdentity) {
		UpdatePlayerCard (card, networkIdentity);
	}

	[Command]
	public void CmdUpdate(int[] card, int symbol, uint networkIdentity) {
		Deck deck = (Deck)GameObject.FindObjectOfType<Deck> ();
		if (deck.ContainsSymbol(symbol)) {
			deck.SetTopCard(card);
			UpdatePlayerCard(deck.NextCard(), this.netId.Value);
			RpcUpdate(deck.NextCard(), networkIdentity);
			this.cardcount = Mathf.Max(0,cardcount-1);
		}
	}

	// Update is called once per frame
	void Update () {
		if (isLocalPlayer){
			if (!initialized) {
				if (isServer) {
					UpdatePlayerCard(deck.NextCard(), this.netId.Value);
				}
				CmdInitialize(this.netId.Value);
				initialized = true;
			}
			card.gameObject.SetActive(cardcount!=0);
			GameObject.Find("UsernameText").GetComponent<Text>().text = "Hello, "+name+"!";
			if (Input.GetMouseButtonDown(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100)) {
					int symbol = 0;
					if (int.TryParse(hit.transform.gameObject.name, out symbol)) {
						CmdUpdate(this.card.GetCard(), symbol, this.netId.Value);
					}
				}
			}
		}
	}
}
