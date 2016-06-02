using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class WaypointNode : MonoBehaviour {


  public bool pacmanWalkable = true;
  public bool powerPellet = false;
	public WaypointNode twin = null;

	public bool doMagic = false;


  private WaypointNode front;
  private WaypointNode back;
  private WaypointNode left;
  private WaypointNode right;


  public WaypointNode getFront() { return front; }
  public WaypointNode getBack() { return back; }
  public WaypointNode getLeft() { return left; }
  public WaypointNode getRight() { return right; }

  public void setFront(WaypointNode node) { front = node; }
  public void setBack(WaypointNode node) { back = node; }
  public void setLeft(WaypointNode node) { left = node; }
  public void setRight(WaypointNode node) { right = node; }

  public WaypointNode getAdjacent(Direction direction) {
    if (direction == Direction.Front) return front;
    if (direction == Direction.Back) return back;
    if (direction == Direction.Left) return left;
    if (direction == Direction.Right) return right;
    return null;
  }


  public void Awake() {
		GetComponent<Renderer> ().enabled = false;
	}
	public void Start() {
		if (!doMagic)
			return;
		SphereCollider col = GetComponent<SphereCollider> ();
		WaypointGraph graph = GameObject.FindGameObjectWithTag ("Planet").GetComponent<WaypointGraph>();

		Ray ray;
		RaycastHit hit;
		WaypointNode other;
		WaypointEdge edge;
		string[] masks = {"Waypoint"};
		LayerMask mask = LayerMask.GetMask (masks);

		if (right == null) {
			if (Physics.Raycast(transform.position, Vector3.right, out hit, 50.0f, mask.value)) {
				other = hit.collider.gameObject.GetComponent<WaypointNode> ();
				if (other != null) {
					right = other;
					other.left = this;
					edge = new WaypointEdge ();
					edge.one = this;
					edge.two = other;
					graph.edges.Add (edge);
				}
			}
		}
		if (left == null) {
			ray = new Ray(transform.position, Vector3.left);
			if (Physics.Raycast(transform.position, Vector3.left, out hit, 50.0f, mask.value)) {
				other = hit.collider.gameObject.GetComponent<WaypointNode> ();
				if (other != null) {
					left = other;
					other.right = this;
					edge = new WaypointEdge ();
					edge.one = this;
					edge.two = other;
					graph.edges.Add (edge);
				}
			}
		}
		if (front == null) {
			ray = new Ray(transform.position, Vector3.forward);
			if (Physics.Raycast(transform.position, Vector3.forward, out hit, 50.0f, mask.value)) {
				other = hit.collider.gameObject.GetComponent<WaypointNode> ();
				if (other != null) {
					front = other;
					other.back = this;
					edge = new WaypointEdge ();
					edge.one = this;
					edge.two = other;
					graph.edges.Add (edge);
				}
			}
		}
		if (back == null) {
			ray = new Ray(transform.position, Vector3.back);
			if (Physics.Raycast(transform.position, Vector3.back, out hit, 50.0f, mask.value)) {
				other = hit.collider.gameObject.GetComponent<WaypointNode> ();
				if (other != null) {
					back = other;
					other.front = this;
					edge = new WaypointEdge ();
					edge.one = this;
					edge.two = other;
					graph.edges.Add (edge);
				}
			}
		}
	}
}
