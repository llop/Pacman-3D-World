using UnityEngine;
using System.Collections;



[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {


  private GravityAttractor attractor;


  void Awake() {
    GameObject planet = GameObject.FindGameObjectWithTag(Tags.Planet);
    attractor = planet.GetComponent<GravityAttractor>();

    Rigidbody rigidBody = GetComponent<Rigidbody>();
    rigidBody.useGravity = false;
    rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
  }


  // on the fixed update, the boy suffered the devastating effects of gravity
  void FixedUpdate() {
    attractor.attract(transform);
  }


}
