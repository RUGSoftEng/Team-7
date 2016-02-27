using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LetterManagerScript : NetworkBehaviour {
	
	/** Minimum fixed number of shared letters */
	private const int MIN_NUM_SHARED_LETTERS = 10;
	
	/** Number of shared letters for player who enjoys the match */
	private const int PROP_NUM_SHARED_LETTERS = 5;
	
	/** Effective number of shared letters in the mainscreen, calculated from:
	 *  num_shared_letters = MIN_NUM_SHARED_LETTERS + PROP_NUM_SHARED_LETTERS * number_players*/
	public int num_shared_letters; 
	
	/** Abstraction of the shared letters, used for the communication
	 * 	between clients and server and to store the shared avaible letters */
	public Letter[] shared_letters;
	
	/** Container of every GameObject of the shared letters, drawn in the
	 * 	main screen */
	public GameObject[] tiles_letter;
	
	/** Letter tile to draw every single letter, and put them in tiles_letter */	
	public GameObject tile_letter;	
	
	/** Generate a semi-random character to fill the missing shared letter
	 * 	in the board */
	char GetSharedCharacter (){
		
		// Stupid generator of character
		return (char)(Random.Range(0,25)+'A');
	
	}	
	
	/** Fill the whole abstraction of the shared letters*/	
	void SetSharedLetter () {
		
		for (int i = 0; i< num_shared_letters; i++){
			shared_letters[i] = new Letter();
			shared_letters[i].taken = false;
			shared_letters[i].character = GetSharedCharacter () ; 
		}
		
	}
	
	/** Link the GameObject letter to its abstraction, in order to draw
	 * 	on the screen the updated shared letters */
	void UpdateBoardSharedLetters () {
		
		for (int i = 0; i < num_shared_letters; i++)
			tiles_letter[i].transform.Find("Char")
					.gameObject.GetComponent<TextMesh>().text = shared_letters[i].character.ToString();
					
	}

	/** Create a GameObject from letter_tile, in order to draw the initial 
	 * 	game board */
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

	/** Initialization of shared letters board, which includes:
	 * 	- Draw the whole shred letters board on the screen;
	 * 	- Fill every letters on the screen, with the support of its
	 * 	  abstraction */
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
