using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Letter : NetworkBehaviour {
	public GameObject charObject;
	public GameObject background;
	private BoxCollider2D boxCollider;
	private SpriteRenderer sprite;
	public char character;
	[SyncVar]
	public bool taken;

	// Initialisation.
	public void Start () {
		boxCollider = this.GetComponent<BoxCollider2D>();
		sprite = background.GetComponent<SpriteRenderer>();
		setTaken(false);
	}
	
	// Update is called once per frame.
	public void Update () {
		if (HasClicked()) {
			OnClick();
		}
		if (taken) {
			sprite.color = new Color(.4f, .4f, .4f);
			Vector3 loc = transform.localPosition;
			loc.x += 0.1f;
			transform.localPosition = loc;
		} else {
			sprite.color = new Color(1f, 1f, 1f);
		}
	}
	
	// Sets whether or not this Letter is taken.

	public void setTaken(bool isTaken) {
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
