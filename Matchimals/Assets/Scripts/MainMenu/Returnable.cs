using UnityEngine;
using System.Collections;

public class Returnable : MonoBehaviour {
    private MonoBehaviour previous;

    // Set the caller of this script, so we can move back to it.
    public void SetPrevious(MonoBehaviour previous)
    {
        this.previous = previous;
    }

    protected void GoBack()
    {
        this.enabled = false;
        previous.enabled = true;
        Destroy(this);
    }
}
