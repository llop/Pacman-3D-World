using UnityEngine;
using UnityEngine.Events;
using System.Collections;



public abstract class Powerup : MonoBehaviour {


  public void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag == Tags.Pacman) {
      // execute effect
      startAction()();
      GameManager.Instance.callLater(endAction(), length());
    }
  }


  public abstract float length();             // duration of the effect, in seconds
  public abstract UnityAction startAction();  // to be called when the powerup is picked up
  public abstract UnityAction endAction();    // to be called length seconds after the powerup is picked up


}
