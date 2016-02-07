using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour {
	public GameObject targetSet;
	private Transform[] targets;

	private int currentTarget = 1; // ignore #0, since that will be the parent of the actual targets
	public float WALK_SPEED;
	public float TARGET_CLOSENESS;
	private Transform forward;

	private bool turning = false;
	private Quaternion fromRotation;
	private Quaternion toRotation;
	public float ROTATION_SPEED;
	private float rotationTime;
	private float rotationDuration;

	// Use this for initialization
	void Start () {
		targets = targetSet.GetComponentsInChildren<Transform> ();
		forward = transform.Find ("forward");
		prepRotation (currentTarget);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (turning) {
			this.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
			rotationTime += Time.deltaTime;
			transform.rotation = Quaternion.Lerp (fromRotation, toRotation, rotationTime / rotationDuration);
			if (rotationTime >= rotationDuration) {
				turning = false;
			}
		} else {
			Vector3 deltaPosition = targets[currentTarget].position - transform.position;
			float dPMag = deltaPosition.magnitude;
			if (dPMag <= TARGET_CLOSENESS) {
				currentTarget++;
				if (currentTarget >= targets.Length)
					currentTarget = 1;
				prepRotation (currentTarget);
				return;
			}

			transform.rotation = Quaternion.LookRotation (Vector3.forward,deltaPosition);
			//Debug.Log ("moving");
			Vector3 movement = (forward.position - transform.position).normalized;
			movement = movement * WALK_SPEED;
			this.GetComponent<Rigidbody2D> ().velocity = movement;
		}
	}

	void prepRotation (int faceTowardIndex) {
		this.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;

		Vector3 deltaPosition = targets [faceTowardIndex].position - transform.position;
		fromRotation = transform.rotation;
		toRotation = Quaternion.LookRotation (Vector3.forward,deltaPosition);
		float angle = Quaternion.Angle (fromRotation, toRotation);

		rotationTime = 0;
		rotationDuration = angle / ROTATION_SPEED;
		turning = true;
	}

}
