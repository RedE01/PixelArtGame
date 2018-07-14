using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public Transform target;
	Vector2 pos, startShakePos;
	bool shake;
	float shakeMagnitude, shakeStrength, shakeTime, shakeSeedX, shakeSeedY;

	void Start() {
		pos = target.position;
	}

	void Update() {
		if(shake) {
			CameraShake();
		}
	}

	void LateUpdate() {
		transform.position = new Vector3(pos.x, pos.y, transform.position.z);

		pos.x = Mathf.Lerp(transform.position.x, target.position.x, 0.02f);
		pos.y = Mathf.Lerp(transform.position.y, target.position.y, 0.02f);
	}

	public void StartShake(float shakeMagnitude, float shakeStrength, float shakeTime) {
		this.shakeMagnitude = shakeMagnitude;
		this.shakeStrength = shakeStrength;
		this.shakeTime = shakeTime;

		shakeSeedX = Random.Range(0f, 1f);
		shakeSeedY = Random.Range(0f, 1f);

		startShakePos = pos;

		shake = true;
	}

	void CameraShake() {
		float xShake = Mathf.PerlinNoise(shakeSeedY + shakeTime * shakeStrength, shakeSeedX) * shakeMagnitude;
		float yShake = Mathf.PerlinNoise(shakeSeedX + shakeTime * shakeStrength, shakeSeedY) * shakeMagnitude;

		pos.x = startShakePos.x + xShake - shakeMagnitude * 0.5f;
		pos.y = startShakePos.y + yShake - shakeMagnitude * 0.5f;

		shakeTime -= Time.deltaTime;

		if (shakeTime <= 0) {
			shake = false;
		}
	}

}
