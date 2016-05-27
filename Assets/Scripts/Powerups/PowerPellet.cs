using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PowerPellet : Powerup {
  

  public void Awake() {
    // effect
    action = delegate {

      // disable some components
      GetComponent<Collider>().enabled = false;
      GetComponent<Renderer>().enabled = false;
      ((Behaviour)gameObject.GetComponent("Halo")).enabled = false;

      // play sfx
      // picked-up animation?

      // add points
      GameManager gameManager = GameManager.Instance;
      gameManager.pacmanData.score += Score.PowerPellet;
      ++gameManager.levelData.pelletsEaten;

      // activate power mode?
      gameManager.powerPelletEaten();

    };
  }



}
