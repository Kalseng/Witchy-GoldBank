using UnityEngine;
using System.Collections;

public class Dialogue : MonoBehaviour {
	public string[] CHAT_PONZ_1_QUOTE;
	public Person[] CHAT_PONZ_1_SPEAKER;
	public string[] CHAT_PONZ_2_QUOTE;
	public Person[] CHAT_PONZ_2_SPEAKER;
	public string[] NPC_MESSAGES;

	private bool talking = false;
	private int quoteNumber = 0;
	private string[] currentDialogueQuotes;
	private Person[] currentDialogueSpeakers;

	private float debounce = 0f;
	private GameObject talkingTo = null;

	public void talkTo(Person who, GameObject body) {
		if (!talking && debounce <= 0f) {
			talkingTo = body;
			switch (who) {
			case Person.PONZ:
				int randomDialogue = Random.Range (0, 2);
				if (randomDialogue == 0)
					startTalk (CHAT_PONZ_1_QUOTE, CHAT_PONZ_1_SPEAKER);
				else
					startTalk (CHAT_PONZ_2_QUOTE, CHAT_PONZ_2_SPEAKER);
				break;
			case Person.CHAD:
				print ("talk to chad");
				break;
			case Person.STOCK:
				debounce = 0.2f;
				TalkBubble bubble = talkingTo.GetComponentInChildren<TalkBubble> ();
				if (bubble.followUp.Length != 0) // say second part of two-liner
					bubble.sayFollowUp ();
				else {
					int randomQuote = Random.Range (0, NPC_MESSAGES.Length);
					string message = NPC_MESSAGES [randomQuote];
					int divider = message.IndexOf ('&');
					if (divider != -1) // start two-liner
						bubble.sayThing (message.Substring (0, divider), 3, true, message.Substring (divider + 1));
					else // say one-liner
						bubble.sayThing (message, 3, true, "");
					talkingTo = null;
				}
				break;
			}
		}
	}

	void Update () {
		if (talking && Input.GetButtonDown ("Fire2"))
			nextQuote ();
		if (debounce >= 0f)
			debounce -= Time.deltaTime;
	}

	public void nextQuote() {
		quoteNumber++;
		if (quoteNumber >= currentDialogueQuotes.Length)
			endTalk ();
		else
			GameObject.Find ("InGameGUI").GetComponent<InGameGUI> ().chatBarQuote (
				currentDialogueQuotes [quoteNumber], currentDialogueSpeakers [quoteNumber]);
	}

	public void startTalk(string[] quotes, Person[] speakers) {
		quoteNumber = -1;
		currentDialogueQuotes = quotes;
		currentDialogueSpeakers = speakers;
		talking = true;
		//nextQuote ();
		if (talkingTo.GetComponent<Patrol2>())
			talkingTo.GetComponent<Patrol2> ().talkFreeze = true;
		if (talkingTo.GetComponent<Rigidbody2D> ())
			talkingTo.GetComponent<Rigidbody2D> ().isKinematic = true;
		if (talkingTo.GetComponentInChildren<TalkBubble> ())
			talkingTo.GetComponentInChildren<TalkBubble> ().timeLeft = 0;
		GetComponent<PlayerController> ().talkFreeze = true;
		GetComponent<Rigidbody2D> ().isKinematic = true;
	}

	public void endTalk() {
		talking = false;
		debounce = 0.2f;
		if (talkingTo.GetComponent<Patrol2>())
			talkingTo.GetComponent<Patrol2> ().talkFreeze = false;
		if (talkingTo.GetComponent<Rigidbody2D> ())
			talkingTo.GetComponent<Rigidbody2D> ().isKinematic = false;
		GetComponent<PlayerController> ().talkFreeze = false;
		GetComponent<Rigidbody2D> ().isKinematic = false;
		talkingTo = null;
		GameObject.Find ("InGameGUI").GetComponent<InGameGUI> ().hideChatBar ();
	}
}

public enum Person {WITCH, PONZ, CHAD, STOCK};