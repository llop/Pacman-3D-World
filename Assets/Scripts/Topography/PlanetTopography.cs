using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class PlanetTopography : MonoBehaviour {



	public abstract void updatePlane(Vector3 origin, Vector3 target, ref Plane plane);
	public abstract void updateRotation(Transform body, Vector3 target);

	public abstract float calculateDistance(Vector3 origin, Vector3 target);

	public abstract void getUVCoords(WaypointNode one, out float uCoordA, out float vCoordA,
                                   WaypointNode two, out float uCoordB, out float vCoordB);

	public abstract void getPelletPositions (WaypointNode one, WaypointNode two, out List<Vector3> positions);
}
