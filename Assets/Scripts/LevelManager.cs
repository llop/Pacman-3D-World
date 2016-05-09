using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {


	public Button playButton;
	public Button quitButton;

	// Use this for initialization
	void Start () {
		playButton = playButton.GetComponent<Button>();
		quitButton = quitButton.GetComponent<Button>();
	}

	public void playClicked() {
		SceneManager.LoadScene("Scenes/Scene01");
	}

	public void quitClicked() {
		Application.Quit();
	}

}
