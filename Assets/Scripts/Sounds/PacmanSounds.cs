using UnityEngine;
using System.Collections;


[RequireComponent (typeof (AudioSource))]
public class PacmanSounds : MonoBehaviour {

	private AudioSource player;
	public AudioClip chomp;
	public AudioClip death;
	public AudioClip ghost;
	public AudioClip win;
	private float lastTimeEaten;
	// Use this for initialization
	void Start () {
		lastTimeEaten = 0;
		player = GetComponent<AudioSource> ();
		player.loop = false;
	}

	void Update() {
		if (player.clip == chomp && Time.time - lastTimeEaten > 0.3f) {
			player.loop = false;
		}
		if (player.clip == win && !player.isPlaying) {
			GameManager.Instance.transitionToScene (GameManager.Instance.levelManager.nextScene);
		}
	}
	
	// Update is called once per frame
	public void pelletEaten () {
		lastTimeEaten = Time.time;
		if (!player.isPlaying) {
			player.clip = chomp;
			player.Play ();
			player.loop = true;
		}
	}

	public void pacmanDeath () {
		if (player.clip != death) {
			player.clip = death;
			player.Play ();
			player.loop = false;
		}
	}

	public void ghostEaten () {
			player.clip = ghost;
			player.Play ();
			player.loop = false;
	}

	public void levelDone() {
		if (player.clip != win) {
			player.clip = win;
			player.Play ();
			player.loop = false;
			GameManager.Instance.levelManager.playing = false;
		}
	}
}
