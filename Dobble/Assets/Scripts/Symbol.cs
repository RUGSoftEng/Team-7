using UnityEngine;
using System.Collections;
using System;

public class Symbol : MonoBehaviour {

	public int index;

	Sprite[] sprites;
	SpriteRenderer spriteRenderer;
	
	void Start () {
		this.sprites = GetComponentInParent<Card> ().sprites;
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public void SetSymbol(int index) {
		this.index = index;
		this.spriteRenderer.sprite = this.sprites[index];
		Vector3 s = this.sprites[index].bounds.size;
		float scale = 1.0f/Mathf.Max (s.x, s.y);
		this.transform.localScale = new Vector3(scale, scale, scale);
		this.transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.value*360);
	}

}
