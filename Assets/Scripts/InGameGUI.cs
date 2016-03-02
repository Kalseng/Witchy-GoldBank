using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameGUI : MonoBehaviour {
	private int gold = 0;
	public int neededGold;
	public GameObject goldCounter;
	public GameObject chatBar;

	// raw material IDs: 0 = wood, 1 = soil, 2 = human
	private int[] hasMaterials = new int[3];
	private static int[] MATERIAL_VALUES = { 4, 1, 25 };
	private Hashtable NPCnames = new Hashtable();

	void Start() {
		NPCnames [Person.CHAD] = "Chad";
		NPCnames [Person.PONZ] = "Pon Z Dragon";
		NPCnames [Person.STOCK] = "Peasant";
		NPCnames [Person.WITCH] = "Whatever the witch's name is";
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
			addGold (hasMaterials [i] * MATERIAL_VALUES [i]);
			hasMaterials [i] = 0;
		}
		maybeWin ();
	}

	public void chatBarQuote(string quote, Person speaker) {
		chatBar.GetComponent<Text> ().text = NPCnames [speaker] + ": " + quote;
		chatBar.transform.parent.gameObject.SetActive (true);
	}

	public void hideChatBar() {
		chatBar.transform.parent.gameObject.SetActive (false);
	}

}
