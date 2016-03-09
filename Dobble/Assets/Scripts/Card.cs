using UnityEngine;
using System.Collections;
using System;

// circle packing: http://hydra.nat.uni-magdeburg.de/packing/cci/cci.html

public class Card : MonoBehaviour {

	public Symbol symbolPrefab;

	public Sprite[] sprites;
	private float[] scales;

	private Symbol[] symbols; 

	void Start () {

		// hardcoded to 8
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

		this.sprites = Resources.LoadAll<Sprite>("");
		this.scales = new float[this.sprites.Length];
		for (int i = 0; i < this.sprites.Length; ++i) {
			Vector3 size = this.sprites[i].bounds.size;
			this.scales[i] = radius*2/Mathf.Sqrt(size.x*size.x+size.y*size.y);
		}

		this.symbols = new Symbol[8];
		for (int i = 0; i < 8; ++i) {
			(this.symbols[i] = (Symbol) Instantiate (symbolPrefab)).Constructor(this.transform, coordinates[i], sprites, scales);
		}
	}

	public bool ContainsSymbol(int index) {
		foreach (Symbol s in this.symbols) if (s.getSymbol() == index) return true;
		return false;
	}

	public void SetCard(int[] indexes) {
		int index = 0;
		foreach (Symbol s in this.symbols) {
			s.SetSymbol(indexes[index]);
			++index;
		}
		this.transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.value*360);
	}

}
