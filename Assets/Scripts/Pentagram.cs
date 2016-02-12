using UnityEngine;
using System.Collections;

public class Pentagram : MonoBehaviour {
	// these arrays should all have the same length:
	string[] itemNames = { "plant", "chair", "child" };
	int[] itemValues = { 200, 150, 5 };

	// Could be triggered by a bounds check when setting down an item
	public void addItem (GameObject item) {
		// print ("adding item");
		int itemID = getID (item);
		GameObject.Find ("InGameGUI").GetComponent<InGameGUI> ().addGold (itemValues [itemID]);
		item.AddComponent<Disappear> ();
	}

	// not sure the best way to do this
	int getID(GameObject item) {
		for (int i = 0; i < itemNames.Length; i++) {
			if (item.transform.name == itemNames [i])
				return i;
		}
		return 0; // in case the item's name isn't found
	}

}
