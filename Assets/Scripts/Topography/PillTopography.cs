using UnityEngine;
using System.Collections;



public class PillTopography : PlanetTopography {



  protected static float planetRadius = 20f;



  //------------------------------------------------------------------------
  // get the plane towards which the character should move move
  //------------------------------------------------------------------------

  public override void updatePlane(Vector3 origin, Vector3 target, ref Plane plane) {
    Vector3 dVec = target.normalized;
    Vector3 xPoint = Vector3.Dot(origin, dVec) * dVec;
    plane.SetNormalAndPosition(origin - xPoint, xPoint);

    //Vector3 center = new Vector3(0f, target.y, 0f);
    //Vector3 vecA = target - center;
    //Vector3 vecB = origin - center;
    //float angle = Vector3.Angle(vecA, vecB);
    //float length = vecA.magnitude / Mathf.Cos(angle * Mathf.Deg2Rad);
    //Vector3 xPoint = vecB.normalized * length;
    //plane.SetNormalAndPosition(xPoint - target, target);

  }



  //------------------------------------------------------------------------
  // have the character face the target
  //------------------------------------------------------------------------

  public override void updateRotation(Transform body, Vector3 target) {

    // face the next node
    Vector3 center = new Vector3(0f, body.position.y, 0f);
    Vector3 vecA = body.position - center;
    Vector3 vecB = target - center;
    float angle = Vector3.Angle(vecA, vecB);
    float length = vecA.magnitude / Mathf.Cos(angle * Mathf.Deg2Rad);
    Vector3 xPoint = vecB.normalized * length;
    body.LookAt(target);

    // correct up vector
    Vector3 targetDir = vecA.normalized;
    body.rotation = Quaternion.FromToRotation(body.up, targetDir) * body.rotation;

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



}
