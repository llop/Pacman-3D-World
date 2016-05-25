using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;



public class HUDManager : MonoBehaviour {



  protected GameManager gameManager;
  protected Text livesText;
  protected Text scoreText;
  protected Text pelletsText;



  public void Awake() {
    gameManager = GameManager.Instance;
  }


  public void Start() {
    //livesText = transform.Find(Tags.Lives).gameObject.GetComponent<Text>();
    //scoreText = transform.Find(Tags.Score).gameObject.GetComponent<Text>();
    //pelletsText = transform.Find(Tags.Pellets).gameObject.GetComponent<Text>();
  }


  void Update() {
    if (!gameManager.inGame) return;

    // pause?
    if (Input.GetKeyDown(KeyCode.P)) {
      pauseMenu.open = true;
      gameManager.paused = true;
    }

    //livesText.text = "Lives " + gameManager.pacmanData().lives();
    //scoreText.text = "Score " + gameManager.pacmanData().score();
    //pelletsText.text = "Pellets " + 0;//gameManager.pacmanData().pellets();
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


}
