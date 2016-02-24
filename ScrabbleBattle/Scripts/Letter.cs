using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour {
	public GameObject charObject;
	public char character;
	private TextMesh textMesh;
	private BoxCollider2D boxCollider;

	// Use this for initialization
	void Start () {
		textMesh = charObject.GetComponent<TextMesh>();
		boxCollider = this.GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		textMesh.text = ""+character;
		
		foreach (Touch touch in Input.touches) {
			Vector3 wp = Camera.main.ScreenToWorldPoint(touch.position);
			if (boxCollider.OverlapPoint(wp)) {
				textMesh.text = "OUCH!";
			}
		}
	}
}
