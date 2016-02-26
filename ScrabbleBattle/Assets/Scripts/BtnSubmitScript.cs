using UnityEngine;
using System.Collections;
using System.IO;

public class BtnSubmitScript : MonoBehaviour {
	
	/* Move this function outside */
	public int score = 0;
	
	const string ENGLISH_DICTIONARY_PATH = "Assets/Dictionaries/english_dictionary";
	
	/* Move this function outside */
	int CharacterValue(char c){
		
		switch(c){
		
			case 'A':
			case 'E':
			case 'I':
			case 'O':
			case 'U':
			case 'N':
			case 'R':
			case 'T':
			case 'L':
			case 'S': return 1;
			
			case 'D':
			case 'G': return 2;
			
			case 'B':
			case 'C':
			case 'M':
			case 'P': return 3;
			
			case 'F':
			case 'H':
			case 'V':
			case 'W':
			case 'Y': return 4;
			
			case 'K': return 5;
			
			case 'J':
			case 'X': return 8;
			
			case 'Q':
			case 'Z': return 10;
			
			default: 
				return 0;
			
		}
		
	}
	
	/* Move this function outside */
	void UpdateScore(string submitted_word){
		
		for (int i = 0; i < submitted_word.Length; i++) 
			score += CharacterValue(submitted_word[i]);
		return;
		
	}
	
	
	
	/* Move this function outside */
	bool IsInDictionary(string submitted_word){
		
        if (!File.Exists(ENGLISH_DICTIONARY_PATH))
        {
            Debug.Log("File not found");
            return false;
        }
		
		using(StreamReader dictionary = new StreamReader(ENGLISH_DICTIONARY_PATH)){
			
			string line;
			while ((line = dictionary.ReadLine()) != null)
				if (line.ToUpper() == submitted_word) return true;
			return false;
			
		}
		
	}
		
	public void OnClick(){

		string submitted_word = "";//Recive the submited word
		
		if (IsInDictionary(submitted_word)){
			UpdateScore(submitted_word);
			/* We have to clear the used letter from the board */ 
			
		}
		
	}

}
