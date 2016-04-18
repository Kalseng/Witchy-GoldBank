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
	public GameObject PONZ_PORTRAIT;
	public GameObject CHAD_PORTRAIT;
	public GameObject WITCH_PORTRAIT;

	public void initNames() {
		NPCnames [Person.CHAD] = "Chad";
		NPCnames [Person.PONZ] = "Pon Z Dragon";
		NPCnames [Person.STOCK] = "Peasant";
		NPCnames [Person.WITCH] = "Witch";
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
		chatBar.GetComponent<Text> ().text = "<b>" + NPCnames [speaker] + ":</b> " + quote;
		chatBar.transform.parent.gameObject.SetActive (true);
		goldCounter.transform.parent.gameObject.SetActive (false);
		if (Camera.main.WorldToScreenPoint (GameObject.FindGameObjectWithTag ("Player").transform.position).y < Screen.height * 0.3f)
			print ("top"); // move canvas to top of screen somehow
		else
			print ("bottom"); // move canvas to bottom of screen somehow
		PONZ_PORTRAIT.SetActive (speaker == Person.PONZ);
		CHAD_PORTRAIT.SetActive (speaker == Person.CHAD);
		WITCH_PORTRAIT.SetActive (speaker == Person.WITCH);
	}

	public void hideChatBar() {
		hideChatBar (true);
	}

	public void hideChatBar(bool affectGoldCounter) {
		chatBar.transform.parent.gameObject.SetActive (false);
		if (affectGoldCounter)
			showGold ();
	}

	public void showGold() {
		goldCounter.transform.parent.gameObject.SetActive (true);
	}

}
