using UnityEngine;
using System.Collections;



//------------------------------------------------------------------------------------------------------------
// a walker handles movement
// and sets state information which will be used for animation
//------------------------------------------------------------------------------------------------------------
[RequireComponent (typeof (Rigidbody))]
public abstract class WaypointWalker : MonoBehaviour {



  //----------------------------------------------------------------------------------------------------------
  // game manager instance and the walkers rigidbody (aka. stiffbody)
  //----------------------------------------------------------------------------------------------------------

  protected GameManager gameManager;
  protected Rigidbody walkerBody;


  //----------------------------------------------------------------------------------------------------------
  // spawn node
  //----------------------------------------------------------------------------------------------------------

  public WaypointNode spawn;


  //----------------------------------------------------------------------------------------------------------
  // navigation stuff
  //----------------------------------------------------------------------------------------------------------

  protected WaypointGraph graph;
  protected WaypointNode currentNode;
  protected WaypointNode nextNode;

  public WaypointGraph getGraph() { return graph; }
  public WaypointNode getCurrentNode() { return currentNode; }
  public WaypointNode getNextNode() { return nextNode; }


  //----------------------------------------------------------------------------------------------------------
  // next node plane
  //----------------------------------------------------------------------------------------------------------

  protected Plane nextNodePlane;          // this plane contains the nextNode position
  protected float turnTolerance = .01f;   // how close pacman has to be to the target plane to allow turning

  protected bool atNextNode() {
    return nextNodePlane.GetDistanceToPoint(transform.position) <= turnTolerance;
  }


  //----------------------------------------------------------------------------------------------------------
  // directions stuff
  //----------------------------------------------------------------------------------------------------------

  protected Direction currentDirection;
  protected Direction inputDirection;

  public Direction direction() { return currentDirection; }

  public virtual void halt() {
    inputDirection = Direction.None;
    currentDirection = Direction.None;
  }


  //----------------------------------------------------------------------------------------------------------
  // movement
  //----------------------------------------------------------------------------------------------------------

  public float walkSpeed = 8f;          // how fast

  protected Vector3 moveAmount;


  //----------------------------------------------------------------------------------------------------------
  // references the ground mesh
  //----------------------------------------------------------------------------------------------------------

  public LayerMask groundedMask;


  //----------------------------------------------------------------------------------------------------------
  // topography
  //----------------------------------------------------------------------------------------------------------

  protected PlanetTopography topo;


  //------------------------------------------------------------------------
  // awake callbacks
  //------------------------------------------------------------------------

  public void Awake() {
    gameManager = GameManager.Instance;
    walkerBody = GetComponent<Rigidbody>();

    GameObject planet = GameObject.FindGameObjectWithTag(Tags.Planet);
    graph = planet.GetComponent<WaypointGraph>();
    topo = planet.GetComponent<PlanetTopography>();
    awake();
  }

  public virtual void awake() {}


  //------------------------------------------------------------------------
  // start callbacks
  //------------------------------------------------------------------------

  public virtual void startNavigation() {
    // set nodes
    currentNode = spawn;
    nextNode = currentNode;

    // move to spawn
    transform.position = spawn.transform.position;
  }

  public virtual void startDirection() {
    currentDirection = Direction.None;
    inputDirection = Direction.None;
  }

  public virtual void startState() {}

  public virtual void start() {
    topo.updatePlane(currentNode.transform.position, nextNode.transform.position, ref nextNodePlane);
    topo.updateRotation(transform, nextNode.transform.position);
  }

  public void Start() {
    startNavigation();
    startDirection();
    startState();
    start();
  }
	

  //------------------------------------------------------------------------
  // update callbacks
  //------------------------------------------------------------------------

  public abstract void updateInput();
  public abstract void updateDirection();
  public abstract void updateMove();
  public abstract void update();

  public void Update() {
    if (gameManager.paused || !gameManager.inGame) return;

    updateInput();
    updateDirection();
    updateMove();
    update();
  }


  //------------------------------------------------------------------------
  // apply movement
  //------------------------------------------------------------------------

  public void FixedUpdate() {
    if (gameManager.paused || !gameManager.inGame) return;

    walkerBody.MovePosition(walkerBody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
  }


}
