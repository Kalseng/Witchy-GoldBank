using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GameOver(){
		Debug.Log ("GAME OVER");
		Time.timeScale = 0;
	}
}
