using UnityEngine;
using System.Collections;

public class ShaderWidthHeight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Material mat = GetComponent<MeshRenderer> ().material;

		mat.SetFloat ("width", transform.lossyScale.x);
		mat.SetFloat ("height", transform.lossyScale.z);
	}
}
