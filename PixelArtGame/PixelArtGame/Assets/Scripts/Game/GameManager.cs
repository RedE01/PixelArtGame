using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public Vector2 mousePos;

	public int sortingOrderPrecision = -100;
	public float day;
	public float health = 100;
	public float coins = 0;

	private float dayTime = 1f / (60f * 10f);

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
		day = 0.5f;
}

	void Update () {
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		day += Time.deltaTime * dayTime;
	}
}
