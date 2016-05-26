  using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {


  public string nextScene;


  GameManager gameManager;

  public void Awake() {
    gameManager = GameManager.Instance;
  }


  protected bool levelComplete() {
    //LevelData levelData = gameManager.levelData();
    //return levelData.pelletsEaten == levelData.pelletsTotal;

    return Input.GetKeyDown(KeyCode.A);
  }


  public void Update() {
    if (levelComplete()) gameManager.transitionToScene(nextScene);
  }


}
