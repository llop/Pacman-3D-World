using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionWalker : WaypointWalker {


  public float duration;
  public float interval;
  public float durationTimer;
  public float intervalTimer;

  public GameObject explosionPrefab;


  ObjectPool objectPool;

  public override void awake() {
  }

  public override void startNavigation() {
  }

  public override void startDirection() {
  }

  public override void startState() {
    objectPool = gameManager.levelManager.objectPool;
    intervalTimer = 0f;
    durationTimer = duration;
  }

  public override void updateInput() {}
    
  public override void updateDirection() {
    if (currentNode == null) {
      gameObject.SetActiveRecursively(false);
      return;
    }
    // turn when state update forced a direction change
    if ((!_init && currentNode == nextNode) || atNextNode()) processDirection();
  }

  public override void updateMove() {
    moveAmount = Vector3.forward * walkSpeed;
  }


  public override void update() {

    // explode?
    intervalTimer -= Time.deltaTime;
    if (intervalTimer <= 0f) {
      intervalTimer = interval;
      GameObject obj = objectPool.instantiate(Tags.BombFireball);
      obj.transform.position = transform.position;
      obj.transform.rotation = transform.rotation;
    }

    // finish?
    durationTimer -= Time.deltaTime;
    if (durationTimer <= 0f) {
      durationTimer = duration;
      _init = false;
      gameObject.SetActive(false);
    }
  }



	override protected void processDirection() {
    _init = true;
    currentNode = nextNode;

    bool first = true;
    List<Direction> directions = allowedDirections();
    foreach (Direction dir in directions) {
      if (first) {
        first = false;
        currentDirection = dir;
        assignNextNodeFromDirection();
      } else {
        
        GameObject obj = objectPool.instantiate(Tags.ExplosionWalker);
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;

        ExplosionWalker walker = obj.GetComponent<ExplosionWalker>();
        walker.init(currentNode, nextNode, currentDirection);
        walker.Start();
        walker.durationTimer = durationTimer;
        walker.currentDirection = dir;
        walker.assignNextNodeFromDirection();
      }
    }
  }



  private bool _init = false;

  public void init(WaypointNode curr, WaypointNode nex, Direction dir) {
    currentNode = curr;
    nextNode = nex;
    currentDirection = dir;

    if (currentNode != null) {
      topo.updatePlane(currentNode.transform.position, nextNode.transform.position, ref nextNodePlane);
      topo.updateRotation(transform, nextNode.transform.position);
    }
  }


  private void assignNextNodeFromDirection() {
    if (currentDirection == Direction.Front) nextNode = currentNode.getFront();
    else if (currentDirection == Direction.Back) nextNode = currentNode.getBack();
    else if (currentDirection == Direction.Left) nextNode = currentNode.getLeft();
    else if (currentDirection == Direction.Right) nextNode = currentNode.getRight();

    topo.updatePlane(currentNode.transform.position, nextNode.transform.position, ref nextNodePlane);
    topo.updateRotation(transform, nextNode.transform.position);
  }

  private List<Direction> allowedDirections() {
    List<Direction> directions = new List<Direction>();
    if (currentNode.getFront() != null && !currentDirection.isOpposite(Direction.Front)) directions.Add(Direction.Front);
    if (currentNode.getLeft() != null && !currentDirection.isOpposite(Direction.Left)) directions.Add(Direction.Left);
    if (currentNode.getBack() != null && !currentDirection.isOpposite(Direction.Back)) directions.Add(Direction.Back);
    if (currentNode.getRight() != null && !currentDirection.isOpposite(Direction.Right)) directions.Add(Direction.Right);
    return directions;

  }


}
