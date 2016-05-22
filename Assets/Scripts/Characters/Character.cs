using UnityEngine;
using System.Collections;


[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (WaypointWalker))]
public abstract class Character : MonoBehaviour {


  
	protected Animator anim;
  protected WaypointWalker walker;



	void Start() {
		anim = gameObject.GetComponent<Animator>();
    walker = gameObject.GetComponent<WaypointWalker>();
	}



  protected abstract void setAnimationState();

	void Update() {
    setAnimationState();
	}

}
