using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class Login : MonoBehaviour {

	// static variables
	public static string Name = "";
	public static string Password = "";

	public FileInfo NameFile;

	void Start () {
		NameFile = new FileInfo(Application.persistentDataPath +  "\\" + "NameSave.txt");
		if (NameFile.Exists){
			Application.LoadLevel ("LobbyScene");
		}
	}

	void OnGUI(){
		if(!NameFile.Exists){
			LoginGUI();
		}
	}

	void LoginGUI(){
		GUI.Box(new Rect(120,70,(Screen.width/4)+200,(Screen.height/4)+180), "Login");
		if (GUI.Button(new Rect(225,200,120,25), "Log In")){

			StreamWriter w;
     		if(!NameFile.Exists) {
         		w = NameFile.CreateText();    
     		} else {
         		NameFile.Delete();
         		w = NameFile.CreateText();
     		}
     		w.WriteLine(Name);
     		w.Close();
			Application.LoadLevel ("LobbyScene");

		}

		GUI.Label(new Rect(235,110,100,25) , "Name");
		Name = GUI.TextField(new Rect(235,130,100,25), Name);
	}
	
}