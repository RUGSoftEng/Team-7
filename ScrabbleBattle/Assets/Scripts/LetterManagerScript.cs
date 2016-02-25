using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LetterManagerScript : NetworkBehaviour {
	
	// Fixed number of shared letter, it is a minimum
	private const int MIN_NUM_SHARED_LETTERS = 10;
	
	// Number of shared letters for each player 
	private const int PROP_NUM_SHARED_LETTERS = 5;
	
	// Effective number of shared letters in the mainscreen
	public int num_shared_letters; 
	
	// Shared letter
	public Letter[] shared_letters;
	
	// Letter Tile
	public GameObject[] tiles_letter;
		
	public GameObject tile_letter;	
	
	/* Generate a semi-random character */
	char GetSharedCharacter (){
		
		// Stupid generator of character
		return (char)(Random.Range(0,25)+'A');
	
	}	
	
	/* Fill the board of the shared letters */	
	void SetSharedLetter () {
		
		for (int i = 0; i< num_shared_letters; i++){
			shared_letters[i] = new Letter();
			shared_letters[i].taken = false;
			shared_letters[i].character = GetSharedCharacter () ; 
		}
		
	}
	
	/* Draw the board of the shared letters  */
	void UpdateBoardSharedLetters () {
		
		for (int i = 0; i < num_shared_letters; i++)
			tiles_letter[i].transform.Find("Char")
					.gameObject.GetComponent<TextMesh>().text = shared_letters[i].character.ToString();
					
	}

	/* Create an object for each shared letter on the board */
	void CreateSharedLetter () {
		
		int cl = -1;
		for (int i = 0; i < num_shared_letters; i++) {
            tiles_letter[i] = GameObject.Instantiate(tile_letter,
													 new Vector3((i % 10)*2-9, cl*5+8, 0), 
													 Quaternion.identity) as GameObject;
			NetworkServer.Spawn(tiles_letter [i]);
			NetworkServer.AssignClientAuthority (tiles_letter [i]);
            tiles_letter[i].transform.Find("Char")
				.gameObject.GetComponent<TextMesh>().text = shared_letters[i].character.ToString();
            if ((i+1) % 10 == 0) cl--;
        }
        
	}

	// Use this for initialization
	public override void OnStartServer () {
		
		// To obtain from number of connected devices
		int players_number = 2;
	
		num_shared_letters = MIN_NUM_SHARED_LETTERS + PROP_NUM_SHARED_LETTERS * players_number;
		shared_letters = new Letter[num_shared_letters];
		tiles_letter = new GameObject[num_shared_letters];
		
		SetSharedLetter();
		CreateSharedLetter();
 
	}
	
}
