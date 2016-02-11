using UnityEngine;
using System.Collections;

public class Disappear : MonoBehaviour {

	private Vector3 originalScale;
	private float DISAPPEAR_TIME = 1.0f;
	private float timeLeft;

	void Start () {
		timeLeft = DISAPPEAR_TIME;
		originalScale = transform.localScale;
	}

	void Update () {
		timeLeft -= Time.deltaTime;
		if (timeLeft > 0)
			transform.localScale = originalScale * (timeLeft / DISAPPEAR_TIME);
		else
			Destroy (gameObject);
	}
}
