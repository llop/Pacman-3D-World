using UnityEngine;
using System.Collections;

public class SphereGravityAttractor : MonoBehaviour {


  // earth-like gravity?
  public float gravity = -9.8f;


  // attracts a body towards the center of the planet
  // as usual with physics, this should happen on FixedUpdate
  public void attract(Transform body) {
    // rotat the body so its up direction is correct
    // then pull it towards the center
    Vector3 targetDir = (body.position - transform.position).normalized;
    body.rotation = Quaternion.FromToRotation(body.up, targetDir) * body.rotation;
    body.GetComponent<Rigidbody>().AddForce(targetDir * gravity);
  }

}
