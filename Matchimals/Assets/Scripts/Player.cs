using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Timers;
using System.IO;/**/
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour {
	
	public Deck deckPrefab;
	public Card cardPrefab;
	Card card;
	Deck deck;

    // Lobby member prefab.
    public LobbyMember lobbyMemberPrefab;
	
	[SyncVar]
	public string name;
    [SyncVar]
    public string animalName;

	int[][] cardStack;
	
	[SyncVar]
	public int cardcount;

	Card[] bgStack;
	private AudioClip voiceSound;
	private AudioClip errorSound;

	private int selectSymbol;
	private bool correctSymbol = false;
	
	private const int TIME_PENALTY = 2;
	private const string ERROR_SOUND_PATH = "GameSounds/errorSound"; 
	private const int ANIMATION_TIME = 1;
	
	private bool isPenalized = false;
	private bool WaitingForAnimation = false;
	//Set the number of symbols per card legal options are 4,6,8,12 where 12 does not have enough symbols
	private const int symbolsPerCard = 4;

	void Start () {
		if (isLocalPlayer) {			

			if (isServer) {
				(this.deck = (Deck)Instantiate (deckPrefab)).Constructor(this.transform,symbolsPerCard);
			}
			int curAnimal = PlayerPrefs.GetInt("animal");
            CmdInitPlayer(this.netId.Value, LoadName(), Resources.LoadAll<Texture>("Animals")[curAnimal].name);

            this.voiceSound = Resources.Load<AudioClip>("AnimalSounds/"+animalName);
			this.errorSound = Resources.Load<AudioClip>(ERROR_SOUND_PATH);
		}
	}

    private void AddLobbyMember()
    {
        LobbyMember lobbyMember = (LobbyMember) Instantiate(lobbyMemberPrefab);
        lobbyMember.BindPlayer(this);
    }

    // Store and sync all network important player attributes.
    [Command]
    public void CmdInitPlayer(uint networkIdentity, string name, string animalName)
    {
        if (this.netId.Value == networkIdentity) {
            this.name = name;
            this.animalName = animalName;
            AddLobbyMember();
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
				Card thrown = Instantiate (this.card);
				thrown.GetComponent<Move> ().Initialize (thrown.GetComponent<Transform> ().transform.position + Vector3.back, thrown.GetComponent<Transform> ().transform.position + Vector3.up * 5.0f + Vector3.back, 1.0f);
				this.card.SetCard (cardStack[cardcount-1]);
                Debug.Log("CardCount:"+cardcount+" bgStackSize:"+bgStack.Length);
				Destroy(bgStack [cardcount - 1].gameObject);
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
		//StartCoroutine(AnimateWait(card, networkIdentity));
		correctSymbol = false;
		selectSymbol = symbol;
        Debug.Log("Symbol:"+symbol);
		if (deck.ContainsSymbol (symbol)) {
            Debug.Log("Right!");
            deck.SetTopCard(card);
            this.cardcount = Mathf.Max(0, cardcount - 1);
            if (cardcount > 0)
            {
                RpcUpdate(networkIdentity);
            }
            //correctSymbol = true;
        } else {
			RpcPenalty(networkIdentity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer && (SceneManager.GetActiveScene().name == "GameScene")) {
			if (!WaitingForAnimation){
				//card.gameObject.SetActive (cardcount != 0);
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
			} else {
				deck.Zoom(selectSymbol);
				this.card.Zoom(selectSymbol);
			}
		}
	}


	void drawBGStack (int cardsNr) {
		bgStack = new Card[cardsNr];
		for (int i = 0; i < cardsNr  ; i++) {
			(bgStack[i] = (Card)Instantiate (cardPrefab)).Constructor (symbolsPerCard);
			bgStack[i].transform.SetParent(this.transform);
			float z = (i + 1f)*0.5f;
			bgStack[i].transform.localPosition = Random.insideUnitCircle*0.2f;				
			bgStack[i].transform.localPosition += new Vector3(0, 0, z);
		}
	}

    /*
	IEnumerator AnimateWait(int[] card, uint networkIdentity) {
        WaitingForAnimation = true;
        yield return new WaitForSeconds(ANIMATION_TIME);
        this.card.ResetZoom(selectSymbol);
        deck.ResetZoom(selectSymbol);
        if (correctSymbol){
        	deck.SetTopCard (card);
			this.cardcount = Mathf.Max (0, cardcount - 1);
			if (cardcount > 0) {
				RpcUpdate (networkIdentity);
			}
		}
        WaitingForAnimation = false;
    }
    */
	public void PassCards (int[][] cardBlock, int symbolsPerCard, int cardsPerPlayer) {
        // Get the deck if server.
        if (isServer)
        {
            this.deck = GameObject.FindObjectOfType<Deck>();
            Debug.Assert(deck != null);
        }
        // Convert it to a single array.
        int[] cards = new int[cardsPerPlayer*symbolsPerCard];
        for (int i=0; i<cardsPerPlayer; i++)
        {
            for (int j=0; j<symbolsPerCard; j++)
            {
                cards[symbolsPerCard * i + j] = cardBlock[i][j];
            }
        }
		RpcPassAllCards (cards, symbolsPerCard, cardsPerPlayer, this.netId.Value);
	}
	
	[ClientRpc]
	public void RpcPassAllCards (int [] cards, int symbolsPerCard, int cardsPerPlayer, uint netId) {
		if (isLocalPlayer && netId == this.netId.Value) {
            (this.card = (Card)Instantiate(cardPrefab)).Constructor(symbolsPerCard);
            this.card.transform.SetParent(this.transform);
            this.cardcount = cardsPerPlayer;
            cardStack = new int[cardsPerPlayer][];
            for (int i = 0; i < cardsPerPlayer; i++)
            {
                cardStack[i] = new int[symbolsPerCard];
                for (int j = 0; j < symbolsPerCard; j++)
                {
                    cardStack[i][j] = cards[symbolsPerCard* i + j];
                }
            }
            Debug.Log("cardcount:" + cardcount);
			this.card.SetCard (cardStack [this.cardcount - 1]);
			drawBGStack (this.cardcount - 1);
		}
	}

    [ClientRpc]
    public void RpcGotoGame() {
        if (isLocalPlayer) {
            SceneManager.LoadScene("GameScene");
        }
    }

}
