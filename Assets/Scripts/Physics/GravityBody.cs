using UnityEngine;
using System.Collections;



[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {



  private GravityAttractor planet;

  void Awake() {
    planet = GameObject.FindGameObjectWithTag(Tags.Planet).GetComponent<GravityAttractor>();
    GetComponent<Rigidbody>().useGravity = false;
    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
  }

  void FixedUpdate() {
    planet.attract(transform);
  }


}
