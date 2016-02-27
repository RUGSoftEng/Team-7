using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	// publicly accessible (zero by Unity default)
	public Vector3 translation;
	
	// called once per frame
	void Update () {
		transform.Translate (translation);
	}
}
