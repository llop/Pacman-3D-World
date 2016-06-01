using UnityEngine;

[RequireComponent (typeof (BoxCollider))]
public class LethalZone : MonoBehaviour
{
	EffectAction action = null;

	public void Start() {
		action = GetComponent<EffectAction> ();
	}

	public void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == Tags.Pacman && GameManager.Instance.pacmanData.alive) {
			if (action != null) {
				action.triggerAction ();
			}
			GameManager.Instance.killPacman();
		}
	}

	public void OnDrawGizmos() {
		Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.Scale(transform.lossyScale, GetComponent<BoxCollider>().size));
		Gizmos.color = new Color(1, 0, 0, 0.5F);
		Matrix4x4 tmp = Gizmos.matrix;
		Gizmos.matrix *= rotationMatrix;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
		Gizmos.matrix = tmp;
	}
}