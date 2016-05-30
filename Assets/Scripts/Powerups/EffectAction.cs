using UnityEngine;

[RequireComponent (typeof (BoxCollider))]
public abstract class EffectAction : MonoBehaviour
{
	public abstract void triggerAction();
}