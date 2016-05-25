using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;



public class GameManager : MonoBehaviour {


  //-------------------------------------------------------------------
  // game manager singleton
  //-------------------------------------------------------------------
  private static GameManager _instance;

  public static GameManager Instance { 
    get {
      if (_instance == null) {
        GameObject obj = new GameObject("GameManager");
        DontDestroyOnLoad(obj);
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
  // conveniency methods to do stuff later
  //-------------------------------------------------------------------

  public void callLater(UnityAction function, float seconds) {
    StartCoroutine(callLaterInternal(function, seconds));
  }

  public void callLaterRealtime(UnityAction function, float seconds) {
    StartCoroutine(callLaterRealtimeInternal(function, seconds));
  }

  private IEnumerator callLaterInternal(UnityAction function, float seconds) {
    yield return new WaitForSeconds(seconds);
    function();
  }

  private IEnumerator callLaterRealtimeInternal(UnityAction function, float seconds) {
    yield return new WaitForSecondsRealtime(seconds);
    function();
  }



  //-------------------------------------------------------------------
  // 
  //-------------------------------------------------------------------

  public void Awake() {
    SceneManager.activeSceneChanged += activeSceneChanged;

    _instance = this;
    _paused = false;

    inGame = true;

    startNewGame();

    _activePowerPellets = 0;                    // should happen on every level load
  }



  //-------------------------------------------------------------------
  // 
  //-------------------------------------------------------------------

  public void startNewGame() {
    _pacmanData = new PacmanData(false, 3, 0);  // should happen every new game
  }



  //-------------------------------------------------------------------
  // fade in/out between scenes
  //-------------------------------------------------------------------

  protected string currentScene;

  public void transitionToScene(string sceneName) {
    inGame = false;

    GameObject obj = new GameObject();
    obj.AddComponent<SceneFader>();
    SceneFader fader = obj.GetComponent<SceneFader>();
    fader.onFadeOut = delegate {
      // load another scene
      SceneManager.LoadScene(sceneName);
      currentScene = sceneName;
    };
    fader.onFadeIn = delegate {
      // do nothing?
    };
    fader.start = true;

  }

  private void activeSceneChanged(Scene old, Scene justActivated) {
    bool newInGame = currentScene != Tags.MenuScene;
    if (newInGame) callLater(delegate {
        // we can play as long as we aren't in the main menu
        inGame = newInGame;
      }, 2f);
    else inGame = newInGame;
  }

  //-------------------------------------------------------------------
  // can we play?
  //-------------------------------------------------------------------

  public bool inGame { get; protected set; }



  //-------------------------------------------------------------------
  // pause by setting timeScale
  //-------------------------------------------------------------------

  private bool _paused;
  public bool paused {
    get { return _paused; }
    set {
      _paused = value;
      if (_paused) Time.timeScale = 0f;
      else Time.timeScale = 1f;
    }
  }


}
