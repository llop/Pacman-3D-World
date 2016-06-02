using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaneTopography : PlanetTopography {


  
  //------------------------------------------------------------------------
  // get the plane towards which the character should move move
  //------------------------------------------------------------------------

  public override void updatePlane(Vector3 origin, Vector3 target, ref Plane plane) {
    Vector3 planeNormal = (origin - target).normalized;
    plane.SetNormalAndPosition(planeNormal, target);
  }



  //------------------------------------------------------------------------
  // have the character face the target
  //------------------------------------------------------------------------

  public override void updateRotation(Transform body, Vector3 target) {
    // face the next node
    body.LookAt(target);
    // correct up vector
    Vector3 targetUp = Vector3.up;
    body.rotation = Quaternion.FromToRotation(body.up, targetUp) * body.rotation;
  }



  public override float calculateDistance(Vector3 origin, Vector3 target) {
    return (origin-target).magnitude;
  }



  // project one and two on a plane, and get their UV coords
  // this plane's center is on the point between one and two
  // its normal is pointing at the origin
  // and its right vector is on the XZ plane
  public override void getUVCoords(WaypointNode one, out float uCoordA, out float vCoordA,
    WaypointNode two, out float uCoordB, out float vCoordB) {
    Vector3 onePosition = one.transform.position;
    onePosition.y = 0f;
    Vector3 twoPosition = two.transform.position;
    twoPosition.y = 0f;
    Vector3 center = (onePosition + twoPosition) / 2f;

    Vector3 uvA = (onePosition - center);
    uCoordA = uvA.x;
    vCoordA = uvA.z;

    Vector3 uvB = (twoPosition - center);
    uCoordB = uvB.x;
    vCoordB = uvB.z;
  }


	public override void getPelletPositions (WaypointNode one, WaypointNode two, out List<Vector3> positions) {
		positions = new List<Vector3> ();

		Collider col = GetComponent<Collider> ();

		float delta = Mathf.Max (col.bounds.max.x, col.bounds.max.z);
		Vector3 center = transform.position;
		center.y += 4 * delta;

		int distance = (int)calculateDistance (one.transform.position, two.transform.position);
		Vector3 direction1 = (one.transform.position - center).normalized;
		Vector3 direction2 = (two.transform.position - center).normalized;


		for (int i = 0; i <= distance; i++) {
			float ratio = (float)i/(float)distance;
			Vector3 direction = Vector3.Slerp (direction1, direction2, ratio);
			Ray ray = new Ray(center, direction);
			RaycastHit hit;
			// l = sqrt(4.25) = sqrt(0.5^2 + 2^2) = 2.06
			//     /|\  
			// l->/ | \  h = 2
			//   /  |  \ 
			//   -------
			//    b = 1

			col.Raycast (ray, out hit, 5.06f * delta);
			Vector3 position = hit.point;
			//print (position);
			position += (Vector3.up) * 0.5f;
			positions.Add (position);
		}
	}
}
