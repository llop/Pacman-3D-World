using UnityEngine;
using System.Collections;



public class BombExplosion : MonoBehaviour {


  public float duration;


	public void OnEnable() {
    Invoke("halt", duration);
	}

  public void OnDisable() {
    CancelInvoke();
  }


  private void halt() {
    gameObject.SetActive(false);
  }


  public void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag == Tags.Ghost) {
      // kill ghost
      GameManager.Instance.killGhost(other.gameObject.name);
    }
  }


}
