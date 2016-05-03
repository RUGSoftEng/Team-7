using UnityEngine;
using Google.Cast.RemoteDisplay;
using System.Collections;

public class CastManagerController : MonoBehaviour {
    // The camera to cast to.
    public Camera remoteCamera;
    private CastRemoteDisplayManager castDisplayManager;

    void Start () {
        this.castDisplayManager = CastRemoteDisplayManager.GetInstance();
        castDisplayManager.RemoteDisplayErrorEvent.AddListener(OnRemoteDisplayError);
        castDisplayManager.RemoteDisplayCamera = remoteCamera;
    }

    public void OnRemoteDisplayError(CastRemoteDisplayManager manager)
    {
        Debug.LogError("Casting failed: " + manager.GetLastError());
    }
}
