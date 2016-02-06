using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float movementModifier = 1.0f;

	public ItemComponent currentlySelectedItem;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		InputManager ();
	}

	void InputManager(){
		MovementManager ();
	}

	void MovementManager(){
		if (Input.GetAxisRaw ("Horizontal") != 0 || Input.GetAxisRaw ("Vertical") != 0) {
			Vector3 movement = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"), 0);
			movement.Normalize ();
			movement = movement * movementModifier;
			this.GetComponent<Rigidbody2D> ().velocity = movement;

			float rotAngle = Mathf.Atan2 (movement.y, movement.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis (rotAngle, Vector3.forward);
		} else {
			this.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, 0, 0);
		}
	}
}
