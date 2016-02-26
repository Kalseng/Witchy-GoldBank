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
	private bool adjusted = false;
	private Transform collWithItem = null;
	public float BACKUP_DIST;
	private bool targetsIncrement = true;

	private bool alerted = false;
	private float alertTime;
	public GameObject cone;

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
		} else if (!alerted) {
			Vector3 deltaPosition = targetPos - transform.position;
			float dPMag = deltaPosition.magnitude;
			if (dPMag <= TARGET_CLOSENESS) {
				adjusted = false;
				nextTarget ();
				targetPos = targets [currentTarget].position;
				prepRotation (targetPos);
				return;
			}

			transform.rotation = Quaternion.LookRotation (Vector3.forward, deltaPosition);
			Vector3 movement = (forward.position - transform.position).normalized;
			obstacle = Physics2D.Raycast (transform.position, movement, AVOID_DISTANCE, mask);
			if (obstacle) {
				//print ("Avoiding " + obstacle.transform.name);
				if (!adjusted)
					prevTarget ();
				adjusted = true;
				Vector3 deltaPos = obstacle.transform.position - transform.position;
				Vector3 avoidPosLeft = deltaPos + (right.position - transform.position) * -SIDE_DISTANCE;
				RaycastHit2D leftTest = Physics2D.Raycast (transform.position, avoidPosLeft.normalized, avoidPosLeft.magnitude, mask);
				if (!leftTest) {
					avoid (avoidPosLeft);
				} else {
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
		} else {
			this.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
			alertTime -= Time.deltaTime;
			if (alertTime <= 0) {
				alertOff ();
			}
		}
	}

	void avoid(Vector3 extraPoint) {
		targetPos = extraPoint + transform.position;
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

	void OnCollisionEnter2D (Collision2D col) {
		if (col.transform.gameObject.layer != 9) { // NPC Ignore Layer number
			if (collWithItem) {
				Vector3 badPoint = 0.5f * (collWithItem.position + col.transform.position);
				avoid ((2 * transform.position - badPoint).normalized * BACKUP_DIST);
				targetsIncrement = !targetsIncrement;
			} else {
				collWithItem = col.transform;
			}
		}
		if (col.transform.CompareTag("Player")) {
			if (col.transform.GetComponent<PlayerController> ().IsSuspicious ()) {
				print ("What was that?");
				alertOn(GameObject.FindWithTag ("Player").transform, false);
			} else {
				GetComponentInChildren<TalkBubble> ().sayThing ("Excuse me, miss.", 2.0f);
			}
		}
	}

	void OnCollisionExit2D (Collision2D col) {
		if (col.transform.gameObject.layer != 9) { // NPC Ignore Layer number
			collWithItem = null;
		}
	}

	void prevTarget() {
		currentTarget += (targetsIncrement ? -1 : 1);
		if (currentTarget >= targets.Length)
			currentTarget = 1;
		else if (currentTarget <= 0)
			currentTarget = targets.Length - 1;
	}

	void nextTarget() {
		currentTarget += (targetsIncrement ? 1 : -1);
		if (currentTarget >= targets.Length)
			currentTarget = 1;
		else if (currentTarget <= 0)
			currentTarget = targets.Length - 1;
	}

	public void alertOn (Transform player, bool red) {
		prepRotation (player.position);
		alerted = true;
		transform.GetComponentInChildren<VisionCone> ().coneColor (red ? 2 : 1);
		alertTime = 2.5f;
	}

	public void alertOff () {
		if (alerted) {
			prepRotation (targetPos);
			transform.GetComponentInChildren<VisionCone> ().coneColor (0);
			alerted = false;
		}
	}

}
