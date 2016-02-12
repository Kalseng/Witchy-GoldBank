using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameGUI : MonoBehaviour {
	private int gold = 0;
	public int neededGold;
	public GameObject goldCounter;

	void Start() {

	}

	void OnGUI() { // GUI is still in 'ugly' phase
		//GUI.Box (new Rect (0, Screen.height - 50, 200, 50), "Gold: " + gold);
	}

	public void addGold(int amount) {
		gold += amount;
		goldCounter.GetComponent<Text> ().text = "Gold: " + gold;
		maybeWin ();
	}

	public void maybeWin() {
		//print ("maybe win");
		if (gold >= neededGold) {
			//print ("winnn");
			GameObject.Find ("GameManager").GetComponent<GameManager> ().WinRound ();
		}
	}

}
