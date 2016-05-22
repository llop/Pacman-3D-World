using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GhostWalker : WaypointWalker {



  //----------------------------------------------------------------------------------------------------------
  // default implementation for 'start' state initialization is provided here
  // AI controls the ghost's state
  //----------------------------------------------------------------------------------------------------------

  protected GhostAI ai;
  protected GameManager gameManager;

  protected Collider ghostCollider;
  protected Collider pacmanCollider;


  public override void startState() {
    ai = GetComponent<GhostAI>();
    gameManager = GameManager.Instance;

    ghostCollider = GetComponent<Collider>();
    pacmanCollider = GameObject.FindGameObjectWithTag(Tags.Pacman).GetComponent<Collider>();
  }


  //----------------------------------------------------------------------------------------------------------
  // empty input method
  // decisions should be taken only at intersections, ie. in 'updateTurn'
  //----------------------------------------------------------------------------------------------------------

  public void OnCollisionEnter(Collision collision) {
    if (collision.gameObject.tag == Tags.Pacman) {
      if (ai.state != GhostAIState.Dead) {
        if (ai.state == GhostAIState.Frightened) {
          // kill ghost
          ai.state = GhostAIState.Dead;
          Physics.IgnoreCollision(ghostCollider, pacmanCollider, true);

          // give points to pacman
          gameManager.pacmanData().addScore(Score.Ghost);
        } else {
          // kill pacman

        }
      }
    }
  }


  //----------------------------------------------------------------------------------------------------------
  // empty input method
  // decisions should be taken only at intersections, ie. in 'updateTurn'
  //----------------------------------------------------------------------------------------------------------

  public override void updateInput() {}



  //----------------------------------------------------------------------------------------------------------
  // animation states (transitions, to be more precise)
  //----------------------------------------------------------------------------------------------------------

  public bool chase() { return ai.state == GhostAIState.Chase; }
  public bool scatter() { return ai.state == GhostAIState.Scatter; }
  public bool frightened() { return ai.state == GhostAIState.Frightened; }
  public bool dead() { return ai.state == GhostAIState.Dead; }



  //------------------------------------------------------------------------
  // update callbacks
  //------------------------------------------------------------------------

  public override void updateDirection() {
    // turn when state update forced a direction change
    // or when the ghost reaches an itersection
    if (ai.forceReverse || nextNodePlane.GetDistanceToPoint(transform.position) <= turnTolerance) 
      processDirection();
  }

  public override void updateMove() {
    // ghost moves forward
    moveAmount = Vector3.forward * walkSpeed;
  }

  public override void update() {
    // where's the ghost facing?
    Debug.DrawLine(transform.position, transform.position + transform.forward.normalized * 4, Color.red);
  }


  //------------------------------------------------------------------------
  // handle turns
  //------------------------------------------------------------------------

  private void processDirection() {

    // have the AI tell us where to
    currentNode = nextNode;
    currentDirection = ai.direction();
    if (currentDirection == Direction.Front) nextNode = currentNode.getFront();
    if (currentDirection == Direction.Back) nextNode = currentNode.getBack();
    if (currentDirection == Direction.Left) nextNode = currentNode.getLeft();
    if (currentDirection == Direction.Right) nextNode = currentNode.getRight();

    // update next node plane and rotation
    topo.updatePlane(currentNode.transform.position, nextNode.transform.position, ref nextNodePlane);
    topo.updateRotation(transform, nextNode.transform.position);

    // bring back to life?
    if (ai.state == GhostAIState.Dead && currentNode == spawn) {
      ai.state = GhostAIState.Chase;
      Physics.IgnoreCollision(ghostCollider, pacmanCollider, false);
    }
  }


}
