using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class PillTopography : PlanetTopography {



  protected static float planetRadius = 10f;



  //------------------------------------------------------------------------
  // get the plane towards which the character should move move
  //------------------------------------------------------------------------

  public override void updatePlane(Vector3 origin, Vector3 target, ref Plane plane) {
    Vector3 center = new Vector3(0f, target.y, 0f);
    Vector3 vecA = target - center;
    Vector3 vecB = origin - center;
    Vector3 rVec = Vector3.Cross(vecA, vecB);
    Vector3 uVec = Vector3.Cross(center - target, rVec);
    plane.SetNormalAndPosition(uVec.normalized, target);
  }



  //------------------------------------------------------------------------
  // have the character face the target
  //------------------------------------------------------------------------

  public override void updateRotation(Transform body, Vector3 target) {
    Vector3 bodyUp = new Vector3(body.position.x, 0f, body.position.z);
    body.rotation = Quaternion.LookRotation(target - body.position, bodyUp.normalized);
    // correct up vector
    body.rotation = Quaternion.FromToRotation(body.up, bodyUp.normalized) * body.rotation;
  }



  public override float calculateDistance(Vector3 origin, Vector3 target) {
    return Mathf.Abs(Vector3.Angle(origin, target) * Mathf.Deg2Rad * planetRadius);
  }


  // project one and two on a plane, and get their UV coords
  // this plane's center is on the point between one and two
  // its normal is pointing at the origin
  // and its right vector is on the XZ plane
  public override void getUVCoords(WaypointNode one, out float uCoordA, out float vCoordA,
    WaypointNode two, out float uCoordB, out float vCoordB) {

    Vector3 center = (one.transform.position + two.transform.position) / 2f;
    Vector3 planeRight = Vector3.Cross(Vector3.up, center.normalized);
    Vector3 planeUp = Vector3.Cross(planeRight.normalized, center.normalized);

    Vector3 dA = two.transform.position - center;
    uCoordA = Vector3.Dot(dA, planeRight);
    vCoordA = Vector3.Dot(dA, planeUp);

    Vector3 dB = one.transform.position - center;
    uCoordB = Vector3.Dot(dB, planeRight);
    vCoordB = Vector3.Dot(dB, planeUp);
  }


	public override void getPelletPositions (WaypointNode one, WaypointNode two, out List<Vector3> positions) {
		positions = new List<Vector3> ();
		int distance = (int)calculateDistance (one.transform.position, two.transform.position);
    Vector3 center = new Vector3(0f, (one.transform.position.y + two.transform.position.y) / 2f, 0f);
    Vector3 direction1 = (one.transform.position - center).normalized;
    Vector3 direction2 = (two.transform.position - center).normalized;

		Collider col = GetComponent<Collider> ();

		for (int i = 0; i <= distance; i++) {
			float ratio = (float)i/(float)distance;
			Vector3 direction = Vector3.Slerp (direction1, direction2, ratio);
      Ray ray = new Ray(center, direction);
			// reverse ray
			ray.origin = ray.GetPoint (2 * planetRadius);
			ray.direction = -direction;
			RaycastHit hit;
			col.Raycast (ray, out hit, 2 * planetRadius);
			Vector3 position = hit.point;
			//print (position);
			position += hit.normal * 0.25f;
			positions.Add (position);
		}
	}
}
