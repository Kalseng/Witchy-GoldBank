using UnityEngine;
using System.Collections;

public class ChadAppears : MonoBehaviour {
	public GameObject ponz;
	public GameObject witch;
	public ParticleSystem smoke;
	private double elapsedTime = 0f;
	private bool begun = false;

	// Use this for initialization
	public void begin () {
		smoke.Play ();
		GetComponent<SpriteRenderer>().enabled = true;
		begun = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (begun) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 2f) {
				witch.GetComponent<Dialogue> ().endSceneEvent ();
				Destroy (smoke.gameObject);
				Destroy (GetComponent<ChadAppears> ());
			}
		}
	}
}
