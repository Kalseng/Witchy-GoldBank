using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TalkBubble : MonoBehaviour {
	public Material opaqueBubble;
	public Material transparentBubble;
	private bool isVisible = false;
	public float timeLeft = 0;
	public string followUp = "";
	private bool hold = false;
	
	// Update is called once per frame
	void Update () {
		if (isVisible) {
			transform.parent.rotation = Quaternion.identity;
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0) {
				if (followUp.Length == 0) {
					hold = false;
					isVisible = false;
					GetComponent<SpriteRenderer> ().enabled = false;
					GetComponentInChildren<Text> ().text = "";
				} else {
					sayFollowUp ();
					print ("time up");
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

	public void sayThing(string thing, float duration, bool toHold, string thing2) {
		if (followUp.Length != 0 || hold)
			return;
		if (thing2.Length != 0)
			followUp = thing2;
		hold = toHold;
		transform.parent.rotation = Quaternion.identity;
		isVisible = true;
		timeLeft = duration;
		GetComponentInChildren<Text> ().text = thing;
		GetComponent<SpriteRenderer> ().enabled = true;
	}

	public void sayFollowUp() {
		string temp = followUp;
		followUp = "";
		hold = false;
		sayThing (temp, 3, true, "");
	}
}
