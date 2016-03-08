using UnityEngine;
using System.Collections;
using System;

public class Symbol : MonoBehaviour {

	public Sprite[] symbols;
	public int symbol;
	
	void Start () {
		this.symbols = Resources.LoadAll<Sprite>("");
	}

	public void SetSymbol(int symbol) {
		if (symbols.Length == 0)
			return;
		this.symbol = symbol;
		gameObject.GetComponent<SpriteRenderer>().sprite = this.symbols[symbol];
		Bounds b = this.symbols[symbol].bounds;
		Vector3 s = b.size;
		float max = Mathf.Max (s.x, Math.Max (s.y, s.z));
		this.transform.localScale = new Vector3(1.0f/max, 1.0f/max, 1.0f/max);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
