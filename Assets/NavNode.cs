using UnityEngine;
using System.Collections;

public class NavNode : MonoBehaviour {
	public NavNode front, back, left, right;
	public bool frontPhantomOnly = false, backPhantomOnly = false, leftPhantomOnly = false, rightPhantomOnly = false;
	public NavNode twin;
	// Use this for initialization
	void Start () {
		if (front != null) {
			front.back = this;
			if (frontPhantomOnly) {
				front.backPhantomOnly = true;
			}
		}
		if (back != null) {
			back.front = this;
			if (backPhantomOnly) {
				back.frontPhantomOnly = true;
			}
		}
		if (left != null) {
			left.right = this;
			if (leftPhantomOnly) {
				left.rightPhantomOnly = true;
			}
		}
		if (right != null) {
			right.left = this;
			if (rightPhantomOnly) {
				right.leftPhantomOnly = true;
			}
		}
		if (twin != null) {
			twin.twin = this;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool isPacmanWalkable(Direction dir) {
		if (dir == Direction.Back)
			return (back != null && !backPhantomOnly);
		else if (dir == Direction.Front)
			return (front != null && !frontPhantomOnly);
		else if (dir == Direction.Left)
			return (left != null && !leftPhantomOnly);
		else if (dir == Direction.Right)
			return (right != null && !rightPhantomOnly);
		else {
			return false;
		}
	}

	public bool isPhantomWalkable(Direction dir) {
		if (dir == Direction.Back)
			return back != null;
		else if (dir == Direction.Front)
			return front != null;
		else if (dir == Direction.Left)
			return left != null;
		else if (dir == Direction.Right)
			return right != null;
		else {
			return false;
		}
	}

	public NavNode getNavNodeAtDir(Direction dir) {
		if (dir == Direction.Back)
			return back;
		else if (dir == Direction.Front)
			return front;
		else if (dir == Direction.Left)
			return left;
		else if (dir == Direction.Right)
			return right;
		else {
			return null;
		}
	}
}
