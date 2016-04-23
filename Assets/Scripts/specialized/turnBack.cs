using UnityEngine;
using System.Collections;

public class turnBack : MonoBehaviour {
	public GameObject ponz;
	public GameObject witch;
	private float elapsedTime = 0f;
	private bool begun = false;
	private Quaternion ponzInit = Quaternion.identity;
	private Quaternion witchInit = Quaternion.identity;
	private Quaternion ponzFinal = Quaternion.identity;
	private Quaternion witchFinal = Quaternion.identity;

	// Use this for initialization
	public void begin () {
		ponzInit = ponz.transform.rotation;
		witchInit = witch.transform.rotation;
		ponzFinal = Quaternion.LookRotation (Vector3.forward, ponz.transform.position - witch.transform.position);
		Vector3 witchTurn = ponz.transform.position - witch.transform.position;
		witchTurn = new Vector3 (-witchTurn.y, witchTurn.x, witchTurn.z);
		witchFinal = Quaternion.LookRotation (Vector3.forward, witchTurn);
		begun = true;
	}

	// Update is called once per frame
	void Update () {
		if (begun) {
			elapsedTime += Time.deltaTime;
			ponz.transform.rotation = Quaternion.Lerp (ponzInit, ponzFinal, elapsedTime);
			witch.transform.rotation = Quaternion.Lerp (witchInit, witchFinal, elapsedTime);
			if (elapsedTime > 2f) {
				witch.GetComponent<Dialogue> ().endSceneEvent ();
				Destroy (GetComponent<turnBack> ());
			}
		}
	}
}
