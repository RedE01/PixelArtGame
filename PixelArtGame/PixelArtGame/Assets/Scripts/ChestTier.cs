using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chest Tier", menuName = "ChestTier", order = 2)]
public class ChestTier : ScriptableObject {
	public Item[] item;
	[Range(0, 100)]
	public float[] percentPerSlot;
}
