using UnityEngine;
using System.Collections;



public abstract class GravityAttractor : MonoBehaviour {



  // earth-like gravity?
  public float gravity = -9.8f;


  // attracts a body towards the center of the planet
  // as usual with physics, this should happen on FixedUpdate
  // rotat the body so its up direction is correct
  // then pull it towards the center
  public abstract void attract(Transform body);



}
