// The Lobby script is created when a player is hosting or joined a game, 
// it shows the waiting players.
using UnityEngine;
using System.Collections;

public abstract class Menu : MonoBehaviour {
    protected GUISkin menuSkin;
    protected GameObject[] menuEffects;

	public void Start() {
		this.menuSkin = Resources.Load<GUISkin>("Menu/MainMenu");
    }

    protected void ShowMenuEffects(bool show) {
        if (menuEffects == null) { // Safe the effects, as inactive objects cannot be found.
            menuEffects = GameObject.FindGameObjectsWithTag("MenuEffect");
        }

        foreach (GameObject effect in menuEffects) {
            effect.SetActive(show);
        }
    }

    // Go to a returnable menu.
    protected void GotoMenu<E>() where E : Returnable
    {
        this.enabled = false;
        E menu = this.gameObject.AddComponent<E>();
        menu.SetPrevious(this);
    }

}
