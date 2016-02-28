// Graphical script for the guy drawn on screen.
using UnityEngine;
using System.Collections;

public class GuyScript : MonoBehaviour {
	public bool flip; // Whether or not to flip the sprite.
	public float bounce = 1f;
	private float a = 0f;
	private float scaleY;
	private float posY;

	// Initialization
	void Start () {
		Vector3 scale = transform.localScale;
		scaleY = scale.y;
		if (flip) {
			scale.x *= -1;
			transform.localScale = scale;
		}
		posY = transform.localPosition.y;
	}
	
	// Bouncy-stretch the gameobject based on time and bounce.
	void FixedUpdate() {
		a += 0.1f;
		Vector3 scale = transform.localScale;
		Vector3 pos = transform.localPosition;
		float stretch = Time.deltaTime*Mathf.Cos(a)*bounce;
		scale.y = scaleY + stretch;
		pos.y = posY + 2*stretch;
		transform.localScale = scale;
		transform.localPosition = pos;
	}
}
