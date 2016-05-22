using UnityEngine;
using System.Collections;



public class WaypointNode : MonoBehaviour {


  public bool pacmanWalkable = true;


  private WaypointNode front;
  private WaypointNode back;
  private WaypointNode left;
  private WaypointNode right;


  public WaypointNode getFront() { return front; }
  public WaypointNode getBack() { return back; }
  public WaypointNode getLeft() { return left; }
  public WaypointNode getRight() { return right; }

  public void setFront(WaypointNode node) { front = node; }
  public void setBack(WaypointNode node) { back = node; }
  public void setLeft(WaypointNode node) { left = node; }
  public void setRight(WaypointNode node) { right = node; }

  public WaypointNode getAdjacent(Direction direction) {
    if (direction == Direction.Front) return front;
    if (direction == Direction.Back) return back;
    if (direction == Direction.Left) return left;
    if (direction == Direction.Right) return right;
    return null;
  }


  public void Awake() {
    //GetComponent<MeshRenderer>().enabled = false;
  }


}
