// The Lobby script is created when a player is hosting or joined a game, 
// it shows the waiting players.
using UnityEngine;
using UnityEngine.Networking;
using Google.Cast.RemoteDisplay;


public class CastConnector : Returnable {

	public CastRemoteDisplayManager castDisplayManager;
    private GameObject castSearchAnimation;

    public new void Start() {
        base.Start();
        castSearchAnimation = Instantiate(Resources.Load("Menu/CastSearchAnimation", typeof(GameObject))) as GameObject;
        Debug.Assert(castSearchAnimation != null);
        castSearchAnimation.transform.localPosition = new Vector3(0, 0.25f, 6);
        this.castDisplayManager = GameObject.FindObjectOfType<CastDisplayManager>();
        Debug.Assert(castDisplayManager != null);
    }

    public void Update() {
        if (IsCastConnected())
        {
            // Start hosting and join lobby.
            Destroy(castSearchAnimation);
            LocalGameFinder localGameFinder = GameObject.FindObjectOfType<LocalGameFinder>();
            localGameFinder.StartBroadCasting();
            GameNetworkManager networkManager = GameObject.FindObjectOfType<GameNetworkManager>();
            networkManager.StartHost();
            GotoMenu<Lobby>();
        }
    }

	// Draws the GUI.
	public void OnGUI () {
		GUI.skin = menuSkin;

        int padding = 10;
        int width = Screen.width - 2*padding;
        int height = Screen.height - 2*padding;
        int buttonHeight = (int)(0.2*height);

        GUILayout.BeginArea(new Rect((Screen.width-width)/2, (Screen.height-height)/2, width, height));
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Cancel Party...", GUILayout.Height(buttonHeight))) {
            Destroy(castSearchAnimation);
            GoBack();
        }
        GUILayout.EndArea();
    }

    private bool IsCastConnected() {
        // TODO: Write proper implementation.
        return !false;
    }

    public new void GoBack() {
        LocalGameFinder localGameFinder = GameObject.FindObjectOfType<LocalGameFinder>();
        localGameFinder.StopBroadCasting();
        base.GoBack();
    }

    // Go to a returnable menu, but pass the parent menu.
    protected new void GotoMenu<E>() where E : Returnable
    {
        Destroy(castSearchAnimation);
        this.enabled = false;
        E menu = this.gameObject.AddComponent<E>();
        menu.SetPrevious(previous);
        menu.PassMenuEffects(menuEffects);
        Destroy(this);
    }
}
