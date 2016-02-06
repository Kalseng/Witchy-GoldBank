using UnityEngine;
using System.Collections;

public class menu : MonoBehaviour {
	float width;
	float height;
	int page = 0;
	// Use this for initialization
	void Start () {
		width = Screen.width;
		height = Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI () {
		switch (page) {
		case 0: // main menu
			GUI.Label(new Rect(0,200,width,200), "WITCHIE GOLDBANK - MAIN MENU");
			if (GUI.Button (new Rect (0, 0, 200, 200), "Start"))
				print ("Load game level");
			if (GUI.Button (new Rect (200, 0, 200, 200), "How to play"))
				page = 1;
			if (GUI.Button (new Rect (400, 0, 200, 200), "Credits"))
				page = 2;
			if (GUI.Button (new Rect (600, 0, 200, 200), "Exit"))
				Application.Quit ();
			break;
		case 1: // how to play
			if (GUI.Button (new Rect (0, 0, 200, 200), "Back"))
				page = 0;
			GUI.Label(new Rect(0,200,width,200), "HOW TO PLAY");
			break;
		case 2: // credits
			if (GUI.Button (new Rect (0, 0, 200, 200), "Back"))
				page = 0;
			GUI.Label(new Rect(0,200,width,200), "CREDITS");
			break;
		}
	}

}
