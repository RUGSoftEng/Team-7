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
	
	[SyncVar]
	public int cardcount;
	
	private AudioClip voiceSound;
	private AudioClip errorSound;
	private bool initialized = false;
	
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
	
	[Command]
	public void CmdInitialize(uint networkIdentity, string name) {
		Deck deck = (Deck)GameObject.FindObjectOfType<Deck> ();
		RpcUpdate (deck.NextCard(), networkIdentity);
		this.name = name;
	}
	
	void UpdatePlayerCard (int[] card, uint networkIdentity) {
		if (isLocalPlayer ) {
			if (this.netId.Value == networkIdentity) {
				AudioSource.PlayClipAtPoint(voiceSound, new Vector3(0,0,0));
				this.card.SetCard (card);
			}
		}
	}
	
	[ClientRpc]
	public void RpcUpdate(int[] card, uint networkIdentity) {
		UpdatePlayerCard (card, networkIdentity);
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
		Deck deck = (Deck)GameObject.FindObjectOfType<Deck> ();
		if (deck.ContainsSymbol (symbol)) {
			deck.SetTopCard (card);
			UpdatePlayerCard (deck.NextCard (), this.netId.Value);
			RpcUpdate (deck.NextCard (), networkIdentity);
			this.cardcount = Mathf.Max (0, cardcount - 1);
		} else {
			RpcPenalty(networkIdentity);
		}
	}
	
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer){
			if (!initialized) {
				if (isServer) {
					UpdatePlayerCard(deck.NextCard(), this.netId.Value);
				}
				CmdInitialize(this.netId.Value, LoadName());
				initialized = true;
			}
			card.gameObject.SetActive(cardcount!=0);
			GameObject.Find("UsernameText").GetComponent<Text>().text = name;
			if (Input.GetMouseButtonDown(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100)) {
					int symbol = 0;
					if (int.TryParse(hit.transform.gameObject.name, out symbol) &&
					    !this.isPenalized) {
						CmdUpdate(this.card.GetCard(), symbol, this.netId.Value);
					}
				}
			}
		}
	}
}
