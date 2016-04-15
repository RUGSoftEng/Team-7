using UnityEngine;
using System.Collections;

public class SymbolFlipper : MonoBehaviour {
	private Sprite[] sprites;
	
	public void Start () {
		this.sprites = Resources.LoadAll<Sprite>("Symbols");
		NextSprite();
	}
	
	private void NextSprite() {
		int spriteNumber = Random.Range(0,sprites.Length);
		GetComponent<SpriteRenderer>().sprite = sprites[spriteNumber];
	}
	
	// Change sprite if invisible.
	public void OnBecameInvisible() {
		NextSprite();
	}
}
