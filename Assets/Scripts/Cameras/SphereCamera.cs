using UnityEngine;
using System.Collections;



public class SphereCamera : GameCamera {



  public float cameraHeight = 35f;

	

	void Update() {
    // set camera position
    Vector3 pacmanUp = pacman.transform.position.normalized;
    Vector3 cameraPos = pacmanUp * cameraHeight;
    transform.position = cameraPos;
    // have it face down
    transform.LookAt(Vector3.zero);
	}



}
