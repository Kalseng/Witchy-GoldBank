using UnityEngine;
using System.Collections;
// This script is probably temporary

public class LevelStarter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find ("GameManager").GetComponent<GameManager> ().StartRound (1000);
	}
}
