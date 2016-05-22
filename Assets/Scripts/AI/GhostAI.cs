using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public enum GhostAIState {
  Chase,
  Scatter,
  Frightened,
  Dead
}



//-------------------------------------------------------------------------------------
// check this out:
// http://gameinternals.com/post/2072558330/understanding-pac-man-ghost-behavior
// http://www.gamasutra.com/view/feature/3938/the_pacman_dossier.php?print=1
//-------------------------------------------------------------------------------------
[RequireComponent (typeof (WaypointWalker))]
public abstract class GhostAI : MonoBehaviour {



  //-----------------------------------------------------------------------------------
  // in scatter mode, ghosts try to reach this node
  //-----------------------------------------------------------------------------------

  public WaypointNode cornerNode;



  //-----------------------------------------------------------------------------------
  // find the ghost waypoint walker
  // and pacman too
  // and topography
  //-----------------------------------------------------------------------------------

  protected WaypointWalker walker;
  protected GameObject pacman;
  protected PlanetTopography topo;

  public void Awake() {
    walker = GetComponent<WaypointWalker>();
    pacman = GameObject.FindGameObjectWithTag(Tags.Pacman);
    topo = GameObject.FindGameObjectWithTag(Tags.Planet).GetComponent<PlanetTopography>();
    awake();
  }

  // override this method instead of Awake
  public virtual void awake() {}



  //-----------------------------------------------------------------------------------
  // whenever ghosts change from chase or scatter to any other mode, 
  // they are forced to reverse direction. note that
  // when the ghosts leave frightened mode, or die, they do not change direction
  //-----------------------------------------------------------------------------------

  public bool forceReverse { get; private set; }



  //-----------------------------------------------------------------------------------
  // ghost state determines what a ghost will do when they reach an intersection
  //-----------------------------------------------------------------------------------

  private GhostAIState _state;
  public GhostAIState state { 
    get { return _state; } 
    set {
      if ((_state == GhostAIState.Chase || _state == GhostAIState.Scatter) && _state != value)
        forceReverse = true;
      _state = value;
    }
  }



  //-----------------------------------------------------------------------------------
  // subclasses must implement these methods to provide different behaviours
  //-----------------------------------------------------------------------------------

  protected abstract Direction directionChase();

  protected virtual Direction directionScatter() {
    return directionToTarget(cornerNode.transform.position);
  }

  protected virtual Direction directionFrightened() {
    // ghosts use a pseudo-random number generator (PRNG) 
    // to pick a way to turn at each intersection when frightened
    System.Random rnd = new System.Random();
    Direction direction = (Direction)rnd.Next(4);
    if (validDirection(direction)) return direction;

    // if a wall blocks the chosen direction, 
    // the ghost then attempts the remaining directions in this order: 
    // up, left, down, and right, until a passable direction is found
    WaypointNode node = walker.getCurrentNode();
    if (node.getFront() != null) return Direction.Front;
    if (node.getLeft() != null) return Direction.Left;
    if (node.getBack() != null) return Direction.Back;
    if (node.getRight() != null) return Direction.Right;
    return Direction.None;
  }

  protected virtual Direction directionDead() {
    return directionToTarget(walker.spawn.transform.position);
  }



  //-----------------------------------------------------------------------------------
  // ghost walkers should invoke this method to get new directions
  //-----------------------------------------------------------------------------------

  public Direction direction() {

    // force direction change is the easy one
    if (forceReverse) {
      forceReverse = false;
      return walker.getDirection().getOpposite();
    }

    // otherwise let the state determine where to go
    if (state == GhostAIState.Chase) return directionChase();
    if (state == GhostAIState.Scatter) return directionScatter();
    if (state == GhostAIState.Frightened) return directionFrightened();
    if (state == GhostAIState.Dead) return directionDead();

    // should never happen!
    return Direction.None;

  }



  //-----------------------------------------------------------------------------------
  // which directions are allowed now?
  //-----------------------------------------------------------------------------------

  protected List<Direction> allowedDirections() {
    
    // if two or more potential choices are an equal distance from the target, 
    // the decision between them is made in the order of up > left > down (> right)
    List<Direction> directions = new List<Direction>();
    Direction direction = walker.getDirection();
    WaypointNode node = walker.getCurrentNode();
    if (node.getFront() != null && !direction.isOpposite(Direction.Front)) directions.Add(Direction.Front);
    if (node.getLeft() != null && !direction.isOpposite(Direction.Left)) directions.Add(Direction.Left);
    if (node.getBack() != null && !direction.isOpposite(Direction.Back)) directions.Add(Direction.Back);
    if (node.getRight() != null && !direction.isOpposite(Direction.Right)) directions.Add(Direction.Right);
    return directions;

  }


  //-----------------------------------------------------------------------------------
  // find out if we the ghost can move in the specified direction
  //-----------------------------------------------------------------------------------

  protected bool validDirection(Direction direction) {
    WaypointNode node = walker.getCurrentNode();
    if (direction == Direction.Front) return node.getFront() != null;
    if (direction == Direction.Back) return node.getBack() != null;
    if (direction == Direction.Left) return node.getLeft() != null;
    if (direction == Direction.Right) return node.getRight() != null;
    return false;
  }


  //-----------------------------------------------------------------------------------
  // when a decision about which direction to turn is necessary, 
  // the choice is made based on which tile adjoining the intersection 
  // will put the ghost nearest to its target tile, measured in a straight line
  //-----------------------------------------------------------------------------------

  protected Direction directionToTarget(Vector3 target) {

    WaypointNode node = walker.getCurrentNode();
    float bestDistance = float.MaxValue;
    Direction bestDirection = Direction.None;

    List<Direction> directions = allowedDirections();
    foreach (Direction direction in directions) {
      
      Vector3 nextNodePosition = node.getAdjacent(direction).transform.position;
      Vector3 directionVector = (nextNodePosition - node.transform.position).normalized;
      Vector3 origin = node.transform.position + directionVector;
      float distance = topo.calculateDistance(origin, target);
      if (bestDistance > distance) {
        bestDistance = distance;
        bestDirection = direction;
      }

    }
    return bestDirection;
  }


}
