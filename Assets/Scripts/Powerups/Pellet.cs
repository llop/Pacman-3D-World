using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Pellet : Powerup {
  public void Awake() {
	    action = delegate {
      // disable some components
      GetComponent<Collider>().enabled = false;
      GetComponent<Renderer>().enabled = false;
      ((Behaviour)gameObject.GetComponent("Halo")).enabled = false;

      // play sfx
      // picked-up animation?

      // add points
      GameManager gameManager = GameManager.Instance;
      gameManager.pacmanData.score += Score.Pellet;
      ++gameManager.levelData.pelletsEaten;

	  GameObject.FindGameObjectWithTag("Pacman").GetComponent<PacmanSounds>().pelletEaten();
    };
  }

}
