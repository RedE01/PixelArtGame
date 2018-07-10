using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public Vector2 mousePos;

	public int sortingOrderPrecision = -100;
	public float time = 12;
	public float health = 100;
	public float coins = 0;

	private float timeSpeed = 1f / 60f;

	void Awake() {
		if(instance == null) {
			instance = this;
		}		
		else if(instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);

		sortingOrderPrecision = -100;
		health = 100;
		coins = 0;
		time = 12;
}

	void Update () {
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		time += Time.deltaTime * timeSpeed;
		if (time >= 24) time = 0;
	}
}
