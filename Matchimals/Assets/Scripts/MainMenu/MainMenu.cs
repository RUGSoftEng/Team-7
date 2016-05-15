using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : Menu {

	// Menu size in percentage (1=100%).
	public float menuWidth = 1f, menuHeight=1f, logoScale=1f, offsetTop=0f;
	private Texture logo;
	//From left to right: creditsIcon, animalIcon, rulesIcon;
	private Texture[] icons; 
	private int curAnimal;
    public string hostIP;
	private IconAnimator anim;
	private bool showCredits = false;
	private bool showRules = false;

	public new void Start() {
		this.logo = Resources.Load<Texture>("Menu/logo");
		anim = new IconAnimator ();

		this.icons = new Texture[2];
		Sprite[] animalSprites = Resources.LoadAll<Sprite>("Animals");
		this.curAnimal = PlayerPrefs.GetInt("animal");
		this.icons [0] = animalSprites [curAnimal].texture;
		this.icons [1] = Resources.Load<Texture> ("Menu/credits");
		//this.icons [2] = Resources.Load<Texture> ("Menu/rules");

        this.hostIP = null;
        base.Start();
	}
	
	public void Update () {
		anim.AnimateIcon ();
		if (Input.GetKey(KeyCode.Escape) && Application.platform == RuntimePlatform.Android) {
			Application.Quit();
		}
	}
	
    // Called when the object is enabled.
    public void OnEnable() {
        this.hostIP = null;
    }

	// Join a local game, go to lobby.
	private void Join() {
        GameNetworkManager networkManager = GameObject.FindObjectOfType<GameNetworkManager>();
        networkManager.networkAddress = hostIP;
        networkManager.StartClient();
        SceneManager.LoadScene("LobbyScene");
    }

    private void Host() {
        GotoMenu<CastConnector>();
    }

    // Returns wether or not a local game is being hosted.
    private bool isGameHosted() {
        return hostIP != null; // TODO: Add proper code!
	}
	
	public void OnGUI () {
        GUI.skin = menuSkin;
		int width = (int)(Screen.width*menuWidth);
		int height = (int)(Screen.height*menuHeight);
		int offTop = (int)(Screen.height*offsetTop);

		int logoWidth   =  (int)(((float)(width*logoScale)/logo.width)*logo.width);
		int logoHeight  =  (int)(((float)(width*logoScale)/logo.width)*logo.height);

		int buttonHeight = height/4;

		int iconAnimalSize = (int)((Screen.height / 8) * anim.getIconScale());
		int iconSize = Screen.height / 8;
		int offBottomIcon = Screen.height - Screen.height / 7;
		int iconAreaHeight = Screen.height / 7;

		GUI.DrawTexture(new Rect(Screen.width/2-logoWidth/2,15,logoWidth, logoHeight), logo);

		if (!this.showCredits && !this.showRules) {
			/*Menu Area*/
			GUILayout.BeginArea (new Rect (Screen.width / 2 - width / 2, Screen.height / 2 - height / 2 + offTop, width, height));
			GUILayout.FlexibleSpace ();
			if (isGameHosted ()) {
				if (GUILayout.Button ("Visit Party", GUILayout.Height (buttonHeight))) {
					Join ();
				}
			} else {
				if (GUILayout.Button ("Host Party", GUILayout.Height (buttonHeight))) {
					Host ();
				}
			}
			if (GUILayout.Button ("Customize", GUILayout.Height (buttonHeight))) {
				GotoMenu<SettingsMenu> ();
			}
			if (GUILayout.Button ("Exit", GUILayout.Height (buttonHeight))) {
				Application.Quit ();
			}
			GUILayout.EndArea ();
		} else {
			/*Credits window*/
			int creditsWindowWidth = 2*Screen.width/3;
			int creditsWindowHeight = 2*Screen.height/3;
			int creditsWindowOffTop = Screen.height / 6;
			int creditsWindowOffLeft =  Screen.width / 6;
			string creditsText = "Matchminimals, 2016\n\n\n" +
				"Developed by RuggedStudio \n(ruggedgamesstudios@gmail.com)\n\n" +
				"Rijksuniversiteit Groningen \n(University of Groningen, NL)\n\n " +
				"Authors: Sietze Houwink, Matteo Corradini, \n" +
				"Victor Matei Preda, Luc van den Brand, \n" +
				"Twan Schoonen and Dan Chirtoaca";
			
			GUILayout.BeginArea(new Rect(creditsWindowOffLeft, creditsWindowOffTop, Screen.width, Screen.height));
			GUI.Box(new Rect (0, 0, creditsWindowWidth, creditsWindowHeight), creditsText);
			GUILayout.EndArea();
		}

		/*Credits and info area*/
		GUILayout.BeginArea(new Rect(Screen.width/10, offBottomIcon, Screen.width/3, iconAreaHeight));
		GUILayout.BeginHorizontal();
		int blankSpaceAmongIcons = Screen.height / 10;
		for (int i = 1; i < icons.Length; i++) {
			if (GUILayout.Button (this.icons [i], GUILayout.Height (iconSize), GUILayout.Width (iconSize))) {
				switch (i) {
				case 1:
					if (!this.showRules)
						this.showCredits = !this.showCredits ;
					break;
				case 2:
					if (!this.showCredits)
						this.showRules = !this.showRules;
					break;
				}
			}
			GUILayout.Space (blankSpaceAmongIcons);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();

		/*Animal area*/
		GUILayout.BeginArea(new Rect(4*Screen.width/5, offBottomIcon, Screen.width/3, iconAreaHeight));
		GUILayout.BeginHorizontal();
		GUI.backgroundColor = Color.clear;
		if (GUILayout.Button (this.icons [0], GUILayout.Height (iconAnimalSize), GUILayout.Width (iconAnimalSize))) {
			anim.setAnimation (true);
			AudioClip[] cries;
			cries = Resources.LoadAll<AudioClip> ("AnimalSounds");
			AudioSource.PlayClipAtPoint (cries [this.curAnimal], new Vector3 (0, 0, 0));
		}	
		GUILayout.EndHorizontal();
		GUILayout.EndArea();

    }

    public void SetHostIP(string ip)
    {
        this.hostIP = ip;
    }
}
