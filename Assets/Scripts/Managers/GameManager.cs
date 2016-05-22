using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameManager : MonoBehaviour {


  //-------------------------------------------------------------------
  // game manager singleton
  //-------------------------------------------------------------------
  private static GameManager _instance;

  public static GameManager Instance { 
    get {
      if (_instance == null) {
        GameObject obj = new GameObject("GameManager");
        //DontDestroyOnLoad(obj);
        obj.AddComponent<GameManager>();
      }
      return _instance;
    }
  }



  //-------------------------------------------------------------------
  // 
  //-------------------------------------------------------------------

  private PacmanData _pacmanData;
  public PacmanData pacmanData() { return _pacmanData; }


  //-------------------------------------------------------------------
  // 
  //-------------------------------------------------------------------

  private int _activePowerPellets;
  public void powerPelletEffectStart() { 
    ++_activePowerPellets;

    if (_activePowerPellets == 1) {
      // scaredy ghosts!
      GameObject[] ghosts = GameObject.FindGameObjectsWithTag(Tags.Ghost);
      foreach (GameObject ghostObject in ghosts) {
        GhostAI ghost = ghostObject.GetComponent<GhostAI>();
        if (ghost.state == GhostAIState.Chase || ghost.state == GhostAIState.Scatter) 
          ghost.state = GhostAIState.Frightened;
      }
    }
  }
  public void powerPelletEffectEnd() { 
    --_activePowerPellets; 

    if (_activePowerPellets == 0) {
      // ghosts go back to patrol mode
      GameObject[] ghosts = GameObject.FindGameObjectsWithTag(Tags.Ghost);
      foreach (GameObject ghostObject in ghosts) {
        GhostAI ghost = ghostObject.GetComponent<GhostAI>();
        if (ghost.state == GhostAIState.Frightened) ghost.state = GhostAIState.Chase;
      }
    }
  }



  //-------------------------------------------------------------------
  // 
  //-------------------------------------------------------------------

  public void Awake() {
    _instance = this;

    startNewGame();

    _activePowerPellets = 0;                    // should happen on every level load
  }



  //-------------------------------------------------------------------
  // 
  //-------------------------------------------------------------------

  public void startNewGame() {
    _pacmanData = new PacmanData(false, 3, 0);  // should happen every new game

    //loadScene("Scene01");
  }


  //-------------------------------------------------------------------
  // 
  //-------------------------------------------------------------------

  public void loadScene(string sceneName) {
    SceneManager.LoadScene(sceneName);
  }

}
