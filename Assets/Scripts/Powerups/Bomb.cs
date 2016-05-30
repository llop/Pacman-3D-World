using UnityEngine;
using System.Collections;



public class Bomb : Powerup {
  

  public void Awake() {
    action = delegate {
      
      // play sfx
      // picked-up animation?

      // add 3 bombs
      GameManager gameManager = GameManager.Instance;
      gameManager.pacmanData.bombs += 3;

      Destroy(transform.gameObject);
    };
  }



}
