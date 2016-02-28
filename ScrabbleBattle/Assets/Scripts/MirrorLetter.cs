// This script allows a gameobject to mirror another letter.
using UnityEngine;
using System.Collections;

public class MirrorLetter : MonoBehaviour {
	public GameObject letter; // The letter to mirror.
	public GameObject charObject;
	public GameObject background;
	private Letter other;
	private bool taken;
	private SpriteRenderer sprite;
	
	// Called on creation of the object.
	void Start () {
		sprite = background.GetComponent<SpriteRenderer>();
		other = (Letter) letter.GetComponent(typeof(Letter));
		charObject.GetComponent<TextMesh>().text = other.charObject.GetComponent<TextMesh>().text;
	}
	
	// Update is called once per frame
	void Update () {
		taken = other.taken;
		if (taken) {
			sprite.color = new Color(.4f, .4f, .4f);
		} else {
			sprite.color = new Color(1f, 1f, 1f);
		}
	}
}
