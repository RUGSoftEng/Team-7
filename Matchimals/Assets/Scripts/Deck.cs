using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Deck : MonoBehaviour {

	public Card cardPrefab;
	
	public Vector3 topcardloc;
	public int maxAmount;

	private int symbolsPerCard;

	// indicates the total number of cards according to the number of symbols per card
	int numberOfCards;

	// cards array where each row represents a card
	int[][] cards;
	// the index of the current card
	int card_idx = 0;
	// reference to top card (currently visible) object of the deck
	Card topCard;

	// Players in the game.
	Player[] players;

	private bool isGameOver = false;

	// New deck is created.
	public void Constructor(Transform parent, int symbolsPerCard) {
		this.symbolsPerCard = symbolsPerCard;
		this.transform.SetParent (parent);
		if (!IsLegalSymbolsPerCard ()) Debug.LogError ("Invalid symbols per card.");

		InitializeCards ();
		RandomizeArray (this.cards);

		(this.topCard = (Card)Instantiate (cardPrefab)).Constructor(symbolsPerCard);
		this.topCard.transform.SetParent (this.transform);
		this.topCard.SetCard (NextCard ());
		this.topCard.transform.localPosition = new Vector3 (100, 0, 0);
	}

    // When the game scene is loaded, this is triggered.
    public void OnLevelWasLoaded(int level) {
        if (SceneManager.GetActiveScene().name == "GameScene") {
            Debug.Log("DIVIDE!");
            divideCards();
        }
    }

    // Every frame, check if somebody won, divide cards for new players.
    public void Update() {
        if (SceneManager.GetActiveScene().name == "GameScene") {
            int winner = checkWinner();
            if (isGameOver)
            {
                GameObject.Find("WinningText").GetComponent<Text>().text = players[winner].name + " WINS!";
                topCard.gameObject.SetActive(false);
            }
            else
            {
                GameObject.Find("WinningText").GetComponent<Text>().text = "";
                topCard.gameObject.SetActive(true);
            }
        }
	}
	
	// Devide the cards among players.
	void divideCards() {
		Player[] players = GameObject.FindObjectsOfType(typeof(Player)) as Player[];
        Debug.Log("Playercount:"+players.Length);
		int cardsPerPlayer = maxAmount;
		while (cardsPerPlayer * players.Length > numberOfCards) {cardsPerPlayer--;}
		
		for (int i = 0; i < players.Length; i++) {				
			int [][] cardBlock = new int[cardsPerPlayer*2][] ;
			for (int j = 0; j < cardsPerPlayer; j++) {
				cardBlock[j] = cards[i*cardsPerPlayer + j];
			}
			players[i].PassCards(cardBlock);
		}
	}
	
	// Checks if somebody won, if so: it's gameover.
	int checkWinner() {
		if (players != null) {
			for (int i=0; i<players.Length; i++) {
				if (players[i].cardcount == 0) {
					this.isGameOver = true;
					return i;
				}
			}
		}
		return -1;
	}

	// symbols per card should be 0, 1, 2 or (prime + 1)
	bool IsLegalSymbolsPerCard() {
		
		int symbolsPerCard = this.symbolsPerCard;
		
		if (symbolsPerCard < 0) return false;
		if (symbolsPerCard >= 0 && symbolsPerCard <= 4) return true;
		int prime = symbolsPerCard - 1;
		if (prime % 2 == 0) return false;
		for (int i = 3; i * i <= prime; i += 2) if (prime % i == 0) return false;
		return true;
	}


	// initializes cards array
	void InitializeCards () {
		int symbolsPerCard = this.symbolsPerCard;
		int prime = symbolsPerCard - 1;
		numberOfCards = prime * prime + prime + 1;
		int[][] cards = new int[numberOfCards][];
		
		for (int i = 0; i < numberOfCards; ++i) cards [i] = new int[symbolsPerCard];
		int minFactor = 2, boundMinFactor = Convert.ToInt32(Math.Sqrt(prime));
		while (minFactor <= boundMinFactor && prime % minFactor != 0) ++minFactor;
		if (minFactor > boundMinFactor) minFactor = prime;
		int row = 0;
		for (int i = 0; i < prime; ++i) {
			for (int j = 0; j < prime; ++j) cards[row][j] = i * prime + j;
			cards[row][prime] = prime * prime;
			++row;
		}
		for (int i = 0; i < minFactor; ++i) {
			for (int j = 0; j < prime; ++j) {
				for (int k = 0; k < prime; ++k) cards[row][k] = k * prime + (j + i * k) % prime;
				cards[row][prime] = prime * prime + 1 + i;
				++row;
			}
		}
		for (int i = 0; i <= minFactor; ++i) cards[row][i] = prime * prime + i;
		this.cards = cards;
	}

	// Shuffles cards.
	static void RandomizeArray(int[][] array) {
		System.Random random = new System.Random ();
		for (int i = array.Length - 1; i > 0; --i) {
			int r = random.Next(0,i);
			int[] tmp = array[i];
			array[i] = array[r];
			array[r] = tmp;
		}
	}

	// increments symbol and returns the symbol
	int NextSymbol() {
		this.card_idx = ++this.card_idx % cards.Length;
		return this.card_idx;
	}

	public void Zoom(int symbol){
		topCard.Zoom(symbol);
	}

	public void ResetZoom(int symbol){
		topCard.ResetZoom(symbol);
	}
	
	public bool ContainsSymbol(int symbol) {
		return topCard.ContainsSymbol (symbol);
	}
	
	// returns the next card
	public int[] NextCard() {
		return this.cards [NextSymbol()];
	}
	
	public void SetNextCard() {
		this.SetTopCard (NextCard());
	}
	
	public void SetTopCard(int[] card) {
		Card copy = Instantiate (topCard);
		copy.GetComponent<Transform> ().transform.position += Vector3.forward;
		topCard.SetCard (card);
		topCard.GetComponent<Move> ().Initialize (topCard.GetComponent<Transform> ().transform.position + Vector3.down * 5.0f + Vector3.back, topCard.GetComponent<Transform> ().transform.position + Vector3.back, 1.0f);
		Destroy (copy, 1.0f);
	}
	
	public int[] GetTopCard() {
		return this.topCard.GetCard ();
	}
	
	public bool IsGameOver() {
		return this.isGameOver;
	}
}
