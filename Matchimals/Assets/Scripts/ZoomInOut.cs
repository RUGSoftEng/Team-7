using UnityEngine;
using System.Collections;

public class ZoomInOut : MonoBehaviour {

	public float factorZoom, duration;

	private float startTime;
	private Vector3 startScale;
	private bool initialized = false;

	public void Initialize() {
		this.startScale = GetComponent<Transform> ().transform.localScale;
		this.startTime = Time.time;
		initialized = true;
	}

	// Update is called once per frame
	void Update () {
		if (initialized) {
			if (Time.time < startTime + duration) { 
				GetComponent<Transform> ().transform.localScale = startScale * (1 + (factorZoom - 1) * (0.5f - 0.5f * Mathf.Cos(2*Mathf.PI*(Time.time - startTime)/duration))); 
			} else {
				GetComponent<Transform> ().transform.localScale = startScale;
				initialized = false;
			}
		}
	}
}
