using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Server : MonoBehaviour {

	public Card cardPrefab;

	int p = 7;
	int[][] cards;
	int index = 0;
	Card c;
	
	void Start () {
		(c = (Card) Instantiate (cardPrefab)).transform.SetParent(this.transform);
		InitializeCards ();
	}

	void InitializeCards () {
		int picturesPerCard = p + 1, numberOfCards = p * p + p + 1;
		this.cards = new int[numberOfCards][];
		for (int i = 0; i < numberOfCards; ++i) this.cards [i] = new int[picturesPerCard];
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

	public int nextIndex() {
		this.index = (this.index + 1) % cards.Length;
		return this.index;
	}
	
	void Update () {
		c.setCard (cards [nextIndex ()]);
		this.transform.Rotate(new Vector3(0, 0, UnityEngine.Random.value));
	}
}
