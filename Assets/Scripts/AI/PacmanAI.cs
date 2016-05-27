using UnityEngine;
using System.Collections;



public class PacmanAI : MonoBehaviour {



  private GameManager gameManager;


  public void Awake() {
    gameManager = GameManager.Instance;
    powerTime = 0f;
  }


  public float powerTime;


  // handle AI state 
  public void Update() {
    if (gameManager.paused || !gameManager.inGame) return;

    if (powerTime > 0f) powerTime -= Time.deltaTime;
    else powerTime = 0f;
  }

}
