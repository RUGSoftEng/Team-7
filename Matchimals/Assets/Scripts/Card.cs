using UnityEngine;
using System.Collections;
using System;

// circle packing: http://hydra.nat.uni-magdeburg.de/packing/cci/cci.html

public class Card : MonoBehaviour {

	public Symbol symbolPrefab;

	// Array of contained symbols
	private Symbol[] containedSymbols; 

	private string[] lines;

	private int symbolsPerCard;
	
	// Creates a new card gameobject with symbols placed on it.
	public void Constructor(int symbolsPerCard) {
		this.symbolsPerCard = symbolsPerCard;
		//checks if symbols per card are legal
		Debug.Assert(symbolsPerCard==8||symbolsPerCard==6||symbolsPerCard==12||symbolsPerCard==4);
		TextAsset txt = (TextAsset)Resources.Load("Circle packings\\"+symbolsPerCard.ToString(), typeof(TextAsset)) as TextAsset;
		string content = txt.text;
		string[] lines = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
		float radius = float.Parse (lines [0]);
		Vector2[] coordinates = new Vector2[symbolsPerCard];
		string[] line = new string[2];
		for (int i = 1; i < symbolsPerCard+1; ++i) {
			line = lines[i].Split(' ');
			coordinates[i-1] = new Vector2(float.Parse(line[0]), float.Parse(line[1]));
		}
		
		Sprite[] sprites = Resources.LoadAll<Sprite>("Symbols");
		float[] scales = new float[sprites.Length];
		for (int i = 0; i < sprites.Length; ++i) {
			Vector3 size = sprites[i].bounds.size;
			scales[i] = radius*2/Mathf.Sqrt(size.x*size.x+size.y*size.y);
		}
		
		this.containedSymbols = new Symbol[symbolsPerCard];
		for (int i = 0; i < symbolsPerCard; ++i) {
			(this.containedSymbols [i] = (Symbol)Instantiate (symbolPrefab))
										.Constructor (this.transform, coordinates [i], sprites, scales);
			this.containedSymbols[i].transform.SetParent(this.transform);		
		}
	}

	// true if this card contains the symbol
	public bool ContainsSymbol(int symbol) {
		foreach (Symbol s in this.containedSymbols) if (s.getSymbol() == symbol) return true;
		return false;
	}

	//Makes the given symbol zoom in and out
	public void Zoom(int symbol){
		foreach (Symbol s in this.containedSymbols) if (s.getSymbol() == symbol) {
			s.ZoomIn(symbol);
		}
	}

	//Resets the symbol zoom properties
	public void ResetZoom(int symbol){
		foreach (Symbol s in this.containedSymbols) if (s.getSymbol() == symbol) {
			s.ResetZoom(symbol);
		}
	}

	// Translates this card into its abstract representative.
	public int[] ToCardArray() {
		int[] c = new int[symbolsPerCard];
		int i = 0;
		foreach (Symbol s in this.containedSymbols) {
			c[i] = s.getSymbol();
			++i;
		}
		return c;
	}

	// sets card and a random rotation
	public void SetCard(int[] card) {
		int symbol = 0;
		foreach (Symbol s in this.containedSymbols) {
			s.SetSymbol(card[symbol]);
			++symbol;
		}

		// apply a random rotation to the card
		this.transform.eulerAngles = Vector3.forward*UnityEngine.Random.value*360;

	}

}
