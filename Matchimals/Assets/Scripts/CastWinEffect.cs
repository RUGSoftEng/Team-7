using UnityEngine;
using System.Collections;

public class CastWinEffect : MonoBehaviour {
    private const float EFFECT_TIME = 12f;
    public GameObject winText, nameText, matchimal;
    public Obfuscator obfuscator;
	
    // By default, the effect is hidden.
	public void OnEnable() {
        Show(false);
	}
	
	public void ShowWin(string name, string animalName) {
        obfuscator.Obfuscate(0.6f);
        SpriteRenderer spriteRenderer = matchimal.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Animals/" + animalName);
        TextMesh textMesh = nameText.GetComponent<TextMesh>();
        textMesh.text = name + " won!";
        winText.GetComponent<Move>().Initialize(new Vector3(100, 2, -26), new Vector3(100, 1f, -26), 1f);
        nameText.GetComponent<Move>().Initialize(new Vector3(100, -2, -26), new Vector3(100, -1f, -26), 1f);
        matchimal.GetComponent<Move>().Initialize(new Vector3(96.5f, 0.05f, -26), new Vector3(100, 0.05f, -26), 1f);
        Show(true);
        Invoke("HideWin", EFFECT_TIME);
    }

    private void HideWin() {
        obfuscator.Obfuscate(1f);
        winText.GetComponent<Move>().Initialize(new Vector3(100, 1f, -26), new Vector3(100, 2, -26), 1f);
        nameText.GetComponent<Move>().Initialize(new Vector3(100, -1f, -26), new Vector3(100, -2, -26), 1f);
        matchimal.GetComponent<Move>().Initialize(new Vector3(100, 0.05f, -26), new Vector3(103.5f, 0.05f, -26), 1f);
    }

    private void Show(bool hideWin) {
        winText.SetActive(hideWin);
        nameText.SetActive(hideWin);
        //matchimal.SetActive(hideWin);
    }
}
