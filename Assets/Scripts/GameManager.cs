﻿using UnityEngine;
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

	public void WinRound(){
		Debug.Log ("ROUND WON");
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
