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

	private float count=0.0F;

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

		//uses a cosine to zoom in and out of the current symbol
	public void ZoomIn(int symbol) {
		this.count+=0.1F;
		if(count < Mathf.PI*2){
			this.transform.localScale = Vector3.one*scales [symbol] * (-Mathf.Cos(count)+2);
		}
	}

	//Reset this symbol its zoom properties
	public void ResetZoom(int symbol){
		this.count=0.0f;
		this.transform.localScale = Vector3.one*scales [symbol];
	}

}
