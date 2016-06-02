using UnityEngine;
using System.Collections;



// every playable level must have one of these
// handles when to move on to the next scene
// and interaction with the HUD
public class LevelManager : MonoBehaviour {


  public string nextScene;


  private GameManager gameManager;
  private HUDManager hudManager;

  public ObjectPool objectPool;


  public void Start() {
    gameManager = GameManager.Instance;
    gameManager.registerPlayableLevel(this);
    hudManager = GameObject.FindGameObjectWithTag(Tags.HUD).GetComponent<HUDManager>();
    objectPool = GetComponent<ObjectPool>();
  }



  public bool playing = false;

  private void checkForStart() {
    if (playing || gameManager.paused) return;

    if (Input.GetKeyDown(KeyCode.UpArrow)
        || Input.GetKeyDown(KeyCode.DownArrow)
        || Input.GetKeyDown(KeyCode.LeftArrow)
        || Input.GetKeyDown(KeyCode.RightArrow)
        || Input.GetKeyDown(KeyCode.X)) {
      hudManager.showReady(false);
      playing = true;
      gameManager.inGame = true;
    };
  }


  public void waitForInput() {
    playing = false;
    hudManager.showReady(true);
  }


  protected bool levelComplete() {
    // skip to next level by pressing 'A'
    if (Input.GetKeyDown(KeyCode.A)) return true;

    LevelData levelData = gameManager.levelData;
		return levelData.pelletsEaten == levelData.pelletsTotal;
  }


  public void Update() {
    checkForStart();
		if (levelComplete ()) {
			GameManager.Instance.transitionToScene (nextScene);
		}
  }


}
