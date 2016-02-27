using UnityEngine;
using System.Collections;

public class WordLetter : MonoBehaviour {
    public GameObject charObject;
    public GameObject background;
    private BoxCollider2D boxCollider;
    private SpriteRenderer sprite;
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void setWordLetter(GameObject letter)
    {
        this.charObject.GetComponent<TextMesh>().text = letter.GetComponent<TextMesh>().text;
    }
}
