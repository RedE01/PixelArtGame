using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

	public enum PartOfDay {
		Morning,
		Day,
		Evening,
		Night
	}
	public PartOfDay partOfDay = PartOfDay.Day; 

	public Text timeText;
	[Range(0, 1)]
	public float maxSunlight, minSunlight;
	[Range(0, 24)]
	public int sunrise, sunset, sunriseTime, sunsetTime;

	private float sunsetEndUnclamped, sunriseEnd, sunsetEnd;

	Light sun;

	void Start() {
		sun = GetComponent<Light>();

		sunsetEndUnclamped = sunset + sunsetTime;
		sunsetEndUnclamped = sunsetEndUnclamped - 24f;
		sunriseEnd = sunrise + sunriseTime;
		sunsetEnd = sunset + sunsetTime;
	}

	void Update() {
		float time = (GameManager.instance.day - Mathf.Floor(GameManager.instance.day)) * 24f + 12f;
		timeText.text = ((int)time).ToString();

		if (time >= sunsetEndUnclamped && time < sunrise) partOfDay = PartOfDay.Night;
		if (time >= sunrise && time < sunriseEnd) partOfDay = PartOfDay.Morning;
		if (time >= sunriseEnd && time < sunset) partOfDay = PartOfDay.Day;
		if (time >= sunset && time < sunsetEnd || time >= sunset - 24 && time < sunsetEndUnclamped) partOfDay = PartOfDay.Evening;

		float intensity = 0;
		switch(partOfDay) {
			case PartOfDay.Morning:
				intensity = (time - sunrise) / sunriseTime;
				intensity = intensity * (maxSunlight - minSunlight) + minSunlight;
				break;
			case PartOfDay.Day:
				intensity = maxSunlight;
				break;
			case PartOfDay.Evening:
				if (sunsetEnd > 24 && time < sunset) time += 24;
				intensity = 1 - (time - sunset) / sunsetTime;
				intensity = intensity * (maxSunlight - minSunlight) + minSunlight;
				break;
			case PartOfDay.Night:
				intensity = minSunlight;
				break;
		}
		sun.intensity = intensity;
	}

}
