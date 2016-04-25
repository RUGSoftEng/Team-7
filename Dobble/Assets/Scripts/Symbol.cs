using UnityEngine;
using System.Collections;
using System;

public class Symbol : MonoBehaviour {

	// the index to the current symbol
	private int symbol;

	// the sprites (images) of the respective symbols
	private Sprite[] sprites;
	// the scales of the respective sprites (to make it fit with the other sprites in a unit circle)
	private float[] scales;

	// quick reference to the sprite renderer
	private SpriteRenderer spriteRenderer;

	// constructor
	public void Constructor(Transform parent, Vector2 position, Sprite[] sprites, float[] scales) {
		this.spriteRenderer = GetComponent<SpriteRenderer> ();


		this.transform.SetParent (parent);
		this.transform.position = position;
		this.transform.localPosition -= new Vector3 (0, 0, 0.00001f);
		this.sprites = sprites;
		this.scales = scales;
	}

	// gets symbol
	public int getSymbol() {
		return this.symbol;
	}

	// sets symbol and random rotation
	public void SetSymbol(int symbol) {
		this.symbol = symbol;
		this.name = ""+symbol;
		this.spriteRenderer.sprite = this.sprites[symbol];
		this.transform.localScale = Vector3.one*scales [symbol];
		this.transform.eulerAngles = Vector3.forward*UnityEngine.Random.value*360;
	}

}
