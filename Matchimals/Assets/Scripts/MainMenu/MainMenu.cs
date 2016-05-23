using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : Menu {

	enum menuView_t {MENU, CREDITS, RULE};

	private struct windowPosition_t {
		public int windowWidth;
		public int windowHeight;
		public int windowOffTop;
		public int windowOffLeft;

		public windowPosition_t (int p_windowWidth, int p_windowHeight, int p_windowOffTop, int p_windowOffLeft){
			windowWidth = p_windowWidth;
			windowHeight = p_windowHeight; 
			windowOffTop = p_windowOffTop; 
			windowOffLeft = p_windowOffLeft; 
		}
	}

	// Menu size in percentage (1=100%).
	public float menuWidth = 1f, menuHeight=1f, logoScale=1f, offsetTop=0f;
	private Texture logo;
	//From left to right: creditsIcon, animalIcon, rulesIcon;
	private Texture[] icons = new Texture[3]; 
	private int curAnimal;
    public string hostIP;
	private IconAnimator anim;
	private int indexInstructionPage = 0;
	private const int NUMBER_INSTRUCTION_PAGES = 5;
	private string[] instructionPages = new string[NUMBER_INSTRUCTION_PAGES];

	private menuView_t menuView = menuView_t.MENU;


	public new void Start() {
		this.logo = Resources.Load<Texture>("Menu/logo");
		anim = new IconAnimator ();

		Sprite[] animalSprites = Resources.LoadAll<Sprite>("Animals");
		this.curAnimal = PlayerPrefs.GetInt("animal");
		this.icons [0] = animalSprites [curAnimal].texture;
		this.icons [1] = Resources.Load<Texture> ("Menu/credits");
		this.icons [2] = Resources.Load<Texture> ("Menu/rules");

		this.loadInstructionPages ();

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
        Sprite[] animalSprites = Resources.LoadAll<Sprite>("Animals");
        this.curAnimal = PlayerPrefs.GetInt("animal");
        this.icons[0] = animalSprites[curAnimal].texture;
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

		switch (this.menuView) 
		{
			/*Menu buttons*/
			case menuView_t.MENU:
			{
				menuSkin.button.padding.bottom = 0;
				GUI.DrawTexture(new Rect(Screen.width/2-logoWidth/2,15,logoWidth, logoHeight), logo);
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

				break;
			}

			/*Credits Window*/
			case menuView_t.CREDITS:
			{
				windowPosition_t windowCredits = new windowPosition_t (2*Screen.width/3, 2*Screen.height/3, Screen.height / 6, Screen.width / 6);
				menuSkin.box.alignment = TextAnchor.MiddleCenter;
				//menuSkin.box.fontSize = 30;
				string creditsText = "Matchimals, 2016 (public alpha)\n\n\n" +
					"Developed by RuggedStudio \n(ruggedgamesstudios@gmail.com)\n\n" +
					"Rijksuniversiteit Groningen \n(University of Groningen, NL)\n\n " +
					"Authors:\n Sietze Houwink, Matteo Corradini, \n" +
                    "the impeccable Luc van den Brand, \n" +
					"Twan Schoonen and Dan Chirtoaca";

				GUILayout.BeginArea(new Rect(windowCredits.windowOffLeft, windowCredits.windowOffTop, Screen.width, Screen.height));
				GUI.Box(new Rect (0, 0, windowCredits.windowWidth, windowCredits.windowHeight), creditsText);
				GUILayout.EndArea();

				break;
			}

			/*Rules window*/
			case menuView_t.RULE:
			{
				windowPosition_t windowRules = new windowPosition_t (2*Screen.width/3, 2*Screen.height/3, Screen.height / 6, Screen.width / 6);
				menuSkin.box.alignment = TextAnchor.UpperCenter;
				int tmpPaddingTop = menuSkin.box.padding.top;
				Texture2D tmpNormalBackgroudButton = menuSkin.button.normal.background;
				Texture2D tmpActiveBackgroudButton = menuSkin.button.active.background;
				menuSkin.box.padding.top = Screen.height/20;
				windowPosition_t windowIstrutionText = new windowPosition_t (windowRules.windowWidth, windowRules.windowHeight - Screen.height/6, 
					menuSkin.box.fontSize, 0);
				string instructionText = this.instructionPages [this.indexInstructionPage];

				GUILayout.BeginArea(new Rect(windowRules.windowOffLeft, windowRules.windowOffTop, 
					windowRules.windowWidth, windowRules.windowHeight));
				GUI.Box(new Rect (0, 0, windowRules.windowWidth, windowRules.windowHeight), 
					"Instructions ("+(this.indexInstructionPage+1)+"/"+NUMBER_INSTRUCTION_PAGES+")");
				GUI.Label(new Rect(windowIstrutionText.windowOffLeft, windowIstrutionText.windowOffTop, 
					windowIstrutionText.windowWidth, windowIstrutionText.windowHeight), instructionText);
				GUILayout.EndArea();

				/*Arrow Area*/
				windowPosition_t windowArrow = new windowPosition_t (windowRules.windowWidth, Screen.height / 6, 
					Screen.height - windowRules.windowOffTop - Screen.height / 6, windowRules.windowOffLeft);
				
				Texture arrowIcon, arrowFlippedIcon;
				arrowIcon = Resources.Load<Texture>("Menu/arrow");
				arrowFlippedIcon = Resources.Load<Texture>("Menu/arrow-flipped");
				int arrowSize = windowArrow.windowHeight / 2;
				menuSkin.button.normal.background = null;
				menuSkin.button.active.background = null;

				GUILayout.BeginArea(new Rect(windowArrow.windowOffLeft, windowArrow.windowOffTop, windowArrow.windowWidth, windowArrow.windowHeight));
				GUILayout.BeginHorizontal();
				if (this.indexInstructionPage != 0)
					if (GUILayout.Button(arrowFlippedIcon, GUILayout.Height(arrowSize), GUILayout.Width(arrowSize))) {
					this.indexInstructionPage--;
					}
				GUILayout.FlexibleSpace();
				if (this.indexInstructionPage != NUMBER_INSTRUCTION_PAGES - 1) 
					if (GUILayout.Button(arrowIcon, GUILayout.Height(arrowSize), GUILayout.Width(arrowSize))) {
						this.indexInstructionPage++;
					}
				GUILayout.EndHorizontal();
				GUILayout.EndArea();

				menuSkin.box.padding.top = tmpPaddingTop;
				menuSkin.button.normal.background = tmpNormalBackgroudButton;
				menuSkin.button.active.background = tmpActiveBackgroudButton;
				break;
			}
		}

		menuSkin.button.padding.bottom = 10;
		windowPosition_t windowRuleCredit = new windowPosition_t (Screen.width / 3, Screen.height / 7,
				Screen.height - Screen.height / 7, Screen.width / 20 );

		int iconAnimalSize = (int)((Screen.height / 8) * anim.getIconScale());
		int iconSize = Screen.height / 8;

        /*Credits and info area*/
        GUI.backgroundColor = Color.clear;
        GUILayout.BeginArea(new Rect(windowRuleCredit.windowOffLeft, windowRuleCredit.windowOffTop, windowRuleCredit.windowWidth, windowRuleCredit.windowHeight));
		GUILayout.BeginHorizontal();
		int blankSpaceAmongIcons = Screen.width / 20;
		for (int i = 1; i < icons.Length; i++) {
			if (GUILayout.Button (this.icons [i], GUILayout.Height (iconSize), GUILayout.Width (iconSize))) {
				switch (i) {
				case 1:
					if (this.menuView != menuView_t.RULE)
						this.menuView = this.menuView == menuView_t.CREDITS ? menuView_t.MENU : menuView_t.CREDITS;
					break;
				case 2:
					if (this.menuView != menuView_t.CREDITS) {
						this.indexInstructionPage = 0;
						this.menuView = this.menuView == menuView_t.RULE ? menuView_t.MENU : menuView_t.RULE;
					}
					break;
				}
			}
			GUILayout.Space (blankSpaceAmongIcons);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
        GUI.backgroundColor = Color.white;

        windowPosition_t windowIconAnimal = new windowPosition_t (windowRuleCredit.windowWidth, windowRuleCredit.windowHeight, 
				windowRuleCredit.windowOffTop, 5 * Screen.width / 6);

		/*Animal area*/
		GUILayout.BeginArea(new Rect(windowIconAnimal.windowOffLeft, windowIconAnimal.windowOffTop, windowIconAnimal.windowWidth, windowIconAnimal.windowHeight));
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

	private void loadInstructionPages(){
        instructionPages[0] = "\n\nHey pal! Welcome to Matchimals!\n\n\n" +
            "My name is Ben, but people call me\n"+ 
            "the Matchimal Professor!\n\n"+
            "I'll be showing you how to become\n"+  
            "a butter smooth Matchimal Master.\n\n"+
            "Tap the arrow to read instructions\n"+
            "on how to play!\n";
        instructionPages[1] =
            "Matchimals is a card game\n with ALLOT of symbolism!\n"+
            "That is, we have allot of symbols.\n"+
            "On cards.\n\n"+
			"Now, your goal here will be \n " +
			"to find the matching symbol\n" +
			"on your card with the \n" +
			"one on your TV,\n" +
			"before the other players!";
		instructionPages[2] = "\nThere is always a matching symbol\n" +
			"in each card of the game,\n" +
			"so be quick to press it!\n" +
			"The card of the first player to\n" +
			"match will appear on the TV.";
		instructionPages[3] = "\nBe carefull! You'll get\n" +
			"a time penalty if you press\n" +
			"the wrong symbol!";
		instructionPages[4] = "\nSeems easy?\n\n" +
            "Oh, the innocence...\n" +
			"Okay buddy, Let's play!";
	}
}
