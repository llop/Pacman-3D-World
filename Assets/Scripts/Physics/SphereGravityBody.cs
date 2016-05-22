using UnityEngine;
using System.Collections.Generic;
using System.Collections;




[RequireComponent (typeof (Rigidbody))]
public class SphereGravityBody : MonoBehaviour {

  private SphereGravityAttractor planet;

  void Awake() {
    planet = GameObject.FindGameObjectWithTag(Tags.Planet).GetComponent<SphereGravityAttractor>();
    GetComponent<Rigidbody>().useGravity = false;
    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
  }

  void FixedUpdate() {
    planet.attract(transform);
  }

}
