using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {

	RectTransform healthbarTransform;
	float width, height;

	void Start() {
		healthbarTransform = GetComponent<RectTransform>();

		width = healthbarTransform.sizeDelta.x;
		height = healthbarTransform.sizeDelta.y;
	}

	public void UpdateHealthBar(float health) {
		healthbarTransform.sizeDelta = new Vector2(width * health * 0.01f, height);
	}
}
