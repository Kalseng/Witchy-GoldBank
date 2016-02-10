using UnityEngine;
using System.Collections;

public class InGameGUI : MonoBehaviour {
	public string mode;
	public int gold;

	void OnGUI() { // GUI is still in 'ugly' phase
		switch (mode) {
		case "roundstart":
			GUI.Box (new Rect (0, 0, 300, 75), "We need...");
			// list of items needed
			GUI.Box (new Rect (0, 75, 300, 75), "[stuff]");
			if (GUI.Button (new Rect (0, 150, 300, 75), "Start round")) {
				mode = "none";
				// make everything start moving / working
			}
			break;
		}
		GUI.Box (new Rect (0, Screen.height - 50, 200, 50), "Gold: " + gold);
	}

	public void addGold(int amount) {
		gold += amount;
		// check for round win, if gold is the criteria
	}

}
