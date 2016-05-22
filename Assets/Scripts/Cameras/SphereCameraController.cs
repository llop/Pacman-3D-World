using UnityEngine;
using System.Collections;

public class SphereCameraController : MonoBehaviour {


  public float cameraHeight = 30f;


  private GameObject pacman;


  void Start() {
    pacman = GameObject.FindGameObjectWithTag(Tags.Pacman);
	}
	
	void Update() {
    // set camera position
    Vector3 pacmanUp = pacman.transform.position.normalized;
    Vector3 cameraPos = pacmanUp * cameraHeight;
    transform.position = cameraPos;
    // have it face down
    transform.LookAt(Vector3.zero);
	}

}
