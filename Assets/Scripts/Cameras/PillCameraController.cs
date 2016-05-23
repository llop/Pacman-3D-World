using UnityEngine;
using System.Collections;

public class PillCameraController : MonoBehaviour {


  public float cameraHeight = 35f;


  private GameObject pacman;


  void Start() {
    pacman = GameObject.FindGameObjectWithTag(Tags.Pacman);
  }

  void Update() {

    // find camera position
    Vector3 pacmanPosition = pacman.transform.position;
    Vector3 gravityCenter = new Vector3(0f, pacmanPosition.y, 0f);

    Vector3 pacmanUp = pacmanPosition;
    pacmanUp.y = 0f;

    Vector3 cameraPos = gravityCenter + pacmanUp.normalized * cameraHeight;
    transform.position = cameraPos;

    // have it face down
    transform.LookAt(gravityCenter);
  }

}
