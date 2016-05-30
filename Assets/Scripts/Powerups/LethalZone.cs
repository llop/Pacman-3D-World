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
		Gizmos.color = new Color(1, 0, 0, 0.5F);
		Gizmos.DrawCube(transform.position, Vector3.Scale(transform.localScale, GetComponent<BoxCollider>().size));
	}
}