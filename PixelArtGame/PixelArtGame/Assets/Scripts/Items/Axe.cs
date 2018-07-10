using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Axe", menuName = "Axe", order = 2)]
public class Axe : Item {

	void Awake() {
		itemType = ItemType.Axe;
	}

}
