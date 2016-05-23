using UnityEngine;
using System.Collections;




public static class DirectionMethods {
  public static bool isOpposite(this Direction direction, Direction otherDirection) {
    if (direction == Direction.Front) return otherDirection == Direction.Back;
    if (direction == Direction.Back) return otherDirection == Direction.Front;
    if (direction == Direction.Right) return otherDirection == Direction.Left;
    if (direction == Direction.Left) return otherDirection == Direction.Right;
    return false;
  }
  public static Direction getOpposite(this Direction direction) {
    if (direction == Direction.Front) return Direction.Back;
    if (direction == Direction.Back) return Direction.Front;
    if (direction == Direction.Right) return Direction.Left;
    if (direction == Direction.Left) return Direction.Right;
    return Direction.None;
  }
}


public enum Direction { 
  Front, 
  Back, 
  Left, 
  Right, 
  None 
};




public class NavGraphWalker : MonoBehaviour {
	public NavNode currentNode;
	public float velocity = 5;

	float traveledDistance, journeyDistance;
	public NavNode previousNode, nextNode;
	protected Direction currentDirection, inputDirection;

	protected bool isPhantom = false;
	protected bool forceGetNewDirection = false;

	// Use this for initialization
	virtual protected void Start () {
		currentDirection = Direction.None;
		inputDirection = Direction.None;
		nextNode = currentNode;
		previousNode = currentNode;
		transform.position = currentNode.transform.position;
		traveledDistance = 0;
		journeyDistance = 0;
	}

	// Update is called once per frame
	virtual protected void Update () {
		if (forceGetNewDirection) {
			forceGetNewDirection = false;
			inputDirection = GetNewDirection ();
		}
		// Movement logic
		if (currentDirection != Direction.None) {
			traveledDistance += Time.deltaTime * velocity;
		}
		if (traveledDistance >= journeyDistance) {
			previousNode = currentNode;
			if (nextNode.twin) {
				transform.position = nextNode.twin.transform.position;
				currentNode = nextNode.twin;
			} else {
				transform.position = nextNode.transform.position;
				currentNode = nextNode;
			}

			inputDirection = GetNewDirection ();
			if (inputDirection != Direction.None && (isPhantom && nextNode.isPhantomWalkable(inputDirection) || nextNode.isPacmanWalkable(inputDirection))) {
				currentDirection = inputDirection;
				inputDirection = Direction.None;
			}

			NavNode next = null;
			if (currentDirection == Direction.Back)
				next = currentNode.back;
			else if (currentDirection == Direction.Front)
				next = currentNode.front;
			else if (currentDirection == Direction.Left)
				next = currentNode.left;
			else if (currentDirection == Direction.Right)
				next = currentNode.right;
			else {
				next = currentNode;
			}

			if (next == null) {
				nextNode = currentNode;
				currentDirection = Direction.None;
			} else {
				nextNode = next;
			}
			traveledDistance -= journeyDistance;
			journeyDistance = Vector3.Distance (currentNode.transform.position, nextNode.transform.position);
			if (currentDirection == Direction.None) {
				traveledDistance = 0;
			}
		} else if (currentDirection == Direction.Back && inputDirection == Direction.Front
			|| currentDirection == Direction.Front && inputDirection == Direction.Back
			|| currentDirection == Direction.Right && inputDirection == Direction.Left
			|| currentDirection == Direction.Left && inputDirection == Direction.Right) {
			traveledDistance = journeyDistance - traveledDistance;
			NavNode tmp = nextNode;
			nextNode = currentNode;
			currentNode = tmp;
			currentDirection = inputDirection;
			inputDirection = Direction.None;
		}

		if (currentDirection != Direction.None) {
			float ratio = traveledDistance / journeyDistance;
			transform.position = Vector3.Lerp (currentNode.transform.position, nextNode.transform.position, ratio);
		}
	}

	virtual protected Direction GetNewDirection() {
		return Direction.None;
	}
}
