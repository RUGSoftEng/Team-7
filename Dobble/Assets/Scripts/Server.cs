using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Server : MonoBehaviour {

	int[,] cards;

	public Card card;
	private Card c;

	// Use this for initialization
	void Start () {
		this.c = (Card) Instantiate (card);
		InitializeCards ();
	}

	void InitializeCards () {
		// constraint; prime
		int p = 1;
		
		int picturesPerCard = p + 1, numberOfCards = p * p + p + 1;
		this.cards = new int[numberOfCards, picturesPerCard];
		int minFactor = 2, boundMinFactor = Convert.ToInt32(Math.Sqrt(p));
		while (minFactor <= boundMinFactor && p % minFactor != 0) ++minFactor;
		if (minFactor > boundMinFactor) minFactor = p;
		int row = 0;
		for (int i = 0; i < p; ++i) {
			for (int j = 0; j < p; ++j) this.cards[row, j] = i * p + j;
			this.cards[row, p] = p * p;
			++row;
		}
		for (int i = 0; i < minFactor; ++i) {
			for (int j = 0; j < p; ++j) {
				for (int k = 0; k < p; ++k) this.cards[row, k] = k * p + (j + i * k) % p;
				this.cards[row, p] = p * p + 1 + i;
				++row;
			}
		}
		for (int i = 0; i <= minFactor; ++i) this.cards[row, i] = p * p + i;
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 2; ++i) {
			c.setSymbol(i,cards[0,i]);
		}

	}
}
