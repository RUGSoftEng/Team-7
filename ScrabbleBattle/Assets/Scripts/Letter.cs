// Script for the clickable letter.
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class Letter : NetworkBehaviour {
	public GameObject charObject;
	public GameObject background;
	private SpriteRenderer sprite;
	[SyncVar]
	public bool taken; // Whether or not this letter has been taken.

    // Initialisation.
    public void Start () {
		sprite = background.GetComponent<SpriteRenderer>();
		this.taken = false;
	}
	
	// Update is called once per frame.
	public void Update () {
		if (taken) {
			sprite.color = new Color(.4f, .4f, .4f);
		} else {
			sprite.color = new Color(1f, 1f, 1f);
        }
	}
}
