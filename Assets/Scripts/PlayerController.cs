using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float movementModifier = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		InputManager ();
	}

	void InputManager(){
		if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
			Vector3 movement = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"), 0) * movementModifier;
			this.GetComponent<Rigidbody2D>().velocity = movement;
		}
	}
}
