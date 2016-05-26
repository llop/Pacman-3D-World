using UnityEngine;
using System.Collections;


[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (WaypointWalker))]
public abstract class Character : MonoBehaviour {



  protected GameManager gameManager;
	protected Animator anim;
  protected WaypointWalker walker;



  public void Awake() {
    gameManager = GameManager.Instance;
		anim = gameObject.GetComponent<Animator>();
    walker = gameObject.GetComponent<WaypointWalker>();
	}



  protected abstract void setAnimationState();

	public void Update() {
    //if (!gameManager.inGame) return;
    setAnimationState();
	}

}
