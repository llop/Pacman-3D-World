using UnityEngine;
using System.Collections;



//------------------------------------------------------------------------------------------------------------
// a walker handles movement
// and sets state information which will be used for animation
//------------------------------------------------------------------------------------------------------------
[RequireComponent (typeof (Rigidbody))]
public abstract class WaypointWalker : MonoBehaviour {



  //----------------------------------------------------------------------------------------------------------
  // game manager instance
  //----------------------------------------------------------------------------------------------------------

  protected GameManager gameManager;
  protected Rigidbody walkerBody;

  public void Awake() {
    gameManager = GameManager.Instance;
    walkerBody = GetComponent<Rigidbody>();
  }



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

  public Direction getDirection() { return currentDirection; }


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
  // start callbacks
  //------------------------------------------------------------------------

  public virtual void startNavigation() {
    GameObject planet = GameObject.FindGameObjectWithTag(Tags.Planet);
    graph = planet.GetComponent<WaypointGraph>();
    topo = planet.GetComponent<PlanetTopography>();
    currentNode = spawn;
    nextNode = currentNode;
  }

  public virtual void startDirection() {
    currentDirection = Direction.None;
    inputDirection = Direction.None;
  }

  public abstract void startState();

  public virtual void start() {
    topo.updatePlane(currentNode.transform.position, nextNode.transform.position, ref nextNodePlane);
    topo.updateRotation(transform, nextNode.transform.position);
  }

  void Start() {
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

  void Update() {
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
