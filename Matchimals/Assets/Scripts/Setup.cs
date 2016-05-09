using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Setup : MonoBehaviour {

	public void Start () {
		if (!PlayerPrefs.HasKey("initialised")){
			SetupPlayer();
		}
        SceneManager.LoadScene("MainMenuScene");
	}
	
	string GenName() {
		const int minLen = 2;
		const int maxLen = 12;
		const string consonants = "QWRTYPSDFGHJKLZXCVBNM";
		const string vowels		= "EUIA";
		string str = "";
		for (int i=0; i<Random.Range(minLen,maxLen); i++) {
			if (Random.Range(0,2) == 0) {
				str += consonants[Random.Range(0,consonants.Length)];
			} else {
				str += vowels[Random.Range(0,vowels.Length)];
			}
		}
		return str;
	}
	
	// Setup default values.
	private void SetupPlayer () {
		int animalCount = Resources.LoadAll<Sprite>("Animals").Length;
		PlayerPrefs.SetInt("animal", Random.Range(0,animalCount));
		PlayerPrefs.SetString("name", GenName());
		PlayerPrefs.SetInt("initialised", 0);
		PlayerPrefs.Save();
	}
}
