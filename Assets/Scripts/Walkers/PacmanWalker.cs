using UnityEngine;
using System.Collections;



public class PacmanWalker : WaypointWalker {



  //----------------------------------------------------------------------------------------------------------
  // jump variables
  //----------------------------------------------------------------------------------------------------------

  protected float jumpTolerance = .4f;    // used to decide when the character is descending
  public float jumpForce = 400f;          // how high


  //----------------------------------------------------------------------------------------------------------
  // input method shouldn't change too much so implementation is provided here
  //----------------------------------------------------------------------------------------------------------

  protected bool inputJump; 

  public override void updateInput() {
    if (!gameManager.pacmanData.alive) return;

    // read input
    if (Input.GetKeyDown(KeyCode.UpArrow)) inputDirection = Direction.Front;
    else if (Input.GetKeyDown(KeyCode.DownArrow)) inputDirection = Direction.Back;
    else if (Input.GetKeyDown(KeyCode.LeftArrow)) inputDirection = Direction.Left;
    else if (Input.GetKeyDown(KeyCode.RightArrow)) inputDirection = Direction.Right;

    inputJump = Input.GetKeyDown(KeyCode.X);
  }


  //----------------------------------------------------------------------------------------------------------
  // animation states (transitions, to be more precise)
  //----------------------------------------------------------------------------------------------------------

  protected bool isMoving;
  protected bool isGrounded;
  protected bool isJumpingUp;
  protected bool isJumpingDown;
  protected bool isJustLanded;

  public bool moving() { return isMoving; }
  public bool grounded() { return isGrounded; }
  public bool jumpingUp() { return isJumpingUp; }
  public bool jumpingDown() { return isJumpingDown; }
  public bool justLanded() { return isJustLanded; }


  public override void awake() {
    gameManager.registerPacman(gameObject);
  }

  //----------------------------------------------------------------------------------------------------------
  // default implementation for 'start' state initialization is provided here
  //----------------------------------------------------------------------------------------------------------

  public override void startState() {
    isMoving = false;
    isGrounded = true;
    isJumpingUp = false;
    isJumpingDown = false;
    isJustLanded = false;
  }


  //------------------------------------------------------------------------
  // update callbacks
  //------------------------------------------------------------------------

  public override void updateDirection() {
    if (!gameManager.pacmanData.alive) return;

    // turn if we are at the next node or past it
    // also turn if user reversed direction, or accept new direction when standing still
    if (currentDirection == Direction.None || currentDirection.isOpposite(inputDirection) || atNextNode()) 
      processDirection();

	topo.updateRotation (transform, nextNode.transform.position);
  }

  public override void updateMove() {
    // movement + jump
    processMove();
    processJump();
  }

  public override void update() {
    // where's pacman facing?
    Debug.DrawLine(transform.position, transform.position + transform.forward.normalized * 4, Color.red);
  }


  //------------------------------------------------------------------------
  // pacman moves forward
  //------------------------------------------------------------------------

  private void processMove() {
    isMoving = gameManager.pacmanData.alive && currentDirection != Direction.None;
    Vector3 moveDir = isMoving ? Vector3.forward : Vector3.zero;
    moveAmount = moveDir * walkSpeed;
  }


  //------------------------------------------------------------------------
  // handle turns
  //------------------------------------------------------------------------

  private void processDirection() {
    // reset nodes
    currentNode = nextNode;

    // reset directions 
    Direction nextDirection = validDirection(inputDirection) ? inputDirection : currentDirection;
    currentDirection = Direction.None;

    // apply changes if possible
    if (validDirection(nextDirection)) {
      currentDirection = nextDirection;
      if (currentDirection == Direction.Front) nextNode = currentNode.getFront();
      else if (currentDirection == Direction.Back) nextNode = currentNode.getBack();
      else if (currentDirection == Direction.Left) nextNode = currentNode.getLeft();
      else if (currentDirection == Direction.Right) nextNode = currentNode.getRight();
    }

    // update next node plane and rotation if applicable
    if (currentDirection != Direction.None) {
      topo.updatePlane(currentNode.transform.position, nextNode.transform.position, ref nextNodePlane);
      topo.updateRotation(transform, nextNode.transform.position);
    }

  }


  //------------------------------------------------------------------------
  // jump
  //------------------------------------------------------------------------

  private void processJump() {
    // do something about jumping
    if (isGrounded) {
      // grounded, do we jump?
      isJustLanded = false;
      isJumpingUp = false;
      isJumpingDown = false;
      if (inputJump) {
        GetComponent<Rigidbody>().AddForce(transform.up * jumpForce);
        isGrounded = false;
        isJumpingUp = true;
      }
    } else {
      if (isJumpingUp) {
        // ascending
        float velocity = GetComponent<Rigidbody>().velocity.magnitude;
        if (velocity <= jumpTolerance) {
          isJumpingUp = false;
          isJumpingDown = true;
        }
      } else {
        // descending jump, check if we're back on the floor
        RaycastHit hit;
        Vector3 position = transform.position + transform.up.normalized;
        Ray ray = new Ray(position, -transform.up);
        isGrounded = Physics.Raycast(ray, out hit, 1.1f, groundedMask);
        if (isGrounded) {
          isJustLanded = true;
          isJumpingUp = false;
          isJumpingDown = false;
        }
      }
    }
  }


  //----------------------------------------------------------------------------------------------------------
  // helper methods
  //----------------------------------------------------------------------------------------------------------

  protected bool validDirection(Direction dir) {
    if (dir == Direction.Front) return currentNode.getFront() != null && currentNode.getFront().pacmanWalkable;
    else if (dir == Direction.Back) return currentNode.getBack() != null && currentNode.getBack().pacmanWalkable;
    else if (dir == Direction.Left) return currentNode.getLeft() != null && currentNode.getLeft().pacmanWalkable;
    else if (dir == Direction.Right) return currentNode.getRight() != null && currentNode.getRight().pacmanWalkable;
    return false;
  }



}
