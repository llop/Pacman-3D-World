using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



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
  // init everything here
  //-------------------------------------------------------------------

  public void Awake() {
    _instance = this;
    _paused = false;
    _inGame = true; // false;

    // register scene changed callback:
    // when the scene is considered 'loaded', most of the shit in it isn't
    // so we need to use the 'active' callback instead
    SceneManager.activeSceneChanged += activeSceneChanged;

    resetPacmanData();        // 
    resetLevelData();         // should happen on every level load
    _activePowerPellets = 0;
  }



  //-------------------------------------------------------------------
  // stores the player's data
  //-------------------------------------------------------------------

  private PacmanData _pacmanData = new PacmanData();
  public PacmanData pacmanData { get { return _pacmanData; } }

  private void resetPacmanData() {
    _pacmanData.alive = true;
    _pacmanData.lives = 3;
    _pacmanData.score = 0;
  }



  //-------------------------------------------------------------------
  // stores the level's data
  //-------------------------------------------------------------------

  private LevelData _levelData = new LevelData();
  public LevelData levelData { get { return _levelData; } }

  private void resetLevelData() {
    _levelData.pelletsEaten = 0;
    _levelData.pelletsTotal = 0;
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
  // pacman and ghosts game objects
  //-------------------------------------------------------------------

  private GameObject _pacmanGO;
  private Dictionary<string, GameObject> _ghostsGOs = new Dictionary<string, GameObject>();


  public void registerPacman(GameObject obj) {
    _pacmanGO = obj;
  }

  public void registerGhost(string ghostName, GameObject obj) {
    _ghostsGOs[ghostName] = obj;
  }


  //-------------------------------------------------------------------
  // activate/deactivate colliders
  //-------------------------------------------------------------------

  private void pacmanGhostsIgnoreCollision(bool ignore) {
    Collider pacmanCollider = _pacmanGO.GetComponent<Collider>();
    foreach (KeyValuePair<string, GameObject> ghostEntry in _ghostsGOs) 
      Physics.IgnoreCollision(ghostEntry.Value.GetComponent<Collider>(), pacmanCollider, ignore);
  }




  //-------------------------------------------------------------------
  // kill pacman
  //-------------------------------------------------------------------

  public void killPacman() {
    if (_pacmanGO == null) return;

    pacmanGhostsIgnoreCollision(true);

    // needs to: play sfx
    //           play pacman dead animation

    if (!_pacmanData.alive) return;
    _pacmanData.alive = false;
    _pacmanData.lives -= 1;

    if (_pacmanData.lives <= 0) {
      // if pacman had zero lives,
      //   if current score is a hi-score, 
      //     ask user for initials to store his score
      //     game over screen
      //   else just go to game over screen

      transitionToScene(Tags.MenuScene);

    } else {
      fadeInAndOut(
        delegate {
          // on fade out complete, halt walkers 
          _inGame = false;

          // send everyone to their spawns (do a hard reset by calling the start method)
          foreach (KeyValuePair<string, GameObject> ghostEntry in _ghostsGOs) 
            ghostEntry.Value.GetComponent<WaypointWalker>().Start();
          _pacmanGO.GetComponent<WaypointWalker>().Start();

          // bring pacman back to life
          revivePacman();
        },
        delegate {
          // ok, do your thang
          _inGame = true;
        });
      
    }
  }

  public void revivePacman() {
    _pacmanData.alive = true;
    if (_pacmanGO == null) return;
    pacmanGhostsIgnoreCollision(false);
  }


  //-------------------------------------------------------------------
  // kill a ghost
  //-------------------------------------------------------------------

  private ulong _ghostScoreMultiplier;

  public void killGhost(string ghostName) {
    GameObject ghostGO = _ghostsGOs[ghostName];
    if (ghostGO == null) return;

    // kill ghost
    GhostAI ai = ghostGO.GetComponent<GhostAI>();
    ai.state = GhostAIState.Dead;

    // deactivate collisions
    Collider ghostCollider = ghostGO.GetComponent<Collider>();
    Collider pacmanCollider = _pacmanGO.GetComponent<Collider>();
    Physics.IgnoreCollision(ghostCollider, pacmanCollider, true);

    // give points to pacman
    _pacmanData.score += Score.Ghost * _ghostScoreMultiplier;
    _ghostScoreMultiplier *= Score.GhostEatenMultiplierFactor;
  }

  public void reviveGhost(string ghostName) {
    GameObject ghostGO = _ghostsGOs[ghostName];
    if (ghostGO == null) return;

    // reactivate collisions
    Collider ghostCollider = ghostGO.GetComponent<Collider>();
    Collider pacmanCollider = _pacmanGO.GetComponent<Collider>();
    Physics.IgnoreCollision(ghostCollider, pacmanCollider, false);

    // reset AI state? 
    ghostGO.GetComponent<GhostAI>().state = GhostAIState.Scatter;
  }



  //-------------------------------------------------------------------
  // power pellet mode activation/deactivation
  //-------------------------------------------------------------------

  public void startGame() {
    resetPacmanData();        // 
    resetLevelData();         // should happen on every level load
    _activePowerPellets = 0;
    transitionToScene(Tags.Scene1);
  }


  //-------------------------------------------------------------------
  // power pellet mode activation/deactivation
  //-------------------------------------------------------------------

  private int _activePowerPellets;
  public void powerPelletEffectStart() { 
    ++_activePowerPellets;

    if (_activePowerPellets == 1) {
      
      // init score multiplier
      _ghostScoreMultiplier = 1;

      // scaredy ghosts!
      foreach (KeyValuePair<string, GameObject> ghostEntry in _ghostsGOs) {
        GhostAI ai = ghostEntry.Value.GetComponent<GhostAI>();
        if (ai.state == GhostAIState.Chase || ai.state == GhostAIState.Scatter) 
          ai.state = GhostAIState.Frightened;
      }
    }
  }

  public void powerPelletEffectEnd() { 
    --_activePowerPellets; 

    if (_activePowerPellets == 0) {
      // reset score multiplier
      _ghostScoreMultiplier = 1;

      // ghosts go back to patrol mode
      foreach (KeyValuePair<string, GameObject> ghostEntry in _ghostsGOs) {
        GhostAI ai = ghostEntry.Value.GetComponent<GhostAI>();
        if (ai.state == GhostAIState.Frightened) ai.state = GhostAIState.Chase;
      }
    }
  }



  //-------------------------------------------------------------------
  // fade in-out conveniency method
  //-------------------------------------------------------------------

  private void fadeInAndOut(UnityAction onFadeOut, UnityAction onFadeIn) {
    GameObject obj = new GameObject();
    obj.AddComponent<Fader>();
    Fader fader = obj.GetComponent<Fader>();
    fader.onFadeOut = onFadeOut;
    fader.onFadeIn = onFadeIn;
    fader.start = true;
  }


  //-------------------------------------------------------------------
  // fade in/out between scenes
  //-------------------------------------------------------------------

  private string _currentScene;
  public string currentScene { get { return _currentScene; } }

  public void transitionToScene(string sceneName) {
    _inGame = false;

    fadeInAndOut(
      delegate {

        // reset leveldata
        resetLevelData();
        // load another scene
        _currentScene = sceneName;
        SceneManager.LoadScene(_currentScene);

      }, 
      delegate {

      });
  }

  // we need to manually set a timeout for characters to start moving in the scene
  private void activeSceneChanged(Scene old, Scene justActivated) {
    bool newInGame = _currentScene != Tags.MenuScene;
    if (newInGame) callLater(delegate {
        // we can play as long as we aren't in the main menu
        _inGame = newInGame;
      }, 2f);
    else _inGame = newInGame;
  }



  //-------------------------------------------------------------------
  // can we play?
  //-------------------------------------------------------------------

  private bool _inGame;
  public bool inGame { get { return _inGame; } }



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
