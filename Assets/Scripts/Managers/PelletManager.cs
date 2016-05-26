using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (WaypointGraph))]
public class PelletManager : MonoBehaviour {

	private WaypointGraph graph;
	private PlanetTopography topography;
	public GameObject pellet;
	public GameObject powerPellet;

	private HashSet<int> nodePellet;

	// Use this for initialization
	void Start () {
		graph = GetComponent<WaypointGraph> ();
		topography = GetComponent<PlanetTopography> ();

		nodePellet = new HashSet<int> ();
		int count = 0;
		foreach (WaypointEdge edge in graph.edges) {
			if (edge.one.pacmanWalkable && edge.two.pacmanWalkable) {
				List<Vector3> positions = new List<Vector3> ();
				topography.getPelletPositions (edge.one, edge.two, out positions);
				for (int i = 1; i < positions.Count -1; ++i) {
					Instantiate (pellet, positions[i], Quaternion.identity);
					count++;
				}
				if (!nodePellet.Contains (edge.one.GetInstanceID ())) {
					nodePellet.Add(edge.one.GetInstanceID ());
					GameObject p = pellet;
					if (edge.one.powerPellet) {
						p = powerPellet;
					}
					Instantiate (p, positions[0], Quaternion.identity);
					count++;
				}
				if (!nodePellet.Contains (edge.two.GetInstanceID ())) {
					nodePellet.Add(edge.two.GetInstanceID ());
					GameObject p = pellet;
					if (edge.two.powerPellet) {
						p = powerPellet;
					}
					Instantiate (p, positions[positions.Count-1], Quaternion.identity);
					count++;
				}
			}
		}
		GameManager.Instance.levelData.pelletsTotal = count;
	}
}
