using UnityEngine;
using System.Collections;

public class ZoomInOut : MonoBehaviour {

	private float factorZoom, startTime, duration;
	private Vector3 startScale;
	private bool initialized = false;

	public void Initialize(float factorZoom, float duration) {
		this.startScale = GetComponent<Transform> ().transform.localScale;
		this.factorZoom = factorZoom;
		this.startTime = Time.time;
		this.duration = duration;
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
