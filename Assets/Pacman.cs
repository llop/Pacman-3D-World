using UnityEngine;
using System.Collections;

public class Pacman : NavGraphWalker {

	private bool jumping = false;
	private float timeJumpStarted;
	public float jumpHeight = 3;
	public float jumpingTime = 3;
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	// Update is called once per frame
	protected override void Update () {
		// Input
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			inputDirection = Direction.Front;
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			inputDirection = Direction.Back;
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			inputDirection = Direction.Left;
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			inputDirection = Direction.Right;
		} else if (Input.GetKeyDown (KeyCode.X) && !jumping) {
			jumping = true;
			timeJumpStarted = Time.time;
		}

		base.Update ();

		if (jumping) {
			float jumpingTimeDelta = Time.time - timeJumpStarted;
			transform.position += new Vector3 (0, Mathf.Sin (jumpingTimeDelta * Mathf.PI / jumpingTime) * jumpHeight, 0);
			jumping = (jumpingTime > jumpingTimeDelta);
		}
	}

	protected override Direction GetNewDirection() {
		return inputDirection;
	}
}
