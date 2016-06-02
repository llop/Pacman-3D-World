using UnityEngine;
using System.Collections;

public class ExplosionEffect : EffectAction {
	void Start() {
		GetComponent<ParticleSystem> ().Stop ();
	}

	override public void triggerAction() {
		GetComponent<ParticleSystem> ().Play ();
		GetComponent<AudioSource> ().Play ();
	}
}
