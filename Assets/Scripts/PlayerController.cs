using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float movementModifier = 1.0f;

	private GameObject currentlySelectedItem;
	private GameObject currentlyHeldItem;
	private ArrayList possibleItems;

	private GameObject itemHolder;

	private GameObject itemFolder;

	private GameObject pentagram;
	private double minPentagramDistance;

	// Use this for initialization
	void Start () {
		possibleItems = new ArrayList ();
		itemHolder = this.transform.FindChild ("ItemPosition").gameObject;
		itemFolder = GameObject.Find ("ITEMS");
		pentagram = GameObject.Find ("Pentagram");
		minPentagramDistance = pentagram.GetComponent<CircleCollider2D> ().radius;
	}
	
	// Update is called once per frame
	void Update () {
		InputManager ();
		ItemManager ();
	}

	void InputManager(){
		MovementManager ();
		if (Input.GetButtonDown ("Fire1")) {
			if (currentlyHeldItem != null) {
				currentlyHeldItem.transform.parent = itemFolder.transform;
				while (possibleItems.Contains(currentlyHeldItem)) // in case the item is in possibleItems multiple times (bugfix)
					possibleItems.Remove (currentlyHeldItem);
				Item properties = currentlyHeldItem.GetComponent<Item> ();
				if (properties)
					movementModifier /= properties.speedMod;

				if ((pentagram.transform.position - currentlyHeldItem.transform.position).magnitude <= minPentagramDistance) {
					pentagram.GetComponent<Pentagram> ().addItem (currentlyHeldItem);
					currentlySelectedItem = null;
				} else {
					currentlyHeldItem.GetComponent<Collider2D> ().isTrigger = false;
					possibleItems.Add (currentlyHeldItem);
				}
				
				currentlyHeldItem = null;
			} else if (currentlySelectedItem) {
				currentlyHeldItem = currentlySelectedItem;
				currentlyHeldItem.transform.parent = itemHolder.transform;
				currentlyHeldItem.GetComponent<Collider2D> ().isTrigger = true;
				currentlyHeldItem.transform.localPosition = new Vector3 (0, 0, 0);
				Item properties = currentlyHeldItem.GetComponent<Item> ();
				if (properties)
					movementModifier *= properties.speedMod;
			}
		}
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
		}// end movement axis if statement
	}

	void ItemManager(){
		if (possibleItems.Count == 0) {
			currentlySelectedItem = null;	
			return;
		}
		if (possibleItems.Count == 1) {
			currentlySelectedItem = possibleItems [0] as GameObject;
			return;
		} else {
			GameObject closestItem = possibleItems[0] as GameObject;
			float closestDist = 1000000;
			foreach (GameObject obj in possibleItems) {
				float currentDist = (this.transform.position - obj.transform.position).sqrMagnitude;
				if (currentDist < closestDist) {
					closestDist = currentDist;
					closestItem = obj;
				}
			}
			currentlySelectedItem = closestItem;
		}
		if (currentlySelectedItem != null) {
			Debug.Log (currentlySelectedItem.name);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Item") {
			possibleItems.Add (col.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.tag == "Item") {
			possibleItems.Remove (col.gameObject);
		}
	}

	public bool IsSuspicious(){
		if (currentlyHeldItem != null) {
			return true;
		}


		return false;
	}

}
