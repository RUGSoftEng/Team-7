using UnityEngine;
using System.Collections;

public class Symbol : MonoBehaviour {

	public Sprite[] symbols;
	public int symbol;
	
	void Start () {
		this.symbols = Resources.LoadAll<Sprite>("");
		Debug.Log (symbols.Length);
	}

	public void SetSymbol(int symbol) {
		Debug.Log ("symbol: " + symbol);
		this.symbol = symbol;
		gameObject.GetComponent<SpriteRenderer>().sprite = this.symbols[symbol];
	}

	// Update is called once per frame
	void Update () {
	
	}
}
