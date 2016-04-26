using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GameOver(){
		Debug.Log ("GAME OVER");
		SceneManager.LoadScene ("GameOver");
	}

	public void WinRound(int gold){
		Debug.Log ("ROUND WON");
		Dialogue d = GameObject.FindWithTag ("Player").GetComponent<Dialogue>();
		d.WIN_TEXT_QUOTE [1] = "$" + gold + ".";
		d.startTalk (d.WIN_TEXT_QUOTE, d.WIN_TEXT_SPEAKER);
		//SceneManager.LoadScene ("Win Screen"); // this is called from ToWinScene now
	}

	public void ToWinScreen() {
		SceneManager.LoadScene ("Win Screen");
	}

	// to be called after level is loaded?
	public void StartRound(int startGold, int endGold) {
		// GUI code may be better located elsewhere, I just made a prefab with a separate script for now
		/*GameObject gui = Instantiate (Resources.Load ("InGameGUI")) as GameObject;
		gui.name = "InGameGUI";*/
		InGameGUI guiCode = GameObject.Find("InGameGUI").GetComponent<InGameGUI> ();
		guiCode.neededGold = endGold;
		guiCode.addGold (startGold);
		guiCode.showGold ();
	}
}
