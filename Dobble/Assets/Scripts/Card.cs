using UnityEngine;
using System.Collections;
using System;

// circle packing: http://hydra.nat.uni-magdeburg.de/packing/cci/cci.html

public class Card : MonoBehaviour {

	public Symbol symbolPrefab;
	public Sprite[] sprites;
	
	Symbol[] symbols;

	void Start () {
		this.sprites = Resources.LoadAll<Sprite>("");

		// hardcoded to 8
		float radius = 0.302593388348611302909204224934f;
		float factor = 1/(2*radius);
		((Symbol)Instantiate (symbolPrefab, new Vector3 (-0.302593388348611302909204224933f*factor, -0.628341645367213738512227388956f*factor, 0), Quaternion.identity)).transform.SetParent(this.transform);
		((Symbol)Instantiate (symbolPrefab, new Vector3 (0.302593388348611302909204224933f*factor, -0.628341645367213738512227388956f*factor, 0), Quaternion.identity)).transform.SetParent(this.transform);
		((Symbol)Instantiate (symbolPrefab, new Vector3 (-0.679921171839088240043878874469f*factor, -0.155187570571975671990838057814f*factor, 0), Quaternion.identity)).transform.SetParent(this.transform);
		((Symbol)Instantiate (symbolPrefab, new Vector3 (0.679921171839088240043878874469f*factor, -0.155187570571975671990838057814f*factor, 0), Quaternion.identity)).transform.SetParent(this.transform);
		((Symbol)Instantiate (symbolPrefab, new Vector3 (0.000000000000000000000000000000f*factor, 0.000000000000000000000000000000f*factor, 0), Quaternion.identity)).transform.SetParent(this.transform);
		((Symbol)Instantiate (symbolPrefab, new Vector3 (-0.545254445070410775447749861103f*factor, 0.434825910113495061957667559237f*factor, 0), Quaternion.identity)).transform.SetParent(this.transform);
		((Symbol)Instantiate (symbolPrefab, new Vector3 (0.545254445070410775447749861103f*factor, 0.434825910113495061957667559237f*factor, 0), Quaternion.identity)).transform.SetParent(this.transform);
		((Symbol)Instantiate (symbolPrefab, new Vector3 (0.000000000000000000000000000000f*factor,  0.697406611651388697090795775067f*factor, 0), Quaternion.identity)).transform.SetParent(this.transform);

		this.symbols = GetComponentsInChildren<Symbol> ();
	}

	public bool ContainsSymbol(int index) {
		foreach (Symbol s in this.symbols) {
			if (s.index == index) return true;
		}
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
