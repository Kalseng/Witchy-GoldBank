using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameGUI : MonoBehaviour {
	private int gold = 0;
	public int neededGold;
	public GameObject goldCounter;

	// raw material IDs: 0 = wood, 1 = soil, 2 = human
	private int[] hasMaterials = new int[3];
	private static int[] materialValues = { 4, 1, 25 };

	void Start() {

	}

	public void addGold(int amount) {
		gold += amount;
		goldCounter.GetComponent<Text> ().text = "Gold: " + gold;
	}

	public void maybeWin() {
		//print ("maybe win");
		if (gold >= neededGold) {
			//print ("winnn");
			GameObject.Find ("GameManager").GetComponent<GameManager> ().WinRound ();
		}
	}

	public void addMaterial(int materialID, int quantity) {
		hasMaterials [materialID] += quantity;
	}

	public void convertAll() {
		for (int i = 0; i < hasMaterials.Length; i++) {
			addGold (hasMaterials [i] * materialValues [i]);
			hasMaterials [i] = 0;
		}
		maybeWin ();
	}

}
