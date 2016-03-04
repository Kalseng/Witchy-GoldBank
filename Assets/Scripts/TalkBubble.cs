using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TalkBubble : MonoBehaviour {
	public Material opaqueBubble;
	public Material transparentBubble;
	private bool isVisible = false;
	public float timeLeft = 0;
	private string followUp = "";
	public bool talkSkip = false;
	
	// Update is called once per frame
	void Update () {
		if (isVisible) {
			transform.parent.rotation = Quaternion.identity;
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0) {
				if (followUp.Length == 0) {
					isVisible = false;
					GetComponent<SpriteRenderer> ().enabled = false;
					GetComponentInChildren<Text> ().text = "";
				} else {
					string temp = followUp;
					followUp = "";
					sayThing (temp, 3);
				}
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
		if (followUp.Length != 0) {
			if (talkSkip) {
				timeLeft = 0;
				talkSkip = false;
			}
			return;
		}
		transform.parent.rotation = Quaternion.identity;
		isVisible = true;
		timeLeft = duration;
		GetComponentInChildren<Text> ().text = thing;
		GetComponent<SpriteRenderer> ().enabled = true;
	}

	public void sayThing2(string thing, string thing2, float duration) { // thing with a follow up (punchline?)
		followUp = thing2;
		transform.parent.rotation = Quaternion.identity;
		isVisible = true;
		timeLeft = duration;
		GetComponentInChildren<Text> ().text = thing;
		GetComponent<SpriteRenderer> ().enabled = true;
	}
}
