using UnityEngine;
using System.Collections;
using System;

// circle packing: http://hydra.nat.uni-magdeburg.de/packing/cci/cci.html

public class Card : MonoBehaviour {

	public Symbol symbolPrefab;

	// Array of contained symbols
	private Symbol[] containedSymbols; 
	
	// Creates a new card gameobject with symbols placed on it.
	public void Constructor() {
		// Noted as strings, as these will eventually be read from a file.
		string[] lines = {"0.302593388349",
						  "-0.302593388348611302909204224933 -0.628341645367213738512227388956",
						  "0.302593388348611302909204224933 -0.628341645367213738512227388956",
						  "-0.679921171839088240043878874469 -0.155187570571975671990838057814",
						  "0.679921171839088240043878874469 -0.155187570571975671990838057814",
						  "0.000000000000000000000000000000 0.000000000000000000000000000000",
						  "-0.545254445070410775447749861103 0.434825910113495061957667559237",
						  "0.545254445070410775447749861103 0.434825910113495061957667559237",
						  "0.000000000000000000000000000000 0.697406611651388697090795775067"};
		float radius = float.Parse (lines [0]);
		Vector2[] coordinates = new Vector2[8];
		string[] line;
		for (int i = 1; i < 8+1; ++i) {
			line = lines[i].Split(' ');
			coordinates[i-1] = new Vector2(float.Parse(line[0]), float.Parse(line[1]));
		}
		
		Sprite[] sprites = Resources.LoadAll<Sprite>("Symbols");
		float[] scales = new float[sprites.Length];
		for (int i = 0; i < sprites.Length; ++i) {
			Vector3 size = sprites[i].bounds.size;
			scales[i] = radius*2/Mathf.Sqrt(size.x*size.x+size.y*size.y);
		}
		
		this.containedSymbols = new Symbol[8];
		for (int i = 0; i < 8; ++i) {
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

	// Translates this card into its abstract representative.
	public int[] GetCard() {
		int[] c = new int[8];
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
