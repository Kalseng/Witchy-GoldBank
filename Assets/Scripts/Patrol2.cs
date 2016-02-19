using UnityEngine;
using System.Collections;

public class Patrol2 : MonoBehaviour {
	public GameObject targetSet;
	private Transform[] targets;

	private int currentTarget = 1; // ignore #0, since that will be the parent of the actual targets
	private Vector3 targetPos;
	public float WALK_SPEED;
	public float TARGET_CLOSENESS;
	private Transform forward;
	private Transform right;

	private bool turning = false;
	private Quaternion fromRotation;
	private Quaternion toRotation;
	public float ROTATION_SPEED;
	private float rotationTime;
	private float rotationDuration;

	public float AVOID_DISTANCE;
	public float SIDE_DISTANCE;
	private RaycastHit2D obstacle;
	private LayerMask mask;

	// Use this for initialization
	void Start () {
		targets = targetSet.GetComponentsInChildren<Transform> ();
		forward = transform.Find ("forward");
		right = transform.Find ("right");
		mask = ~LayerMask.GetMask("NPC Ignore");
		targetPos = targets [currentTarget].position;
		prepRotation (targetPos);
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
			Vector3 deltaPosition = targetPos - transform.position;
			//print (deltaPosition.magnitude);
			float dPMag = deltaPosition.magnitude;
			if (dPMag <= TARGET_CLOSENESS) {
				currentTarget++;
				if (currentTarget >= targets.Length)
					currentTarget = 1;
				targetPos = targets [currentTarget].position;
				prepRotation (targetPos);
				return;
			}

			transform.rotation = Quaternion.LookRotation (Vector3.forward,deltaPosition);
			Vector3 movement = (forward.position - transform.position).normalized;
			obstacle = Physics2D.Raycast (transform.position, movement, AVOID_DISTANCE, mask);
			if (obstacle) {
				print (obstacle.transform.name);
				Vector3 deltaPos = obstacle.transform.position - transform.position;
				Vector3 avoidPosLeft = deltaPos + (right.position - transform.position) * -SIDE_DISTANCE;
				RaycastHit2D leftTest = Physics2D.Raycast (transform.position, avoidPosLeft.normalized, avoidPosLeft.magnitude, mask);
				if (!leftTest) {
					avoid (avoidPosLeft);
				}
				else {
					Vector3 avoidPosRight = deltaPos + (right.position - transform.position) * SIDE_DISTANCE;
					RaycastHit2D rightTest = Physics2D.Raycast (transform.position, avoidPosRight.normalized, avoidPosRight.magnitude, mask);
					if (!rightTest || rightTest.distance < leftTest.distance) {
						avoid (avoidPosRight);
					} else {
						avoid (avoidPosLeft);
					}
				}
			} else {
			//Debug.Log ("moving");

			movement = movement * WALK_SPEED;
			this.GetComponent<Rigidbody2D> ().velocity = movement;
			}
		}
	}

	void avoid(Vector3 extraPoint) {
		targetPos = extraPoint + transform.position;
		currentTarget--;
		prepRotation (targetPos);
	}

	void prepRotation (Vector3 toPos) {
		this.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;

		Vector3 deltaPosition = toPos - transform.position;
		fromRotation = transform.rotation;
		toRotation = Quaternion.LookRotation (Vector3.forward,deltaPosition);
		float angle = Quaternion.Angle (fromRotation, toRotation);

		rotationTime = 0;
		rotationDuration = angle / ROTATION_SPEED;
		turning = true;
	}

}
