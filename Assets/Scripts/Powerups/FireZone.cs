using UnityEngine;
using System.Collections;

public class FireZone : MonoBehaviour {
	public float initialDelay = 0;
	public float activeTime = 1;
	public float inactiveTime = 1;

	private float ellapsedTime = 0;
	enum State { Initial, Playing, Waiting };
	private State state;
	private BoxCollider coll;
	private ParticleSystem particles;
	private AudioSource audio;

	public void Start() {
		coll = GetComponent<BoxCollider> ();
		state = State.Initial;
		particles = GetComponent<ParticleSystem> ();
		audio = GetComponent<AudioSource> ();
		particles.Stop ();
	}

	public void Update() {
		ellapsedTime += Time.deltaTime;
		if (state == State.Initial && ellapsedTime >= initialDelay) {
			ellapsedTime -= initialDelay;
			state = State.Playing;
		} else if (state == State.Playing && ellapsedTime >= activeTime) {
			ellapsedTime -= activeTime;
			state = State.Waiting;
		} else if (state == State.Waiting && ellapsedTime >= inactiveTime) {
			ellapsedTime -= inactiveTime;
			state = State.Playing;
		}

		if (state == State.Playing) {
			coll.enabled = true;
			particles.Play ();
			if (!audio.isPlaying)
				audio.Play ();
		} else {
			coll.enabled = false;
			particles.Stop ();
			audio.Stop ();
		}
	}
}