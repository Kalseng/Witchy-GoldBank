using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

	public Text[] menuOptions;

	private int SelectionIndex;

	private float lastSelectTime = -5.0f;
	private float selectDelay = 0.25f;

	// Use this for initialization
	void Start () {
		SelectionIndex = 0;
		UpdateMenu ();

	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetAxisRaw ("Vertical") != 0 && Time.time - lastSelectTime > selectDelay) {
			lastSelectTime = Time.time;
			SelectionIndex -= (int)Input.GetAxisRaw ("Vertical");
			if (SelectionIndex < 0) {
				SelectionIndex = menuOptions.Length - 1;
			} else if (SelectionIndex > menuOptions.Length - 1) {
				SelectionIndex = 0;
			}
			UpdateMenu ();
		}
		if (Input.GetAxisRaw ("Vertical") == 0) {
			lastSelectTime = -5.0f;
		}

		if (Input.GetButtonDown ("Fire1")) {
			switch (SelectionIndex) {

			case 0:
				SceneManager.LoadScene ("Tutorial Scene");
				break;
			case 1:
				Debug.Log ("Credits");
				break;

			case 2:
				Application.Quit ();
				Debug.Log ("Quit");
				break;

			}
		}

	}

	void UpdateMenu(){
		for (int i = 0; i < menuOptions.Length; i++) {
			menuOptions [i].fontStyle = FontStyle.Normal;
		}
		menuOptions [SelectionIndex].fontStyle = FontStyle.Bold;
	}
}
