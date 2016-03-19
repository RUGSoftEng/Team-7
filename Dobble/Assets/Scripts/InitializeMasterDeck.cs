using UnityEngine;
using System.Collections;

public class InitializeMasterDeck : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Deck> ().Constructor ();
	}
}
