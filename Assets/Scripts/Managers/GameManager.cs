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
    _inGame = false;
    _gameOver = true;
    _ghostScoreMultiplier = 1;
    _isPlayableLevel = false;

    // register scene changed callback:
    // when the scene is considered 'loaded', most of the shit in it isn't
    // so we need to use the 'active' callback instead
    SceneManager.activeSceneChanged += activeSceneChanged;

    resetPacmanData();        // 
    resetLevelData();         // should happen on every level load
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
    _pacmanData.bombs = 0;
  }



  //-------------------------------------------------------------------
  // stores the level's data
  //-------------------------------------------------------------------

  private LevelData _levelData = new LevelData();
  public LevelData levelData { get { return _levelData; } }

  private void resetLevelData() {
    _levelData.pelletsEaten = 0;
    _levelData.pelletsTotal = 0;

    _isPlayableLevel = false;
    _levelManager = null;
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

    _pacmanData.alive = false;
    _pacmanData.lives -= 1;

    if (_pacmanData.lives <= 0) doGameOver();
    else fadeInAndOut(
      delegate {
        // on fade out complete:
        // halt walkers 
        // if in the middle of a game, wait for user input to restart
        _inGame = false;
        if (_isPlayableLevel) _levelManager.waitForInput();

        // send everyone to their spawns (do a hard reset by calling the start method)
        foreach (KeyValuePair<string, GameObject> ghostEntry in _ghostsGOs) 
          ghostEntry.Value.GetComponent<WaypointWalker>().Start();
        _pacmanGO.GetComponent<WaypointWalker>().Start();

        // bring pacman back to life
        revivePacman();
      },
      delegate {
        // do nothing!
      });
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
    if (_pacmanGO.GetComponent<PacmanAI>().powerTime <= 0f) _ghostScoreMultiplier = 1;
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
  // ghost mode table
  //-------------------------------------------------------------------

  // Mode     Level 1     Levels 2-4     Levels 5+
  // ----------------------------------------------
  // Scatter  7           7              5
  // Chase    20          20             20
  // Scatter  7           7              5
  // Chase    20          20             20
  // Scatter  5           5              5
  // Chase    20          1033           1037
  // Scatter  5           1/60           1/60
  // Chase    indefinite  indefinite     indefinite

  public List<KeyValuePair<GhostAIState, float>> ghostModesForCurrentLevel() {
    if (_currentScene == Tags.Scene01) {
      List<KeyValuePair<GhostAIState, float>> ghostModesLevel1 = new List<KeyValuePair<GhostAIState, float>>();
      ghostModesLevel1.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Scatter, 7f));
      ghostModesLevel1.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Chase, 20f));
      ghostModesLevel1.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Scatter, 7f));
      ghostModesLevel1.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Chase, 20f));
      ghostModesLevel1.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Scatter, 5f));
      ghostModesLevel1.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Chase, 20f));
      ghostModesLevel1.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Scatter, 5f));
      ghostModesLevel1.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Chase, -1f));
      return ghostModesLevel1;
    } else if (_currentScene == Tags.Scene02) {
      List<KeyValuePair<GhostAIState, float>> ghostModesLevel2To4 = new List<KeyValuePair<GhostAIState, float>>();
      ghostModesLevel2To4.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Scatter, 7f));
      ghostModesLevel2To4.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Chase, 20f));
      ghostModesLevel2To4.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Scatter, 7f));
      ghostModesLevel2To4.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Chase, 20f));
      ghostModesLevel2To4.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Scatter, 5f));
      ghostModesLevel2To4.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Chase, 1033f));
      ghostModesLevel2To4.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Scatter, 1f / 60f));
      ghostModesLevel2To4.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Chase, -1f));
      return ghostModesLevel2To4;
    } else if (_currentScene == Tags.Scene03) {
      List<KeyValuePair<GhostAIState, float>> ghostModesLevel5Plus = new List<KeyValuePair<GhostAIState, float>>();
      ghostModesLevel5Plus.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Scatter, 5f));
      ghostModesLevel5Plus.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Chase, 20f));
      ghostModesLevel5Plus.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Scatter, 5f));
      ghostModesLevel5Plus.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Chase, 20f));
      ghostModesLevel5Plus.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Scatter, 5f));
      ghostModesLevel5Plus.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Chase, 1037f));
      ghostModesLevel5Plus.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Scatter, 1f / 60f));
      ghostModesLevel5Plus.Add(new KeyValuePair<GhostAIState, float>(GhostAIState.Chase, -1f));
      return ghostModesLevel5Plus;
    }
    return null;
  }

  public float ghostFrightenedTimeForCurrentLevel() {
    if (_currentScene == Tags.Scene01) return 5f;
    else if (_currentScene == Tags.Scene02) return 5f;
    else if (_currentScene == Tags.Scene03) return 3f;
    return 5f;
  }


  //-------------------------------------------------------------------
  // character speed multipliers
  //-------------------------------------------------------------------

  public float pacmanSpeedMultiplier(float powerTime) {
    float multiplier = 1f;
    if (_currentScene == Tags.Scene01) multiplier = .8f;
    else if (_currentScene == Tags.Scene02) multiplier = .9f;
    else if (_currentScene == Tags.Scene03) multiplier = 1f;
    if (powerTime > 0f) multiplier = Mathf.Clamp01(multiplier + .5f); 
    return multiplier;
  }

  public float ghostSpeedMultiplier(GhostAIState ghostState) {
    if (ghostState != GhostAIState.Dead) {
      if (_currentScene == Tags.Scene01) return ghostState == GhostAIState.Frightened ? .4f : .75f;
      if (_currentScene == Tags.Scene02) return ghostState == GhostAIState.Frightened ? .45f : .85f;
      if (_currentScene == Tags.Scene03) return ghostState == GhostAIState.Frightened ? .5f : .95f;
    }
    return 1f;
  }



  //-------------------------------------------------------------------
  // if the scene is a playable level, we can access its level manager
  //-------------------------------------------------------------------

  private bool _isPlayableLevel;
  private LevelManager _levelManager;

  public LevelManager levelManager { get { return _levelManager; } }

  public void registerPlayableLevel(LevelManager levelManager) {
    _isPlayableLevel = true;
    _levelManager = levelManager;
  }


  //-------------------------------------------------------------------
  // can we play?
  //-------------------------------------------------------------------

  private bool _inGame;
  public bool inGame { 
    get { return _inGame; } 
    set { _inGame = value; }
  }
    

  //-------------------------------------------------------------------
  // is this game over? (ie. did pacman lose all his lives?)
  //-------------------------------------------------------------------

  private bool _gameOver;

  public bool gameOver {
    get { return _gameOver; }
  }


  //-------------------------------------------------------------------
  // call these to finish the game
  //-------------------------------------------------------------------

  public void doGameComplete() {
    transitionToScene(Tags.GameCompleteScene);
    // after game complete scene, must check for hi score!
  }

  public void doGameOver() {
    _gameOver = true;
    if (HiScoreManager.Instance.isHiScore(_pacmanData.score)) transitionToScene(Tags.HiScoreScene);
    else transitionToScene(Tags.GameOverScene);
  }



  //-------------------------------------------------------------------
  // call this to start a game
  //-------------------------------------------------------------------

  private void initGame() {
    resetPacmanData();        // 
    resetLevelData();         // should happen on every level load
    _gameOver = false;
  }

  public void doStartGame() {
    initGame();
    transitionToScene(Tags.Scene01);
  }

  public void doContinueGame() {
    initGame();
    transitionToScene(_lastPlayedLevel);
  }


  //-------------------------------------------------------------------
  // power pellet mode activation/deactivation
  //-------------------------------------------------------------------

  public void powerPelletEaten() {
    _pacmanGO.GetComponent<PacmanAI>().powerTime = ghostFrightenedTimeForCurrentLevel();

    // scaredy ghosts!
    foreach (KeyValuePair<string, GameObject> ghostEntry in _ghostsGOs) 
      ghostEntry.Value.GetComponent<GhostAI>().scare();
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

  private string _lastPlayedLevel;

  private string _currentScene;
  public string currentScene { get { return _currentScene; } }


  public void transitionToScene(string sceneName) {
    GameObject pacmanGO = GameObject.FindGameObjectWithTag("Pacman");
    if (pacmanGO) pacmanGO.GetComponent<PacmanSounds>().levelDone();

    _inGame = false;
    if (_isPlayableLevel) {
      // halt walkers
      foreach (KeyValuePair<string, GameObject> ghostEntry in _ghostsGOs) 
        ghostEntry.Value.GetComponent<WaypointWalker>().halt();
      _pacmanGO.GetComponent<WaypointWalker>().halt();
    }

    fadeInAndOut(
      delegate {

        // reset leveldata
        resetLevelData();

        // load another scene
        if (_currentScene != null && _currentScene.StartsWith(Tags.LevelPrefix)) _lastPlayedLevel = _currentScene;
        _currentScene = sceneName;
        SceneManager.LoadScene(_currentScene);

      }, 
      delegate {

      });
  }

  // we need to manually set a timeout for characters to start moving in the scene
  private void activeSceneChanged(Scene old, Scene justActivated) {
    //bool newInGame = _currentScene != Tags.MenuScene;
    //if (newInGame) callLater(delegate {
        // we can play as long as we aren't in the main menu
    //_inGame = newInGame;
    //}, 2f);
    //else _inGame = newInGame;

  }



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
