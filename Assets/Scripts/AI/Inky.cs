using UnityEngine;
using System.Collections;

public class Inky : GhostAI {



  //------------------------------------------------------------------------------------------------
  // we need to be able to access pacman's direction and blinky's position
  //------------------------------------------------------------------------------------------------

  WaypointWalker pacmanWalker;
  private GameObject blinky;


  public override void awake() {
    pacmanWalker = pacman.GetComponent<WaypointWalker>();
    blinky = Object.FindObjectOfType<Blinky>().gameObject;
  }



  //------------------------------------------------------------------------------------------------
  // inky actually uses both pacman’s position/facing as well as blinky’s (the red ghost’s). 
  // to locate inky’s target, we first start by selecting 
  // the position two tiles in front of pacman in his current direction of travel
  // from there, imagine drawing a vector from blinky’s position to this tile, 
  // and then doubling the length of the vector. 
  // The tile that this new, extended vector ends on will be inky’s actual target.
  //------------------------------------------------------------------------------------------------

  protected override Direction directionChase() {

    float numTiles = 2f;
    Transform pacmanTransform = pacman.transform;
    Vector3 pacmanTarget = pacmanTransform.position;
    Direction pacmanDirection = pacmanWalker.getDirection();
    if (pacmanDirection == Direction.Front) {
      pacmanTarget += pacmanTransform.forward.normalized * numTiles;
      pacmanTarget -= pacmanTransform.right.normalized * numTiles;
    }
    if (pacmanDirection == Direction.Back) pacmanTarget -= pacmanTransform.forward.normalized * numTiles;
    if (pacmanDirection == Direction.Left) pacmanTarget -= pacmanTransform.right.normalized * numTiles;
    if (pacmanDirection == Direction.Right) pacmanTarget += pacmanTransform.right.normalized * numTiles;

    Vector3 blinkyPosition = blinky.transform.position;
    Vector3 target = blinkyPosition + (pacmanTarget - blinkyPosition) * 2f;
    return directionToTarget(target);
  }


}
