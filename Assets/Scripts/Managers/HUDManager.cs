using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;



public class HUDManager : MonoBehaviour {



  protected GameManager gameManager;
  protected Text livesText;
  protected Text scoreText;
  protected Text bombsText;
  protected Text pelletsText;



  public void Awake() {
    gameManager = GameManager.Instance;
  }


  public void Start() {
    livesText = GameObject.FindGameObjectWithTag(Tags.LivesText).GetComponent<Text>();
	scoreText = GameObject.FindGameObjectWithTag(Tags.ScoreText).GetComponent<Text>();
		bombsText = GameObject.Find("BombText").GetComponent<Text>();
    pelletsText = GameObject.FindGameObjectWithTag(Tags.PelletsText).GetComponent<Text>();
  }


  void OnGUI() {
    if (!gameManager.inGame) return;

    // pause?
    //if (Input.GetKeyDown(KeyCode.P)) {
    //  pauseMenu.open = true;
    //  gameManager.paused = true;
    //}

    // write data
    PacmanData pacmanData = gameManager.pacmanData;
    livesText.text = new string('c', pacmanData.lives);
    scoreText.text = "" + pacmanData.score;
	bombsText.text = "" + GameManager.Instance.pacmanData.bombs;
    LevelData levelData = gameManager.levelData;
    pelletsText.text = "" + levelData.pelletsEaten + '/' + levelData.pelletsTotal;
	}



  //------------------------------------------------------------------------------------
  // pause menu
  //------------------------------------------------------------------------------------

  public MenuAnimator pauseMenu;


  public void resume() {
    pauseMenu.open = false;
    gameManager.callLaterRealtime(delegate {
      gameManager.paused = false;
    }, .5f);
  }

  public void save() {
    Debug.Log("save");

  }

  public void quit() {
    pauseMenu.open = false;
    gameManager.callLaterRealtime(delegate {
      gameManager.paused = false;
      gameManager.transitionToScene(Tags.MenuScene);
    }, .5f);
  }


  //------------------------------------------------------------------------------------
  // ready watermark
  //------------------------------------------------------------------------------------

  public Text readyText;

  public void showReady(bool show) {
    readyText.enabled = show; 
  }


}
