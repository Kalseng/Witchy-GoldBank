using UnityEngine;
using System.Collections;

public class Patrol2 : MonoBehaviour {
	public GameObject targetSet;
	private Transform[] targets;

	private int currentTarget = 1; // ignore #0, since that will be the parent of the actual targets
	public Vector3 targetPos;
	public float WALK_SPEED;
	public float TARGET_CLOSENESS; // how close to the target is sufficient for targeting the next one
	private Transform forward;
	private Transform right;

	private bool turning = false;
	private Quaternion fromRotation;
	private Quaternion toRotation;
	public float ROTATION_SPEED;
	private float rotationTime;
	private float rotationDuration;

	public float AVOID_DISTANCE; // deals with raycasting obstacle avoidance
	public float SIDE_DISTANCE; // deals with setting avoidance target
	private RaycastHit2D obstacle;
	private LayerMask mask;
	private bool adjusted = false;
	private Transform collWithItem = null;
	public float BACKUP_DIST; // deals only with reversing direction when caught between objects
	public float MIN_APPROACH; // deals only with moving toward player when alerted
	private bool targetsIncrement = true;

	private bool alerted = false;
	private float alertTime;
	public GameObject cone;
	private float NPCdebounce = 0f;

	public bool talkFreeze = false;
	private bool approach = false;

	private static string[] NOTICE_MESSAGES = {"What're you doing?", "Excuse me? Miss?", "Where're you going with that?"};

	// Use this for initialization
	void Start () {
		targets = targetSet.GetComponentsInChildren<Transform> ();
		forward = transform.Find ("forward");
		right = transform.Find ("right");
		mask = ~LayerMask.GetMask("NPC Ignore");
		targetPos = targets [currentTarget].position;
		clampTargetPos ();
		prepRotation (targetPos);
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (talkFreeze)
			return;
		if (NPCdebounce > 0f)
			NPCdebounce -= Time.deltaTime;
		if (turning) {
			this.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
			rotationTime += Time.deltaTime;
			transform.rotation = Quaternion.Lerp (fromRotation, toRotation, rotationTime / rotationDuration);
			if (rotationTime >= rotationDuration) {
				turning = false;
				considerApproach ();
			}
		} else if (!alerted || approach) {
			Vector3 deltaPosition = targetPos - transform.position;
			float dPMag = deltaPosition.magnitude;
			if (dPMag <= TARGET_CLOSENESS) {
				adjusted = false;
				nextTarget ();
				targetPos = targets [currentTarget].position;
				clampTargetPos ();
				if (!alerted)
					prepRotation (targetPos);
				else {
					approach = false;
					considerApproach ();
				}
				return;
			}

			transform.rotation = Quaternion.LookRotation (Vector3.forward, deltaPosition);
			Vector3 movement = (forward.position - transform.position).normalized;
			obstacle = Physics2D.Raycast (transform.position, movement, AVOID_DISTANCE, mask);
			if (obstacle) {
				//print ("Avoiding " + obstacle.transform.name);
				if (!adjusted)
					prevTarget ();
				if (alerted) {
					alertOff();
					approach = false;
				}
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

	void considerApproach() {
		float distanceToPlayer = (GameObject.FindWithTag ("Player").transform.position - transform.position).magnitude;
		if (alerted && distanceToPlayer > MIN_APPROACH) {
			if (!GameObject.FindWithTag ("Player").GetComponent<PlayerController> ().IsSuspicious ()) {
				approach = false;
				alertOff ();
				return;
			}
			// move forward and approach
			approach = true;
			if (!adjusted)
				prevTarget ();
			adjusted = true;
			targetPos = (forward.position - transform.position) * (distanceToPlayer - MIN_APPROACH) * 2 + transform.position;
			clampTargetPos ();
		}
	}

	void avoid(Vector3 extraPoint) {
		targetPos = extraPoint + transform.position;
		clampTargetPos ();
		prepRotation (targetPos);
	}

	void clampTargetPos() { // make sure NPCs stay within certain bounds
		GameObject topleft = GameObject.FindWithTag ("NPCBound_topleft");
		GameObject bottomright = GameObject.FindWithTag ("NPCBound_bottomright");
		if (topleft == null || bottomright == null)
			print ("ASDFHU;WAEJ");
		float rad = GetComponent<CircleCollider2D> ().radius;
		if (topleft != null) {
			if (targetPos.y + rad > topleft.transform.position.y)
				targetPos.y = topleft.transform.position.y - rad;
			if (targetPos.x - rad < topleft.transform.position.x)
				targetPos.x = topleft.transform.position.x + rad;
		}
		if (bottomright != null) {
			if (targetPos.y - rad < bottomright.transform.position.y)
				targetPos.y = bottomright.transform.position.y + rad;
			if (targetPos.x + rad > bottomright.transform.position.x)
				targetPos.x = bottomright.transform.position.x - rad;
		}
	}

	private int nprep = 0;

	void prepRotation (Vector3 toPos) {
		//print ("prep" + nprep);
		nprep++;
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
		if (col.transform.gameObject.layer != 9) { // NPC Ignore Layer number is 9
			if (collWithItem) {
				Vector3 badPoint = 0.5f * (collWithItem.position + col.transform.position);
				avoid ((2 * transform.position - badPoint).normalized * BACKUP_DIST);
				targetsIncrement = !targetsIncrement;
				//print ("reversing");
			} else {
				collWithItem = col.transform;
			}
		}
		if (col.transform.CompareTag ("Player")) {
			if (col.transform.GetComponent<PlayerController> ().IsSuspicious ()) {
				//print ("What was that?");
				alertOn (GameObject.FindWithTag ("Player").transform, false);
			} else {
				GetComponentInChildren<TalkBubble> ().sayThing ("Excuse me, miss.", 2.0f, false, "");
			}
		} else if (col.transform.CompareTag ("NPC")) {
			if (NPCdebounce <= 0f) {
				approach = false;
				alertOff ();
				GetComponentInChildren<TalkBubble> ().sayThing ("Watch it!", 1.5f, false, "");
				NPCdebounce = 3.0f;
				Vector3 deltaPos = col.transform.position - transform.position;
				Vector3 avoidPosLeft = deltaPos + (right.position - transform.position) * -SIDE_DISTANCE;
				if (!adjusted)
					prevTarget ();
				adjusted = true;
				avoid (avoidPosLeft);
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
		if (!red && transform.GetComponentInChildren<VisionCone> ().isRed ())
			return;
		prepRotation (player.position);
		alerted = true;
		transform.GetComponentInChildren<VisionCone> ().coneColor (red ? 2 : 1);
		alertTime = 2.5f;
		GetComponentInChildren<TalkBubble> ().sayThing (NOTICE_MESSAGES [(int)(Random.value * 3)], 1f, true, "");
	}

	public void alertOff () {
		if (alerted) {
			prepRotation (targetPos);
			transform.GetComponentInChildren<VisionCone> ().coneColor (0);
			alerted = false;
		}
	}

}
