using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TalkBubble : MonoBehaviour {
	public Material opaqueBubble;
	public Material transparentBubble;
	private bool isVisible = false;
	private float timeLeft;
	
	// Update is called once per frame
	void Update () {
		if (isVisible) {
			transform.parent.rotation = Quaternion.identity;
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0) {
				isVisible = false;
				GetComponent<SpriteRenderer> ().enabled = false;
				GetComponentInChildren<Text> ().text = "";
			}
		}
	}

	void OnTriggerEnter2D(Collider2D overlapObject) {
		if (overlapObject.transform.CompareTag("PlayerCollider")
			|| (overlapObject.transform.CompareTag("Item") && overlapObject.transform.root.CompareTag("Player")))
			GetComponent<SpriteRenderer> ().material = transparentBubble;
	}

	void OnTriggerExit2D(Collider2D overlapObject) {
		if (overlapObject.transform.CompareTag("PlayerCollider")
			|| (overlapObject.transform.CompareTag("Item") && overlapObject.transform.root.CompareTag("Player")))
			GetComponent<SpriteRenderer> ().material = opaqueBubble;
	}

	public void sayThing(string thing, float duration) {
		transform.parent.rotation = Quaternion.identity;
		isVisible = true;
		timeLeft = duration;
		GetComponentInChildren<Text> ().text = thing;
		GetComponent<SpriteRenderer> ().enabled = true;
	}
}
