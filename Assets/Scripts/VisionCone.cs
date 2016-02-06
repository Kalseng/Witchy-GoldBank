using UnityEngine;
using System.Collections;

public class VisionCone : MonoBehaviour {
	double alertLevel = 0f;
	public double ALERT_THRESHOLD;
	public double ALERT_MOD; // Scale factor for multiplying alert incrementation by
	bool seesPlayer = false;
	Transform player;
	public GameObject alertSound;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		bool playerSuspicious = true; // will come from a variable in the player script
		if (seesPlayer && playerSuspicious) {
			if (alertLevel == 0) // if this is the first frame the player is detected
				alertSound.GetComponent<AudioSource>().Play();
			alertLevel += Time.deltaTime * ALERT_MOD / Mathf.Max((player.position - transform.position).magnitude,1);
			print (alertLevel);
			if (alertLevel >= ALERT_THRESHOLD) {
				print ("Suspicion level critical");

			}
		}
	}

	void OnTriggerEnter2D (Collider2D maybePlayer) {
		// print ("Triggered");
		// print (maybePlayer.transform.name);
		if (maybePlayer.gameObject.CompareTag ("Player")) {
			seesPlayer = true;
		}
	}

	void OnTriggerExit2D (Collider2D maybePlayer) {
		if (maybePlayer.gameObject.CompareTag ("Player")) {
			seesPlayer = false;
			alertLevel = 0f;
		}
	}
}
