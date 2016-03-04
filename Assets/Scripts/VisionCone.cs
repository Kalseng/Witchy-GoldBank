using UnityEngine;
using System.Collections;

public class VisionCone : MonoBehaviour {
	double alertLevel = 0f;
	public double ALERT_THRESHOLD;
	public double ALERT_MOD; // Scale factor for multiplying alert incrementation by
	bool seesPlayer = false;
	Transform player;
	public GameObject alertSound;

	private Sprite goodCone;
	private Sprite medCone;
	private Sprite badCone;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player").transform;
		goodCone = transform.parent.GetComponent<SpriteStore> ().sprites [0];
		medCone = transform.parent.GetComponent<SpriteStore> ().sprites [1];
		badCone = transform.parent.GetComponent<SpriteStore> ().sprites [2];
	}
	
	// Update is called once per frame
	void Update () {
		bool playerSuspicious = player.GetComponent<PlayerController>().IsSuspicious(); // will come from a variable in the player script
		if (seesPlayer && playerSuspicious) {
			if (alertLevel == 0) { // if this is the first frame the player is detected
				alertSound.GetComponent<AudioSource> ().Play ();
				transform.parent.parent.GetComponent<Patrol2> ().alertOn (player, true);
			}
			alertLevel += Time.deltaTime * ALERT_MOD / Mathf.Max((player.position - transform.position).magnitude,1);
			//print (alertLevel);
			if (alertLevel >= ALERT_THRESHOLD) {
				print ("Suspicion level critical");
				transform.parent.parent.GetComponent<Patrol2> ().alertOn (player, true);
				transform.parent.parent.GetComponentInChildren<TalkBubble> ().sayThing ("It's a witch!", 2.0f);
				GameObject.Find ("GameManager").GetComponent<GameManager> ().GameOver ();
			}
		}
	}

	void OnTriggerEnter2D (Collider2D maybePlayer) {
		if (maybePlayer.gameObject.CompareTag ("PlayerCollider") && !maybePlayer.isTrigger) {
			seesPlayer = true;
		} else if (maybePlayer.gameObject.CompareTag ("Item") && maybePlayer.isTrigger) {
			transform.parent.parent.GetComponent<Patrol2> ().alertOn (player, false);
		}
	}

	void OnTriggerExit2D (Collider2D maybePlayer) {
		if (maybePlayer.gameObject.CompareTag ("PlayerCollider")) {
			seesPlayer = false;
			if (player.GetComponent<PlayerController>().IsSuspicious())
				transform.parent.parent.GetComponent<Patrol2> ().alertOn (player, false);
			alertLevel = 0f;
		}
	}

	public void coneColor(int colorID) { // 0 is good, 1 is medium suspicion, 2 is suspicious (bad)
		switch (colorID) {
		case 0:
			transform.parent.GetComponent<SpriteRenderer> ().sprite = goodCone;
			break;
		case 1:
			transform.parent.GetComponent<SpriteRenderer> ().sprite = medCone;
			break;
		case 2:
			transform.parent.GetComponent<SpriteRenderer> ().sprite = badCone;
			break;
		}
	}
}
