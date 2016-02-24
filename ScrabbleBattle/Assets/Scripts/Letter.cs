using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour {
	public GameObject charObject;
	public GameObject background;
	private TextMesh textMesh;
	private BoxCollider2D boxCollider;
	private SpriteRenderer sprite;
	private bool taken;

	// Initialisation.
	public void Start () {
		textMesh = charObject.GetComponent<TextMesh>();
		boxCollider = this.GetComponent<BoxCollider2D>();
		sprite = background.GetComponent<SpriteRenderer>();
		setTaken(false);
	}
	
	// Update is called once per frame.
	public void Update () {
		if (HasClicked()) {
			OnClick();
		}
	}
	
	public void setChar(char c) {
		textMesh.text = ""+c;
	}
	
	// Sets whether or not this Letter is taken.
	public void setTaken(bool isTaken) {
		if (isTaken) {
			sprite.color = new Color(.4f, .4f, .4f);
		} else {
			sprite.color = new Color(1f, 1f, 1f);
		}
		this.taken = isTaken;
	}
	
	// Called when this Letter has been clicked.
	private void OnClick() {
		setTaken(!taken);
	}
	
	// Checks for a mouseclick, use this on PC.
	private bool HasClicked() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (boxCollider.OverlapPoint(wp)) {
				return true;
			}
		}
		return false;
	}
	
	// Checks for a touch.
	private bool HasTouched() {
		foreach (Touch touch in Input.touches) {
			Vector3 wp = Camera.main.ScreenToWorldPoint(touch.position);
			if (boxCollider.OverlapPoint(wp) && touch.phase == TouchPhase.Ended) {
				return true;
			}
		}
		return false;
	}
}
