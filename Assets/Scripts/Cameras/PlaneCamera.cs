using UnityEngine;
using System.Collections;

public class PlaneCamera : MonoBehaviour {


  private GameObject pacman;

  public float cameraHeight = 15f;


  void Start() {
    pacman = GameObject.FindGameObjectWithTag(Tags.Pacman);
  }

  void Update() {

    // find camera position
    Vector3 pacmanPosition = pacman.transform.position;
    Vector3 cameraPos = pacmanPosition;
    cameraPos.y = cameraHeight;
    transform.position = cameraPos;

    // have it face down
    transform.LookAt(pacmanPosition);
  }



}
