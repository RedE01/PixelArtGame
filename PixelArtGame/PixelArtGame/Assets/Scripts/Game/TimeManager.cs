using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {
	public Light sun;
	[Range(0, 1)]
	public float maxSunlight, minSunlight;
	[Range(0, 24)]
	public int sunrise, sunset, sunriseTime, sunsetTime;

	Text text;

	void Start() {
		text = GetComponent<Text>();
	}

	void Update() {
		float time = GameManager.instance.time;
		text.text = ((int)time).ToString();

		if(time > sunrise && time < sunrise + sunriseTime) {
			float t = (time - (float)sunrise) / (float)(sunriseTime);
			sun.intensity = Mathf.Lerp(minSunlight, maxSunlight, t);
		}
		if (time > sunset && time < sunset + sunsetTime) {
			float t = (time - (float)sunset) / (float)(sunsetTime);
			sun.intensity = Mathf.Lerp(maxSunlight, minSunlight, t);
		}
	}

}
