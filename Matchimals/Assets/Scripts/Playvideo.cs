using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Playvideo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		#if UNITY_ANDROID
			Handheld.PlayFullScreenMovie("splash.mp4", Color.blue, FullScreenMovieControlMode.Hidden);
        #endif
        SceneManager.LoadScene("StartupScene");
    }
}
