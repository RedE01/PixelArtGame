using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 0)]
public class Item : ScriptableObject {

	public new string name;
	public Sprite sprite;

	[Range(0.4f, 2)]
	public float pickupRadius = 0.4f;
}
