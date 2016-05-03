using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	private Vector3 initialPosition, finalPosition;
	private float startTime, duration;
	private float time;
	private bool initialized = false;

	public void Initialize(Vector3 initialPosition, Vector3 finalPosition, float duration) {
		this.initialPosition = initialPosition;
		this.finalPosition = finalPosition;
		this.startTime = Time.time;
		this.duration = duration;
		initialized = true;
	}

	// Update is called once per frame
	void Update () {
		if (initialized) {
			if (Time.time < startTime + duration) { 
				GetComponent<Transform> ().transform.position = initialPosition + (finalPosition - initialPosition) * (Time.time - startTime) / duration;
			} else {
				GetComponent<Transform> ().transform.position = finalPosition;
			}
		}
	}
}
