using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	public void setCard(int[] indexes) {
		for (int i = 0; i < indexes.Length; ++i) {
			setSymbol(i,indexes[i]);
		}
	}

	public void setSymbol(int symbol, int index) {
		GetComponentsInChildren<Symbol>()[symbol].SetSymbol(index);
	}

	// Update is called once per frame
	void Update () {

	}
}
