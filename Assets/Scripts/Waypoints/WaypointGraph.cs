using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent (typeof (PlanetTopography))]
public class WaypointGraph : MonoBehaviour {


  protected PlanetTopography topo;


  public List<WaypointEdge> edges;
  public WaypointNode root;



  public void Awake() {
    topo = GetComponent<PlanetTopography>();
    foreach (WaypointEdge edge in edges) {
      float uCoordA, vCoordA, uCoordB, vCoordB;
      topo.getUVCoords(edge.one, out uCoordA, out vCoordA, edge.two, out uCoordB, out vCoordB);
      linkNodes(edge.one, uCoordA, vCoordA, edge.two, uCoordB, vCoordB);
    }
  }


  protected void linkNodes(WaypointNode one, float uCoordA, float vCoordA,
                           WaypointNode two, float uCoordB, float vCoordB) {
    float diffU = Mathf.Abs(uCoordA - uCoordB);
    float diffV = Mathf.Abs(vCoordA - vCoordB);
    if (diffU > diffV) {
      if (uCoordA > uCoordB) { one.setLeft(two); two.setRight(one); }
      else { one.setRight(two); two.setLeft(one); }
    } else {
      if (vCoordA > vCoordB) { one.setBack(two); two.setFront(one); }
      else { one.setFront(two); two.setBack(one); }
    }
  }


}
