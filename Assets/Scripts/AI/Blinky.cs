using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Blinky : GhostAI {
  
  protected override Direction directionChase() {
    return directionToTarget(pacman.transform.position);
  }

}
