using UnityEngine;
using System.Collections;
using System;

public class Symbol : MonoBehaviour {

	private int index;
	public float radius;
	public Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public int getIndex() {
		return this.index;
	}

	public void SetSymbol(int index) {
		this.index = index;
		this.spriteRenderer.sprite = this.sprites[index];
		Vector3 s = this.sprites[index].bounds.size;
		float scale = 1.0f/Mathf.Sqrt (s.x*s.x + s.y*s.y)*this.radius*2;
		this.transform.localScale = new Vector3(scale, scale, scale);
		this.transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.value*360);
	}

}
