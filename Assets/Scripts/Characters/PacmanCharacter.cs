using UnityEngine;
using System.Collections;


public enum PacmanState {
  Idle, 
  Run,
  JumpUp,
  JumpDown,
  Die
}


public class PacmanCharacter : Character {



  private PacmanState lastState = PacmanState.Idle;


  
	protected override void setAnimationState() {
    
    if (gameManager.pacmanData.alive) {

      if (lastState == PacmanState.Die) {
        anim.SetInteger("AnimState", 5);
        lastState = PacmanState.Idle;
      } else {
        PacmanWalker pacman = walker as PacmanWalker;
        if (pacman.grounded()) {
          if (pacman.justLanded()) {
            anim.SetInteger("AnimState", 4);
            lastState = PacmanState.Idle;
          } else if (pacman.moving()) {
            anim.SetInteger("AnimState", 1);
            lastState = PacmanState.Run;
          } else {
            anim.SetInteger("AnimState", 0);
            lastState = PacmanState.Idle;
          }
        } else {
          if (pacman.jumpingUp()) {
            anim.SetInteger("AnimState", 2);
            lastState = PacmanState.JumpUp;
          } else if (pacman.jumpingDown()) {
            anim.SetInteger("AnimState", 3);
            lastState = PacmanState.JumpDown;
          }
        }
      }
    } else {
      if (lastState == PacmanState.Idle) anim.SetInteger("AnimState", 8);
      else if (lastState == PacmanState.JumpUp) anim.SetInteger("AnimState", 6);
      else if (lastState == PacmanState.JumpDown) anim.SetInteger("AnimState", 7);
      else if (lastState == PacmanState.Run) anim.SetInteger("AnimState", 9);
      lastState = PacmanState.Die;
    }

	}


}
