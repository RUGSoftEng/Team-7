using UnityEngine;
using System.Collections;
using Google.Cast.RemoteDisplay;

public class LobbyMember : MonoBehaviour {
    private Player myPlayer;
    private AudioClip bounceSound;
    private bool isBound = false;
    private static float spawnForce = 200f;

    public void Start() {
        Vector3 castCamPos = GameObject.Find("Cast Camera").transform.localPosition;
        castCamPos += new Vector3(0, 0, 5);
        this.transform.localPosition = castCamPos;
        Rigidbody2D physBod = this.GetComponent<Rigidbody2D>();
        physBod.AddForce(new Vector2(Random.Range(-spawnForce, spawnForce), 
                                     Random.Range(-spawnForce, spawnForce)));
    }

    // Checks if the bound player has been destroyed, kills itself if so.
    public void Update() {
        if (myPlayer == null && isBound) {
            Destroy(this.gameObject);
        }
    }

    // When it touches something.
    public void OnCollisionEnter2D(Collision2D collision) {
        if (CastRemoteDisplayManager.GetInstance().IsCasting()) {
            AudioSource.PlayClipAtPoint(bounceSound, Vector3.zero, 0.3f);
        }
    }

    // Binds a player to visualise with this LobbyMember.
    public void BindPlayer(Player player) {
        this.myPlayer = player;
        SetAnimal(player.animalName);
        SetBounceSound(player.animalName);
        SetName(player.playerName);
        this.isBound = true;
    }

    // The animal icon to display.
    private void SetAnimal(string animalName) {
        SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        Sprite animalSprite = Resources.Load<Sprite>("Animals/" + animalName);
        spriteRenderer.sprite = animalSprite;
    }

    // The animal sound to play on bounce.
    private void SetBounceSound(string animalName) {
        this.bounceSound = Resources.Load<AudioClip>("AnimalSounds/" + animalName);
    }

    // The player name to display.
    private void SetName(string playerName) {
        TextMesh textMesh = this.gameObject.GetComponentInChildren<TextMesh>();
        textMesh.text = playerName;
    }

}
