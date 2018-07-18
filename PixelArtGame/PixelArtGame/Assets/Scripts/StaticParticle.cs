using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticParticle : MonoBehaviour {

	public float sortOrderOffset;

	void Start () {
		GetComponent<ParticleSystemRenderer>().sortingOrder = (int)((transform.position.y + sortOrderOffset) * GameManager.instance.sortingOrderPrecision);
	}
}
