using UnityEngine;
using System.Collections;
using System.IO;

public class BtnSubmitScript : MonoBehaviour {
	
	/** Gloabl score */
	public int score = 0;
	
	/** Path of the english dictionary */
	const string ENGLISH_DICTIONARY_PATH = "Assets/Dictionaries/english_dictionary";
	
	/** Score of every english letter, taken from the official rules of english game
	 * @param c character to value 
	 * @return the official value of the letter c */
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
	
	/** Update the global score, with the score of submitted_word */
	void UpdateScore(string submitted_word){
		
		for (int i = 0; i < submitted_word.Length; i++) 
			score += CharacterValue(submitted_word[i]);
		return;
		
	}
	
	
	
	/** Check if the world is in the own dictionary 
	 *  @param submitted_word word to find in the dictionary 
	 * 	@param DICTIONARY path of dictionary of the language of the word to searchun
	 * 	@return true if the word exists in the dictionary, else return false */
	bool IsInDictionary(string submitted_word, string DICTIONARY){
		
        if (!File.Exists(DICTIONARY))
        {
            Debug.Log("File not found");
            return false;
        }
		
		using(StreamReader dictionary = new StreamReader(DICTIONARY)){
			
			string line;
			while ((line = dictionary.ReadLine()) != null)
				if (line.ToUpper() == submitted_word) return true;
			return false;
			
		}
		
	}
	
	/** Submit the word to check if exists in the dictionary */	
	public void OnClick(){

		string submitted_word = "";//Recive the submited word
		
		if (IsInDictionary(submitted_word, ENGLISH_DICTIONARY_PATH)){
			UpdateScore(submitted_word);
			/* We have to clear the used letter from the board */ 
			
		}
		
	}

}
