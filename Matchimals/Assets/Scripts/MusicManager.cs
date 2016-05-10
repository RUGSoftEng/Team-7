using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {
	
	public AudioClip relaxedMusic, panicMusic, endMusic;
	public int panicThreshold = 5;
	private AudioSource audioSource;
	private Deck deck;

	// Use this for initialization
	public void Start () {
		this.audioSource = GetComponent<AudioSource>();
		audioSource.clip = relaxedMusic;
		audioSource.Play();
	}
	
	private bool IsBelowThreshold() {
		Player[] players = GameObject.FindObjectsOfType(typeof(Player)) as Player[];
		for (int i=0; i<players.Length; i++) {
			int count = players[i].cardcount;
			if (count < panicThreshold && count > 0) {
					return true;
			}
		}
		return false;
	}
	
	private void SwitchMusic(AudioClip music) {
		if (audioSource.clip != music) {
			audioSource.clip = music;
			audioSource.Play();
		}
	}
	
	public void Update () {
		if (this.deck == null) {
			this.deck = GameObject.FindObjectOfType(typeof(Deck)) as Deck;
		} else if (deck.IsGameOver()) {
			SwitchMusic(endMusic);
		} else if (IsBelowThreshold()) {
			SwitchMusic(panicMusic);
		} else {
			SwitchMusic(relaxedMusic);
		}
	}
}
