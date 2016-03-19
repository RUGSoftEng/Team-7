using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Deck : MonoBehaviour {

	public Card cardPrefab;
	// from outside initialized variables
	public int symbolsPerCard;
	public Vector3 topcardloc;
	
	// cards array where each row represents a card
	int[][] cards;
	// the index of the current card
	int card_idx = 0;
	// reference to top card (currently visible) object of the deck
	Card topCard;
	
	// Number of cards in the game.
	public int totalCards;

	// Players in the game.
	Player[] players;

	private bool isGameOver = false;
	
	public void Constructor() {
		if (!IsLegalSymbolsPerCard (this.symbolsPerCard)) Debug.LogError ("Invalid symbols per card.");
		InitializeCards (this.symbolsPerCard);
		RandomizeArray (this.cards);
		(this.topCard = (Card)Instantiate (cardPrefab)).Constructor();
		this.topCard.transform.SetParent (this.transform);		
		this.topCard.transform.localPosition = topcardloc;
		this.SetNextCard ();
	}
	
	public void Update() {
		divideCards();
		int winner = checkWinner();
		if (!this.CompareTag("MasterDeck"))
			if (isGameOver) {
				GameObject.Find("WinningText").GetComponent<Text>().text = "WOW! "+players[winner].name+" WINS!";
				this.gameObject.SetActive(false);
			}  else {
				GameObject.Find("WinningText").GetComponent<Text>().text = "";
				this.gameObject.SetActive(true);
			}
	}
	
	// Devide the number of cards.
	private void divideCards() {
		Player[] foundPlayers = GameObject.FindObjectsOfType(typeof(Player)) as Player[];
		if (players == null || (players.Length != foundPlayers.Length)) {
			players = foundPlayers;
			int cardsPerPlayer = totalCards/players.Length;
			for (int i=0; i<players.Length; i++) {
				players[i].cardcount = cardsPerPlayer;
			}
		}
	}
	
	private int checkWinner() {
		if (players != null) {
			for (int i=0; i<players.Length; i++) {
				if (players[i].cardcount == 0) {
					this.isGameOver = true;
					return i;
				}
			}
		}
		this.isGameOver = false;
		return -1;
	}

	// symbols per card should be 0, 1, 2 or (prime + 1)
	bool IsLegalSymbolsPerCard(int num) {
		if (num < 0) return false;
		if (num >= 0 && num <= 4) return true;
		int prime = num - 1;
		if (prime % 2 == 0) return false;
		for (int i = 3; i * i <= prime; i += 2) if (prime % i == 0) return false;
		return true;
	}


	// initializes cards array
	void InitializeCards (int symbolsPerCard) {
		int prime = symbolsPerCard - 1;
		int numberOfCards = prime * prime + prime + 1;
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
		this.topCard.SetCard (card);
	}
	
	public int[] GetTopCard() {
		return this.topCard.GetCard ();
	}
}
