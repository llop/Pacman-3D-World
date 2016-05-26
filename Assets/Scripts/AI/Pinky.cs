using UnityEngine;
using System.Collections;

public class Pinky : GhostAI {



  //------------------------------------------------------------------------------------------------
  // we need to be able to access pacman's direction
  //------------------------------------------------------------------------------------------------

  WaypointWalker pacmanWalker;

  public override void awake() {
    pacmanWalker = pacman.GetComponent<WaypointWalker>();
  }



  //------------------------------------------------------------------------------------------------
  // pinky’s target tile in chase mode is determined by looking at pacman’s current position and orientation, 
  // and selecting the location four tiles straight ahead of pacman. 
  // when pacman is facing upwards, an overflow error in the game’s code causes pinky’s target tile 
  // to actually be set as four tiles ahead of pacman and four tiles to the left of him (LOL)
  //------------------------------------------------------------------------------------------------

  protected override Direction directionChase() {

    float numTiles = 4f;
    Transform pacmanTransform = pacman.transform;
    Vector3 target = pacmanTransform.position;
    Direction pacmanDirection = pacmanWalker.direction();
    if (pacmanDirection == Direction.Front) {
      target += pacmanTransform.forward.normalized * numTiles;
      target -= pacmanTransform.right.normalized * numTiles;
    }
    if (pacmanDirection == Direction.Back) target -= pacmanTransform.forward.normalized * numTiles;
    if (pacmanDirection == Direction.Left) target -= pacmanTransform.right.normalized * numTiles;
    if (pacmanDirection == Direction.Right) target += pacmanTransform.right.normalized * numTiles;

    return directionToTarget(target);
  }


}
