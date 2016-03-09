using UnityEngine;
using System.Collections;
using System;

public class Symbol : MonoBehaviour {

	private int symbol;

	private Sprite[] sprites;
	private float[] scales;

	private SpriteRenderer spriteRenderer;

	public void Constructor(Transform parent, Vector2 position, Sprite[] sprites, float[] scales) {
		this.spriteRenderer = GetComponent<SpriteRenderer> ();

		this.transform.SetParent (parent);
		this.transform.position = position;
		this.sprites = sprites;
		this.scales = scales;
	}

	public int getSymbol() {
		return this.symbol;
	}

	public void SetSymbol(int symbol) {
		this.symbol = symbol;
		this.spriteRenderer.sprite = this.sprites[symbol];
		this.transform.localScale = Vector3.one*scales [symbol];
		this.transform.eulerAngles = Vector3.forward*UnityEngine.Random.value*360;
	}

}
