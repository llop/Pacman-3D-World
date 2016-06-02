using UnityEngine;
using System.Collections;

public class RotarManivela : MonoBehaviour {

	public float rotationSpeed = 360;
	private MeshRenderer mesh;
	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 center = mesh.bounds.center;
		center.x /= transform.localScale.x;
		center.y /= transform.localScale.y;
		center.z /= transform.localScale.z;
		transform.RotateAround(center, transform.right, Time.deltaTime * rotationSpeed);
	}
}
