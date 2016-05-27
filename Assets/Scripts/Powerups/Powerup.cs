using UnityEngine;
using UnityEngine.Events;
using System.Collections;



public abstract class Powerup : MonoBehaviour {


  public UnityAction action;  // to be called when the powerup is picked up


  public void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag == Tags.Pacman) action();  // execute effect
  }


}
