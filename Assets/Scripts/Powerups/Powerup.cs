using UnityEngine;
using System.Collections;
using UnityEngine.Events;



public abstract class Powerup : MonoBehaviour {


  public void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag == Tags.Pacman) {
      // execute effect
      startAction()();
      StartCoroutine(callLater(endAction(), length()));
    }
  }


  private IEnumerator callLater(UnityAction function, float seconds) {
    yield return new WaitForSecondsRealtime(seconds);
    function();
  }


  public abstract float length();             // duration of the effect, in seconds
  public abstract UnityAction startAction();  // to be called when the powerup is picked up
  public abstract UnityAction endAction();    // to be called length seconds after the powerup is picked up


}
