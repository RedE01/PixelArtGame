using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public Vector2 mousePos;

	void Awake() {
		if(instance == null) {
			instance = this;
		}		
		else if(instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	void Update () {
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
