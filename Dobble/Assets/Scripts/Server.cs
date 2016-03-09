using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Server : MonoBehaviour {

	public Card cardPrefab;
	// should be 0, 1, 2 or (prime + 1)
	public int symbolsPerCard;

	int[][] cards;
	int index = 0;
	Card card;
	
	void Start () {
		if (!IsLegalSymbolsPerCard ()) Debug.LogError ("Invalid symbols per card.");
		InitializeCards ();
		(this.card = (Card)Instantiate (cardPrefab)).Constructor(this.transform);
	}

	private bool IsLegalSymbolsPerCard() {
		int symbolsPerCard = this.symbolsPerCard;
		if (symbolsPerCard < 0) return false;
		if (symbolsPerCard >= 0 && symbolsPerCard <= 4) return true;
		int prime = symbolsPerCard - 1;
		if (prime % 2 == 0) return false;
		for (int i = 3; i * i <= prime; i += 2) if (prime % i == 0) return false;
		return true;
	}

	private void InitializeCards () {
		int symbolsPerCard = this.symbolsPerCard;
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

	int NextIndex() {
		this.index = ++this.index % cards.Length;
		return this.index;
	}

	public int[] NextCard() {
		return this.cards [NextIndex ()];
	}
	
	void Update () {
		card.SetCard (cards [NextIndex ()]);
	}
}
