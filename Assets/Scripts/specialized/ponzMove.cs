using UnityEngine;
using System.Collections;

public class ponzMove : MonoBehaviour {
	private float elapsedTime = 0f;
	private bool begun = false;
	public ParticleSystem smoke;
	private bool step1 = false;
	public Transform newPos;

	// Use this for initialization
	public void begin () {
		begun = true;
		smoke.Play ();
	}

	// Update is called once per frame
	void Update () {
		if (begun) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 0.3f) {
				transform.position = newPos.position;
				transform.rotation = newPos.rotation;
				smoke.Play ();
				//Destroy (smoke.gameObject);
				Destroy (GetComponent<ponzMove> ());
			}
		}
	}
}
