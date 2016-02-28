// Server side script.
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	
	// Update is called once per frame
	void Update () {
		CheckClicked();
	}
	
	// Called on the server by the client.
	[Command]
	void CmdSetTaken(string name) {
		SetTaken(name);
	}
	
	// Sets the letter with the given name to taken.
	void SetTaken(string name) {
		Letter l = GameObject.Find(name).GetComponent<Letter>();
		l.taken = !l.taken;
	}
	
	// Check if a letter has been clicked.
	void CheckClicked() {
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit;
         
			if( Physics.Raycast( ray, out hit, 100 ) ) {
				string hitName = hit.transform.gameObject.name;
				if (Network.isServer) {
					SetTaken(name);
				} else {
					CmdSetTaken(hitName);
				}
			}
		}
	}
	
}
