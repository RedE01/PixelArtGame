using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : MonoBehaviour {

	public void FollowCursor() {
		transform.position = Input.mousePosition;
	}
}
