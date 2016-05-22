using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Pellet : Powerup {
  


  private float _length;
  private UnityAction _action;
  private UnityAction _endAction;

  public override float length() { return _length; }
  public override UnityAction startAction() { return _action; }
  public override UnityAction endAction() { return _endAction; }



  public void Awake() {
    // effect
    _length = 0f;
    _action = delegate {
      
      // disable some components
      GetComponent<SphereCollider>().enabled = false;
      GetComponent<MeshRenderer>().enabled = false;
      ((Behaviour)gameObject.GetComponent("Halo")).enabled = false;

      // play sfx
      // picked-up animation?

      // add points
      PacmanData pacmanData = GameManager.Instance.pacmanData();
      pacmanData.addScore(Score.Pellet);

    };
    _endAction = delegate {
      Destroy(gameObject);
    };
  }



}
