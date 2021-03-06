using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Timers;
using System.IO;/**/


public class Player : NetworkBehaviour {
	
	public Deck deckPrefab;
	public Card cardPrefab;
	Card card;
	Deck deck;
	
	[SyncVar]
	public string name;

	int[][] cardStack;
	
	[SyncVar]
	int cardcount;
	public int cardCount {
		get {return cardcount;}
		set {cardcount = value;}
	}
	
	Card[] bgStack;
	private AudioClip voiceSound;
	private AudioClip errorSound;
	
	private const int TIME_PENALTY = 2;
	private const string ERROR_SOUND_PATH = "GameSounds/errorSound"; 
	
	private bool isPenalized = false;
	
	void Start () {
		if (isLocalPlayer) {			
			(this.card = (Card)Instantiate (cardPrefab)).Constructor ();
			this.card.transform.SetParent(this.transform);
			
			if (isServer) {
				(this.deck = (Deck)Instantiate (deckPrefab)).Constructor(this.transform);
			}
			int curAnimal = PlayerPrefs.GetInt("animal");
			string animalName = Resources.LoadAll<Texture>("Animals")[curAnimal].name;
			this.voiceSound = Resources.Load<AudioClip>("AnimalSounds/"+animalName);
			this.errorSound = Resources.Load<AudioClip>(ERROR_SOUND_PATH);
		}
	}
	
	string LoadName(){
		return PlayerPrefs.GetString("name");
	}
	
	[ClientRpc]
	public void RpcUpdate(uint networkIdentity) {
		if (isLocalPlayer ) {
			if (this.netId.Value == networkIdentity) {
				AudioSource.PlayClipAtPoint(voiceSound, new Vector3(0,0,0));
				this.card.SetCard (cardStack[cardcount-1]);
			} 
		}
	}
	
	[ClientRpc]
	public void RpcPenalty(uint networkIdentity) {
		if (isLocalPlayer) {
			if (this.netId.Value == networkIdentity) {
				AudioSource.PlayClipAtPoint (errorSound, new Vector3 (0, 0, 0));
				StartCoroutine (StartCountDownPenalty ());
			}
		}
	}
	
	IEnumerator StartCountDownPenalty()
	{
		this.isPenalized = true;
		yield return new WaitForSeconds(TIME_PENALTY);
		this.isPenalized = false;
	}
	
	[Command]
	public void CmdUpdate(int[] card, int symbol, uint networkIdentity) {
		if (deck.ContainsSymbol (symbol)) {
			deck.SetTopCard (card);
			this.cardcount = Mathf.Max (0, cardcount - 1);
			if (cardcount > 0) {
				RpcUpdate (networkIdentity);
			}
		} else {
			RpcPenalty(networkIdentity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			card.gameObject.SetActive (cardcount != 0);
			GameObject.Find ("UsernameText").GetComponent<Text> ().text = name;
			if (Input.GetMouseButtonDown (0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, 100)) {
					int symbol = 0;
					if (int.TryParse (hit.transform.gameObject.name, out symbol) &&	!this.isPenalized) {
						CmdUpdate (this.card.GetCard (), symbol, this.netId.Value);
					}
				}
			}
		}
	}
	
	void drawBGStack (int cardsNr) {
		bgStack = new Card[cardsNr];
		for (int i = 0; i < cardsNr  ; i++) {
			(bgStack[i] = (Card)Instantiate (cardPrefab)).Constructor ();
			bgStack[i].transform.SetParent(this.transform);
			float z = (i + 1f)*0.5f;
			bgStack[i].transform.localPosition = Random.insideUnitCircle*0.2f;				
			bgStack[i].transform.localPosition += new Vector3(0, 0, z);
		}
	}

	public void PassCards (int[][] cardBlock) {
		RpcPassAllCards (cardBlock, this.netId.Value);
	}

	[ClientRpc]
	public void RpcPassAllCards (int [][] cardBlock, uint netId) {
		if (isLocalPlayer && netId == this.netId.Value) {	
			this.cardStack = cardBlock;
			this.card.SetCard (cardStack [this.cardCount - 1]);
		}
	}

}
