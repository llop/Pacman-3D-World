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

  protected GameManager gameManager;
  protected WaypointWalker walker;
  protected GameObject pacman;
  protected PlanetTopography topo;

  protected int modesTableIndex;
  protected List<KeyValuePair<GhostAIState, float>> modesTable;
  protected float frightenedTime;
  protected float liveTime;

  public void Awake() {
    gameManager = GameManager.Instance;
    walker = GetComponent<WaypointWalker>();
    pacman = GameObject.FindGameObjectWithTag(Tags.Pacman);
    topo = GameObject.FindGameObjectWithTag(Tags.Planet).GetComponent<PlanetTopography>();

    modesTable = gameManager.ghostModesForCurrentLevel();
    resetMode();

    awake();
  }

  // override this method instead of Awake
  public virtual void awake() {}

  // handle AI state 
  public void Update() {
    if (gameManager.paused || !gameManager.inGame) return;

    if (_state == GhostAIState.Frightened) {
      frightenedTime -= Time.deltaTime;
      if (frightenedTime < 0f) {
        frightenedTime = 0f;
        _state = _previousState;
      }
    } else {
      if (_state != GhostAIState.Dead) {
        if (modesTableIndex >= 0 && liveTime >= 0f) {
          liveTime -= Time.deltaTime;
          if (liveTime < 0f) {
            ++modesTableIndex;
            _state = modesTable[modesTableIndex].Key;
            liveTime = modesTable[modesTableIndex].Value;
          }
        }
      }
    }
  }


  //-----------------------------------------------------------------------------------
  // animators can use this to pick a texture
  //-----------------------------------------------------------------------------------

  public void resetMode() {
    frightenedTime = 0f;
    if (modesTable != null && modesTable.Count > 0) {
      modesTableIndex = 0;
      _state = modesTable[modesTableIndex].Key;
      liveTime = modesTable[modesTableIndex].Value;
    } else {
      modesTableIndex = -1;
      _state = GhostAIState.Chase;
      liveTime = -1f;
    }
  }


  //-----------------------------------------------------------------------------------
  // animators can use this to pick a texture
  //-----------------------------------------------------------------------------------

  public double remainingFrightenedTime { get { return frightenedTime; } }


  //-----------------------------------------------------------------------------------
  // whenever ghosts change from chase or scatter to any other mode, 
  // they are forced to reverse direction. note that
  // when the ghosts leave frightened mode, or die, they do not change direction
  //-----------------------------------------------------------------------------------

  public bool forceReverse { get; private set; }



  //-----------------------------------------------------------------------------------
  // call this to make scare ghost
  //-----------------------------------------------------------------------------------

  public void scare() {
    if (_state == GhostAIState.Dead) return;
    frightenedTime = gameManager.ghostFrightenedTimeForCurrentLevel();
    if (_state != GhostAIState.Frightened) state = GhostAIState.Frightened;
  }


  //-----------------------------------------------------------------------------------
  // ghost state determines what a ghost will do when they reach an intersection
  //-----------------------------------------------------------------------------------

  private GhostAIState _state;
  private GhostAIState _previousState;
  public GhostAIState state { 
    get { return _state; } 
    set {
      // ghosts are forced to reverse direction by the system anytime the mode changes from: 
      // chase-to-scatter, chase-to-frightened, scatter-to-chase, and scatter-to-frightened 
      // ghosts do not reverse direction when changing back from frightened to chase or scatter modes
      forceReverse = (_state == GhostAIState.Chase && 
          (value == GhostAIState.Scatter || value == GhostAIState.Frightened))
        || (_state == GhostAIState.Scatter && 
          (value == GhostAIState.Chase || value == GhostAIState.Frightened));
      _previousState = _state;
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
      return walker.direction().getOpposite();
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
    Direction direction = walker.direction();
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
