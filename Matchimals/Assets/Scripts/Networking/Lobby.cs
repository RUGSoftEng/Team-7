// The Lobby script is created when a player is hosting or joined a game, 
// it shows the waiting players.
using UnityEngine;
using System.Collections;

public class Lobby : Returnable {
	private GUISkin menuSkin;
	
	public void Start() {
		this.menuSkin = Resources.Load<GUISkin>("Menu/MainMenu");
	}
	
	// Draws the GUI.
	public void OnGUI () {
		GUI.skin = menuSkin;
		
		Player[] players = getPlayers();
		foreach (Player player in players) {
            //Draw the waiting players.
            Debug.Log("Player!");
		}
		//Draw the ready button.
	}
	
	private Player[] getPlayers() {
		return GameObject.FindObjectsOfType(typeof(Player)) as Player[];
	}
	
	// Checks if all players are ready.
	private bool allReady(Player[] players) {
		bool ready = true;
		foreach (Player player in players) {
			// TODO: create ready function in Player. 
			//ready = ready && player.isReady();
		}
		return ready;
	}
	
	// Start the game with all the players in it.
	private void startGame() {
		
	}
	
}
