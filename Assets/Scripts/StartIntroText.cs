using UnityEngine;
using System.Collections;

public class StartIntroText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find ("InGameGUI").GetComponent<InGameGUI> ().initNames ();
		GameObject p = GameObject.FindWithTag ("Player");
		p.GetComponent<PlayerController> ().talkFreeze = true;
		p.GetComponent<Rigidbody2D> ().isKinematic = true;
		Dialogue d = p.GetComponent<Dialogue> ();
		d.startTalk (d.INTRO_TEXT_QUOTE, d.INTRO_TEXT_SPEAKER);
		//d.startTalk(d.CHAT_PONZ_1_QUOTE, d.CHAT_PONZ_1_SPEAKER); //goes quicker for testing, but doesn't initialize gameplay properly
	}

}
