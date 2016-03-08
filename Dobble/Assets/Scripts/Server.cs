using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Server : MonoBehaviour {

	public Card cardPrefab;
	// should be 0, 1, 2 or (prime + 1)
	public int symbolsPerCard;

	int numberOfCards;
	int[][] cards;
	int index = 0;
	Card card;
	
	void Start () {
		this.numberOfCards = this.symbolsPerCard * (this.symbolsPerCard - 1) + 1;
		this.card = (Card)Instantiate (cardPrefab);
		this.card.transform.SetParent(this.transform);
		InitializeCards ();
	}

	void InitializeCards () {
		this.cards = new int[this.numberOfCards][];
		if (this.symbolsPerCard < 1) return;
		int p = this.symbolsPerCard - 1;
		for (int i = 0; i < this.numberOfCards; ++i) this.cards [i] = new int[this.symbolsPerCard];
		int minFactor = 2, boundMinFactor = Convert.ToInt32(Math.Sqrt(p));
		while (minFactor <= boundMinFactor && p % minFactor != 0) ++minFactor;
		if (minFactor > boundMinFactor) minFactor = p;
		int row = 0;
		for (int i = 0; i < p; ++i) {
			for (int j = 0; j < p; ++j) this.cards[row][j] = i * p + j;
			this.cards[row][p] = p * p;
			++row;
		}
		for (int i = 0; i < minFactor; ++i) {
			for (int j = 0; j < p; ++j) {
				for (int k = 0; k < p; ++k) this.cards[row][k] = k * p + (j + i * k) % p;
				this.cards[row][p] = p * p + 1 + i;
				++row;
			}
		}
		for (int i = 0; i <= minFactor; ++i) this.cards[row][i] = p * p + i;
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
