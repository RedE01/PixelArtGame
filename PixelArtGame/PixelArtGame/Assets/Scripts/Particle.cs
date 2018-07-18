using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {

	ParticleSystemRenderer psr;

	void Start() {
		psr = GetComponent<ParticleSystemRenderer>();
	}

	void LateUpdate() {
		psr.sortingOrder = (int)((transform.position.y - 0.5f) * GameManager.instance.sortingOrderPrecision);
	}
}
