using UnityEngine;
using System.Collections;

public abstract class Returnable : Menu {
    protected Menu previous;

    public new void Start() {
        base.Start();
        ShowMenuEffects(false);
    }

    // Set the caller of this script, so we can move back to it.
    public void SetPrevious(Menu previous)
    {
        this.previous = previous;
    }

    public void PassMenuEffects(GameObject[] menuEffects)
    {
        this.menuEffects = menuEffects;
    }

    protected void GoBack()
    {
        ShowMenuEffects(true);
        this.enabled = false;
        previous.enabled = true;
        Destroy(this);
    }
}
