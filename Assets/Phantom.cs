using UnityEngine;
using System.Collections;

public class Phantom : NavGraphWalker {
	public enum Type { Blinky, Pinky, Inky, Clyde };
	public Type type;
	public Pacman pacman;
	public NavNode home;
	public float returnVelocity;
	bool alive = true;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		returnVelocity = 2 * velocity;
		isPhantom = true;
		if (pacman == null) {
			pacman = GameObject.Find ("Pac-man").GetComponent<Pacman> ();
		}
		if (home == null) {
			home = GameObject.Find ("PhantomHome").GetComponent<NavNode> ();
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		if (Input.GetKeyDown (KeyCode.C) && alive) {
			alive = false;
			forceGetNewDirection = true;
		}
		alive = (alive || currentNode == home);
		base.Update ();
	}

	protected override Direction GetNewDirection() {
		// diff is calculated different for every type of ghost
		Vector3 diff;
		if (alive) {
			if (type == Type.Blinky || type == Type.Clyde) {
				diff = pacman.nextNode.transform.position - transform.position;
			} else {
				diff = pacman.currentNode.transform.position - transform.position;
			}
		} else {
			diff = home.transform.position - transform.position;
		}

		float x = diff.x, y = diff.z;

		// Sort directions by farthest
		Direction[] directions = {Direction.Left, Direction.Right, Direction.Back, Direction.Front};

		if (x > 0) {
			directions [0] = Direction.Right;
			directions [1] = Direction.Left;
		}

		if (y > 0) {
			directions [2] = Direction.Front;
			directions [3] = Direction.Back;
		}

		if (Mathf.Abs (x) > Mathf.Abs (y)) {
			Direction tmp = directions [3];
			directions [3] = directions [1];
			directions [1] = directions [2];
			directions [2] = tmp;
		} else {
			Direction tmp = directions [0];
			directions [0] = directions [2];
			directions [2] = directions [1];
			directions [1] = tmp;
		}

		// Choose first walkable direction
		for (int i = 0; i < 4; ++i) {
			if (previousNode != currentNode.getNavNodeAtDir(directions[i]) && currentNode.isPhantomWalkable(directions[i])) {
				return directions[i];
			}
		}
		return Direction.None;
	}
}
