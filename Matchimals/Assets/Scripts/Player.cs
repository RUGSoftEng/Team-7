using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Timers;
using System.IO;/**/
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour {
    // Game constants.
    private const int TIME_PENALTY = 2;
    private const string ERROR_SOUND_PATH = "GameSounds/errorSound";
    private const int ANIMATION_TIME = 1;

    // Prefabs to use.
    public Deck deckPrefab;
	public Card cardPrefab;
    public LobbyMember lobbyMemberPrefab;

    //Set the number of symbols per card legal options are 4,6,8,12 where 12 does not have enough symbols
    public int symbolsPerCard = 4;
    // The number of cards of the player.
    public int cardcount;

    // Player data that should be synced and shared.
    [SyncVar]
	public string playerName;
    [SyncVar]
    public string animalName;

    // Player specific data, used locally.
    private Deck deck;
    private Card[] stack;
	private AudioClip errorSound;
	private bool isPenalized = false;

    // When the player is created in the lobby.
	void Start () {
		if (isLocalPlayer) {			

			if (isServer) {
				(this.deck = (Deck)Instantiate (deckPrefab)).Constructor(this.transform,symbolsPerCard);
			}
			int curAnimal = PlayerPrefs.GetInt("animal");
            CmdInitPlayer(this.netId.Value, LoadName(), Resources.LoadAll<Texture>("Animals")[curAnimal].name);

            this.errorSound = Resources.Load<AudioClip>(ERROR_SOUND_PATH);
        }
    }

    // Adds a visual lobby member for the lobby.
    private void AddLobbyMember() {
        LobbyMember lobbyMember = (LobbyMember) Instantiate(lobbyMemberPrefab, new Vector3(100,0,0), new Quaternion());
        lobbyMember.BindPlayer(this);
    }

    // Store and sync all network important player attributes.
    [Command]
    public void CmdInitPlayer(uint networkIdentity, string playerName, string animalName) {
        if (this.netId.Value == networkIdentity) {
            this.playerName = playerName;
            this.animalName = animalName;
            AddLobbyMember();
        }
    }
	
	string LoadName(){
		return PlayerPrefs.GetString("name");
	}

    // Returns the current card on top of the stack.
    private Card GetTopCard() {
        return stack[cardcount - 1];
    }
	
    // Move the top card off of the stack.
	[ClientRpc]
	public void RpcUpdate(uint networkIdentity) {
		if (isLocalPlayer ) {
			if (this.netId.Value == networkIdentity && this.cardcount > 0) {
				Card thrown = GetTopCard();
				thrown.GetComponent<Move> ().Initialize (thrown.GetComponent<Transform> ().transform.position + Vector3.back, thrown.GetComponent<Transform> ().transform.position + Vector3.up * 5.0f + Vector3.back, 1.0f);
                this.cardcount = Mathf.Max(0, cardcount - 1);
			} 
		}
	}
		
	[ClientRpc]
	public void RpcPenalty(uint networkIdentity) {
		if (isLocalPlayer) {
			if (this.netId.Value == networkIdentity) {
				AudioSource.PlayClipAtPoint (errorSound, new Vector3 (0, 0, -10));
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
	
    // Is called on the server when a player presses a symbol.
	[Command]
	public void CmdCheckMatch(int[] card, int symbol, uint networkIdentity) {
		if (deck.ContainsSymbol (symbol)) {
            Player matchingPlayer = FindPlayer(networkIdentity);
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("AnimalSounds/" + matchingPlayer.animalName), new Vector3(0, 0, -10),30f);
            deck.SetTopCard(card);
            RpcUpdate(networkIdentity);
            if (!matchingPlayer.isLocalPlayer) {
                // Make the cardcount of non-server players match our server side.
                matchingPlayer.cardcount = Mathf.Max(0, matchingPlayer.cardcount - 1);
            }
        } else {
			RpcPenalty(networkIdentity);
		}
	}

    // Returns the player with a matching networkIdentity.
    private Player FindPlayer(uint networkIdentity)
    {
        foreach (Player p in GameObject.FindObjectsOfType<Player>()) {
            if (p.netId.Value == networkIdentity) { return p; }
        }
        return null;
    }
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer && (SceneManager.GetActiveScene().name == "GameScene")) {
			GameObject.Find ("UsernameText").GetComponent<Text> ().text = playerName;
			if (Input.GetMouseButtonDown (0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, 100)) {
					int symbol = 0;
					if (int.TryParse (hit.transform.gameObject.name, out symbol) &&	!this.isPenalized) {
						CmdCheckMatch(GetTopCard().ToCardArray(), symbol, this.netId.Value);
                        hit.transform.gameObject.GetComponent<ZoomInOut>().Initialize(2, 1);
					}
				}
			}
		}
	}

    // Creates the player stack of cards.
	private void LoadStack (int[][] cardStack) {
        float offset = 0f;
	    stack = new Card[cardcount];
		for (int i = 0; i < cardcount  ; i++) {
			(stack[i] = (Card)Instantiate (cardPrefab)).Constructor (symbolsPerCard);
			stack[i].transform.SetParent(this.transform);
			float z = offset+(i + 1f)*0.5f;
			stack[i].transform.localPosition = Random.insideUnitCircle*0.2f;				
			stack[i].transform.localPosition -= new Vector3(0, 0, z);
            stack[i].SetCard(cardStack[i]);
		}
	}

    // Used to pass cards to the player.
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
        // Pass it to the client side.
		RpcPassAllCards (cards, symbolsPerCard, cardsPerPlayer, this.netId.Value);
	}
	
	[ClientRpc]
	public void RpcPassAllCards (int [] cards, int symbolsPerCard, int cardsPerPlayer, uint netId) {
		if (isLocalPlayer && netId == this.netId.Value) {
            this.cardcount = cardsPerPlayer;
            int[][] cardStack = new int[cardsPerPlayer][];
            for (int i = 0; i < cardsPerPlayer; i++)
            {
                cardStack[i] = new int[symbolsPerCard];
                for (int j = 0; j < symbolsPerCard; j++)
                {
                    cardStack[i][j] = cards[symbolsPerCard* i + j];
                }
            }
            LoadStack(cardStack);
		}
	}

    [ClientRpc]
    public void RpcGotoGame() {
        if (isLocalPlayer) {
            SceneManager.LoadScene("GameScene");
        }
    }

}
