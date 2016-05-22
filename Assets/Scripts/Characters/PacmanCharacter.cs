using UnityEngine;
using System.Collections;


public class PacmanCharacter : Character {
  
  
	protected override void setAnimationState() {
    PacmanWalker pacman = walker as PacmanWalker;
    if (pacman.grounded()) {
      if (pacman.justLanded()) {
        //Debug.Log("just landed");
        anim.SetInteger("AnimState", 4);
      } else if (pacman.moving()) {
        //Debug.Log("moving");
        anim.SetInteger("AnimState", 1);
      } else {
        //Debug.Log("idle");
        anim.SetInteger("AnimState", 0);
      }
    } else {
      if (pacman.jumpingUp()) {
        //Debug.Log("jump up");
        anim.SetInteger("AnimState", 2);
      } else if (pacman.jumpingDown()) {
        //Debug.Log("jump down");
        anim.SetInteger("AnimState", 3);
      }
    }
	}


}
