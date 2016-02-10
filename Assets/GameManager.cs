using UnityEngine;
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
		Time.timeScale = 0;
	}

	public void WinRound(){
		Debug.Log ("ROUND WON");
		Time.timeScale = 0;
	}

	// to be called after level is loaded?
	public void StartRound(int startGold) {
		// GUI code may be better located elsewhere, I just made a prefab with a separate script for now
		GameObject gui = Instantiate (Resources.Load ("InGameGUI")) as GameObject;
		gui.name = "InGameGUI";
		InGameGUI guiCode = gui.GetComponent<InGameGUI> ();
		guiCode.gold = startGold;
		guiCode.mode = "roundstart";
	}
}
