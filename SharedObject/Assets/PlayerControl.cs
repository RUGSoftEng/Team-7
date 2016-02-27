using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {

	public Move move;
	private Vector3 previousTranslation;

	void Start () {
		
		// important optimization, save reference to move object only once.
		move = GameObject.FindGameObjectsWithTag("SharedObject")[0].GetComponent<Move>();

	}

	// let command execute on server
	[Command]
	void CmdTranslate(Vector3 translation) {
		move.translation = translation;
	}

	// called once per frame
	void Update () {
		var x = Input.GetAxis("Horizontal")*0.1f;
		var z = Input.GetAxis("Vertical")*0.1f;
		var translation = new Vector3 (x, 0, z);

		// important optimization, only update when keys have changed
		if (!translation.Equals(previousTranslation))
			CmdTranslate (new Vector3(x,0,z));
		
	}
}
