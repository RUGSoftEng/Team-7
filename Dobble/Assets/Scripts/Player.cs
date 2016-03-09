using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Card cardPrefab;

	Card card;

	// Use this for initialization
	void Start () {
		(this.card = (Card)Instantiate (cardPrefab)).Constructor(this.transform);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100)) {
				int id = hit.transform.gameObject.GetInstanceID();

				//this.card.SetCard (new int[] {0,3,2,4,6,7,8,2});
			}
		}
	}
}
