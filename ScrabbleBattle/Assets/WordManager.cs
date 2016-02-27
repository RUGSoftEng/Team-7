using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WordManager : MonoBehaviour {
    List<GameObject> word = new List<GameObject>();
    List<GameObject> boardLetter = new List<GameObject>();
    public GameObject wordLetter;
    public Letter freeLetter;
    GameObject temp;
    GameObject letter;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    //create 2 arrays one of the letters selected one for the letters from the common board which become unavailable
    public void createWord(GameObject l)
    {
        int n = word.Count;
        boardLetter.Add(l);
        temp=Instantiate(wordLetter,new Vector3(n+5,0,0), Quaternion.identity)as GameObject;
        temp.SetActive(true);
        word.Add(temp);
    }
    //free the current word and emty the word array
    public void clearWord()
    {
        int n = word.Count;
        for (int i = 0; i < n; i++)
        {
            Debug.Log("da");
            word[i].SetActive(false);
            boardLetter[i].GetComponent<Letter>().setTaken(false);
        }
        word.Clear();
    }
}
