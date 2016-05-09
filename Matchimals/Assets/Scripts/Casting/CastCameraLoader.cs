using UnityEngine;
using Google.Cast.RemoteDisplay;
using System.Collections;

public class CastCameraLoader : MonoBehaviour {

    public void OnLevelWasLoaded(int level) {
        CastRemoteDisplayManager.GetInstance().RemoteDisplayCamera = FindCastCamera();
    }

    private Camera FindCastCamera() {
        foreach (Camera cam in GameObject.FindObjectsOfType<Camera>()) {
            if (cam.name == "Cast Camera") {
                return cam;
            }
        }
        return null;
    }
}
