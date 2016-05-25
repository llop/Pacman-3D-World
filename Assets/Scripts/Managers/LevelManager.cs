using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {


  public string nextScene;


  protected bool levelComplete() {
    // should evaluate eaten pellets == total pellets
    return Input.GetKeyDown(KeyCode.A);
  }


  public void Update() {
    if (levelComplete()) GameManager.Instance.transitionToScene(nextScene);
  }


}
