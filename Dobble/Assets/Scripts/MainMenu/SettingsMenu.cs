using UnityEngine;
using System.Collections;

public class SettingsMenu : MonoBehaviour {

	// Menu size in percentage (1=100%).
	public float menuWidth = 0.4f, menuHeight=0.67f, offsetTop=0.15f, iconScale=0.4f;

	private GUISkin menuSkin;
	private MonoBehaviour previous;
	private Sprite[] animalSprites;
	private int curAnimal;
	private string playerName;
	
	public void Start() {
		this.menuSkin = Resources.Load<GUISkin>("Menu/MainMenu");
		animalSprites = Resources.LoadAll<Sprite>("Animals");
		this.curAnimal = PlayerPrefs.GetInt("animal");
		this.playerName = PlayerPrefs.GetString("name");
	}
	
	// Set the caller of this script, so we can move back to it.
	public void SetPrevious(MonoBehaviour previous) {
			this.previous = previous;
	}
	
	private void SafeSettings() {
		PlayerPrefs.SetInt("animal", curAnimal);
		PlayerPrefs.SetString("name", playerName);
		PlayerPrefs.Save();
	}
	
	private int ChangeAnimal(int step){
		this.curAnimal += step;
		this.curAnimal = (int) Mathf.Repeat(curAnimal, animalSprites.Length);
		return curAnimal;
	}
	
	private void GoBack() {
		this.enabled = false;
		previous.enabled = true;
		Destroy(this);
	}
	
	public void OnGUI () {
        GUI.skin = menuSkin;
		int width = (int)(Screen.width*menuWidth);
		int height = (int)(Screen.height*menuHeight);
		int offTop = (int)(Screen.height*offsetTop);
		int buttonHeight = height/4;
		Texture icon = animalSprites[curAnimal].texture;
		int iconWidth   =  (int)(((float)(width*iconScale)/icon.width)*icon.width);
		int iconHeight  =  (int)(((float)(width*iconScale)/icon.width)*icon.height);
		GUI.DrawTexture(new Rect(Screen.width/2-iconWidth/2,50,iconWidth, iconHeight), icon);
        GUILayout.BeginArea(new Rect(Screen.width/2-width/2, Screen.height/2-height/2+offTop, width, height));
        playerName = GUILayout.TextField(playerName, 25);
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Next one!", GUILayout.Height(buttonHeight))) {
			ChangeAnimal(1);
		}
		if (GUILayout.Button("Go back!", GUILayout.Height(buttonHeight))) {
			ChangeAnimal(-1);
		}
		string animalName = animalSprites[curAnimal].name;
		if (GUILayout.Button("Yeah! "+animalName+"s!", GUILayout.Height(buttonHeight))) {
			SafeSettings();
			GoBack();
		}
		GUILayout.EndArea();
    }
}
