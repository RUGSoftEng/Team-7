using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// The skin to use for the menu.
	public GUISkin menuSkin;
	// Menu size in percentage (1=100%).
	public float menuWidth = 1f, menuHeight=1f, offsetTop=0f, logoScale=1f;
	
	private Texture logo;
	
	public void Start() {
		this.logo = Resources.Load<Texture>("Menu/logo");
	}
	
	// Start hosting a new game, go to lobby.
	private void Host() {
		// TODO: Add proper code!
		Debug.Log("Host!");
		Application.LoadLevel ("LobbyScene"); // Filler for now, to make the game playable with this menu.
	}
	
	// Join a local game, go to lobby.
	private void Join() {
		// TODO: Add proper code!
		Debug.Log("Join!");
	}
	
	// Customize player (name, animal, etc.).
	private void Customize() {
		// TODO: Add proper code!
		Debug.Log("Customize!");
	}
	
	// Returns wether or not a local game is being hosted.
	private bool isGameHosted() {
		return false; // TODO: Add proper code!
	}
	
	public void OnGUI () {
        GUI.skin = menuSkin;
		int width = (int)(Screen.width*menuWidth);
		int height = (int)(Screen.height*menuHeight);
		int offTop = (int)(Screen.height*offsetTop);
		int logoWidth   =  (int)(((float)(width*logoScale)/logo.width)*logo.width);
		int logoHeight  =  (int)(((float)(width*logoScale)/logo.width)*logo.height);
		GUI.DrawTexture(new Rect(Screen.width/2-logoWidth/2,15,logoWidth, logoHeight), logo);
        GUILayout.BeginArea(new Rect(Screen.width/2-width/2, Screen.height/2-height/2+offTop, width, height));
        if (GUILayout.Button("Host Party")) {
			Host();
		}
		if (isGameHosted()) {
			if (GUILayout.Button("Visit Party")) {
				Join();
			}
		}
		if (GUILayout.Button("Customize")) {
			Customize();
		}
		GUILayout.EndArea();

    }
}
