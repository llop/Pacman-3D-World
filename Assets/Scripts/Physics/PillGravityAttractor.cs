using UnityEngine;
using System.Collections;

public class PillGravityAttractor : GravityAttractor {



  // attracts a body towards the center of the planet
  // as usual with physics, this should happen on FixedUpdate
  public override void attract(Transform body) {
    // rotat the body so its up direction is correct
    // then pull it towards the center
    Vector3 bodyUp = body.position;
    bodyUp.y = 0;
    Vector3 targetDir = bodyUp.normalized;

    body.rotation = Quaternion.FromToRotation(body.up, targetDir) * body.rotation;
    body.GetComponent<Rigidbody>().AddForce(targetDir * gravity);
  }



}
