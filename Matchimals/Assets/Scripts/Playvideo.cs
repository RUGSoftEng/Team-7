using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof(AudioSource))]

public class Playvideo : MonoBehaviour {
	private float timer = 5f;
	private string levelToLoad = "StartupScene";

	public MovieTexture movie;
	private AudioSource audio;

	// Use this for initialization
	void Start () {
		GetComponent<RawImage>().texture = movie as MovieTexture;
		audio = GetComponent<AudioSource>();
		audio.clip = movie.audioClip;
		movie.Play();
		audio.Play();
		StartCoroutine("DispScene");
	}

	IEnumerator DispScene(){
		yield return new WaitForSeconds(timer);
		Application.LoadLevel(levelToLoad);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
