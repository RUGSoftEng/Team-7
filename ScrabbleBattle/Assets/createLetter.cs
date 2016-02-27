using UnityEngine;
using System.Collections;
using System;

public class createLetter : MonoBehaviour {
    public GameObject x;
    public GameObject[] tiles_letter;
    GameObject temp;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void fao()
    {
        //for (int i = 0; i < 5; i++){
           //temp = GameObject.Instantiate(x,new Vector3(i+5,0,0),Quaternion.identity) as GameObject;
        //temp.SetActive(true);
        //}
    }

    //public void da()
   // {
       // Debug.Log("a ajuns ci");
    //}
    public void palmea()
    {

    }

    internal void palmea(GameObject t)
    {
        temp = GameObject.Instantiate(t, new Vector3(5, 0, 0), Quaternion.identity) as GameObject;
        temp.SetActive(true);
        Debug.Log("a ajuns");
    }
}
