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
				int randomQuote = Random.Range (0, NPC_MESSAGES.Length);
				string[] onlyQuote = new string[1];
				onlyQuote [0] = NPC_MESSAGES [randomQuote];
				Person[] onlySpeaker = new Person[1];
				onlySpeaker [0] = Person.STOCK;
				startTalk (onlyQuote, onlySpeaker);
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
		nextQuote ();
		talkingTo.GetComponent<TalkIdentity> ().startTalk ();
	}

	public void endTalk() {
		talking = false;
		debounce = 0.2f;
		talkingTo.GetComponent<TalkIdentity> ().endTalk ();
		talkingTo = null;
		GameObject.Find ("InGameGUI").GetComponent<InGameGUI> ().hideChatBar ();
	}
}

public enum Person {WITCH, PONZ, CHAD, STOCK};