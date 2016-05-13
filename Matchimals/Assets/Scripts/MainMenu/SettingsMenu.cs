using UnityEngine;
using System.Collections;

public class SettingsMenu : Returnable {

	// Menu size in percentage (1=100%).
	public float menuWidth = 0.55f, menuHeight=0.67f, offsetTop=0.15f, iconScale=0.4f;

	private Sprite[] animalSprites;
	private AudioClip[] cries;
	private Texture arrowIcon, arrowFlippedIcon;
	private int curAnimal;
	private string playerName;
	
	public new void Start() {
		animalSprites = Resources.LoadAll<Sprite>("Animals");
		cries = Resources.LoadAll<AudioClip>("AnimalSounds");
		arrowIcon = Resources.Load<Texture>("Menu/arrow");
		arrowFlippedIcon = Resources.Load<Texture>("Menu/arrow-flipped");
		this.curAnimal = PlayerPrefs.GetInt("animal");
		this.playerName = PlayerPrefs.GetString("name");
        base.Start();
	}
	
	private void SafeSettings() {
		PlayerPrefs.SetInt("animal", curAnimal);
		PlayerPrefs.SetString("name", playerName);
		PlayerPrefs.Save();
	}
	
	private int ChangeAnimal(int step){
		this.curAnimal += step;
		this.curAnimal = (int) Mathf.Repeat(curAnimal, animalSprites.Length);
		AudioSource.PlayClipAtPoint(cries[curAnimal], new Vector3(0,0,0));
		return curAnimal;
	}
	
	public void OnGUI () {
        GUI.skin = menuSkin;
		int width = (int)(Screen.width*menuWidth);
		int height = (int)(Screen.height*menuHeight);
		int offTop = (int)(Screen.height*offsetTop);
		int buttonHeight = height/4;
		
		// Set name.
		int setNameHeight = (int)(Screen.height*0.12f);
		int setNameOffTop = (int)(Screen.height*0.01f);
		GUILayout.BeginArea(new Rect(Screen.width/2-width/2, setNameOffTop, width, setNameHeight));
		playerName = GUILayout.TextField(playerName, 25, GUILayout.Height(setNameHeight));
		GUILayout.EndArea();
		
		// Animal selection.
		int animSelHeight = Screen.height/2;
		int animSelWidth = (int)(Screen.width*0.9f);
		int animSelOffTop = setNameOffTop+(int)(Screen.height*0.32f);
		int arrowSize = animSelHeight/2;
		GUI.backgroundColor = Color.clear;
		GUILayout.BeginArea(new Rect(Screen.width/2-animSelWidth/2, animSelOffTop, animSelWidth, animSelHeight));
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(arrowFlippedIcon, GUILayout.Height(arrowSize), GUILayout.Width(arrowSize))) {
			ChangeAnimal(1);
		}
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(arrowIcon, GUILayout.Height(arrowSize), GUILayout.Width(arrowSize))) {
			ChangeAnimal(-1);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();

		Texture icon = animalSprites[curAnimal].texture;
		int iconSize   =  (int)((animSelWidth-2*arrowSize)*0.5f);
		GUILayout.BeginArea(new Rect(Screen.width/2-iconSize/2, animSelOffTop+(arrowSize-iconSize)/2, iconSize, iconSize));
		if (GUILayout.Button(icon, GUILayout.Height(iconSize), GUILayout.Width(iconSize))){
			ChangeAnimal(0);
		}
		GUILayout.EndArea();

		// Back button.
		GUI.backgroundColor = Color.white;
		GUILayout.BeginArea(new Rect(Screen.width/2-width/2, Screen.height/2-height/2+offTop, width, height));
		GUILayout.FlexibleSpace();
		string animalName = animalSprites[curAnimal].name;
		if (GUILayout.Button("Yeah! "+animalName+"s!", GUILayout.Height(buttonHeight))) {
			SafeSettings();
			GoBack();
		}
		GUILayout.EndArea();
    }
}
