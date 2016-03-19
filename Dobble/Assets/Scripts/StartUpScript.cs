using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class StartUpScript : MonoBehaviour {

	public string Name = "";
	public FileInfo NameFile;
	// Use this for initialization

	void Start () {
		NameFile = new FileInfo(Application.persistentDataPath +  "\\" + "NameSave.txt");
		this.Name = LoadName();
	}

	void OnGUI(){

		if (GUI.Button(new Rect(10,230,120,25), "Change Name")){
			DeleteAll();
			if(NameFile.Exists){
				NameFile.Delete();
			}
			Application.LoadLevel ("LoginScene");
		}


		GUI.Label(new Rect(10,200,100,25) , Name);
		// Name = GUI.TextField(new Rect(235,130,100,25), Name);
	}
	
	string LoadName(){
		if (NameFile.Exists){
			StreamReader r = File.OpenText(Application.persistentDataPath + "\\" + "NameSave.txt");
     		string info = r.ReadToEnd();
     		r.Close();
     		return info;
     	} else {
     		return "No name";
     	}
	}

	public void DeleteAll(){
         foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
             Destroy(o);
         }
    }
}
