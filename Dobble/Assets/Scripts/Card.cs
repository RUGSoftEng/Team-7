using UnityEngine;
using System.Collections;
using System;

// circle packing: http://hydra.nat.uni-magdeburg.de/packing/cci/cci.html

public class Card : MonoBehaviour {

	public Symbol symbolPrefab;

	// array of contained symbols
	private Symbol[] symbols; 

	// constructor
	public void Constructor(Transform parent) {
		this.transform.SetParent (parent);

		// hardcoded coordinates (in a unit circle) and radius for 8 symbols per picture
		float radius = 0.302593388348611302909204224934f;
		Vector2[] coordinates = new Vector2[8] {
			new Vector2(-0.302593388348611302909204224933f, -0.628341645367213738512227388956f),
			new Vector2(0.302593388348611302909204224933f, -0.628341645367213738512227388956f),
			new Vector2(-0.679921171839088240043878874469f, -0.155187570571975671990838057814f),
			new Vector2(0.679921171839088240043878874469f, -0.155187570571975671990838057814f),
			new Vector2(0.000000000000000000000000000000f, 0.000000000000000000000000000000f),
			new Vector2(-0.545254445070410775447749861103f, 0.434825910113495061957667559237f),
			new Vector2(0.545254445070410775447749861103f, 0.434825910113495061957667559237f),
			new Vector2(0.000000000000000000000000000000f, 0.697406611651388697090795775067f)
		}; 
		
		Sprite[] sprites = Resources.LoadAll<Sprite>("Symbols");
		float[] scales = new float[sprites.Length];
		for (int i = 0; i < sprites.Length; ++i) {
			Vector3 size = sprites[i].bounds.size;
			scales[i] = radius*2/Mathf.Sqrt(size.x*size.x+size.y*size.y);
		}
		
		this.symbols = new Symbol[8];
		for (int i = 0; i < 8; ++i) (this.symbols[i] = (Symbol) Instantiate (symbolPrefab)).Constructor(this.transform, coordinates[i], sprites, scales);

		// initialize
		symbols [0].SetSymbol (0);
		symbols [1].SetSymbol (1);
		symbols [2].SetSymbol (2);
		symbols [3].SetSymbol (3);
		symbols [4].SetSymbol (4);
		symbols [5].SetSymbol (5);
		symbols [6].SetSymbol (6);
		symbols [7].SetSymbol (49);

	}

	// true if this card contains the symbol
	public bool ContainsSymbol(int symbol) {
		foreach (Symbol s in this.symbols) if (s.getSymbol() == symbol) return true;
		return false;
	}

	public int[] GetCard() {
		int[] c = new int[8];
		int i = 0;
		foreach (Symbol s in this.symbols) {
			c[i] = s.getSymbol();
			++i;
		}
		return c;
	}

	// sets card and a random rotation
	public void SetCard(int[] symbols) {
		int symbol = 0;
		foreach (Symbol s in this.symbols) {
			s.SetSymbol(symbols[symbol]);
			++symbol;
		}
		this.transform.eulerAngles = Vector3.forward*UnityEngine.Random.value*360;
	}

}
