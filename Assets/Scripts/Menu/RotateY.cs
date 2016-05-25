using UnityEngine;
using System.Collections;


public class RotateY : MonoBehaviour {


  public float speed = 20f;
  public bool clockwise = false;

	
  public void Update() {
    float factor = (clockwise ? 1f : -1f) * speed;
    transform.Rotate(Vector3.up * Time.deltaTime * factor);
  }


}
