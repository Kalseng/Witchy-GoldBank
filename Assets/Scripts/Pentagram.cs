using UnityEngine;
using System.Collections;

public class Pentagram : MonoBehaviour {
	// these arrays should all have the same length:
	int[] itemQuantities = {0, 0, 0};
	public int[] neededItemQuantities;
	string[] itemNames = { "plant", "chair", "child" };
	string[] itemPlurals = { "plants", "chairs", "children" };
	int[] itemValues = { 200, 150, 5 };

	// Could be triggered by a bounds check when setting down an item
	void addItem (GameObject item) {
		int itemID = getID (item);
		itemQuantities [itemID]++;
		GameObject.Find ("InGameGUI").GetComponent<InGameGUI> ().addGold (itemValues [itemID]);
		Destroy (item);
		if (checkForItemWin())
			GameObject.Find ("GameManager").GetComponent<GameManager> ().WinRound ();
	}

	bool checkForItemWin () { // currently set up for required items, but can be modified for a gold requirement
		for (int i = 0; i < itemQuantities.Length; i++) {
			if (itemQuantities[i] < neededItemQuantities[i])
				return false;
		}
		return true;
	}

	// not sure the best way to do this, maybe from the item GameObject's name?
	int getID(GameObject item) {
		return 0;
	}
}
