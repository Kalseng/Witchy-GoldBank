﻿using UnityEngine;
using System.Collections;

public class Dialogue : MonoBehaviour {
	public string[] CHAT_PONZ_1_QUOTE;
	public Person[] CHAT_PONZ_1_SPEAKER;
	public string[] CHAT_PONZ_2_QUOTE;
	public Person[] CHAT_PONZ_2_SPEAKER;
	public string[] INTRO_TEXT_QUOTE;
	public Person[] INTRO_TEXT_SPEAKER;
	public string[] CHAT_CHAD_1_QUOTE;
	public Person[] CHAT_CHAD_1_SPEAKER;
	public string[] CHAT_CHAD_2_QUOTE;
	public Person[] CHAT_CHAD_2_SPEAKER;
	public string[] WIN_TEXT_QUOTE;
	public Person[] WIN_TEXT_SPEAKER;
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
			int randomDialogue = Random.Range (0, 2);
			switch (who) {
			case Person.PONZ:
				if (randomDialogue == 0)
					startTalk (CHAT_PONZ_1_QUOTE, CHAT_PONZ_1_SPEAKER);
				else
					startTalk (CHAT_PONZ_2_QUOTE, CHAT_PONZ_2_SPEAKER);
				break;
			case Person.CHAD:
				if (randomDialogue == 0)
					startTalk (CHAT_CHAD_1_QUOTE, CHAT_CHAD_1_SPEAKER);
				else
					startTalk (CHAT_CHAD_2_QUOTE, CHAT_CHAD_2_SPEAKER);
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
		if (debounce >= 0f)
			debounce -= Time.deltaTime;
		else if (talking && Input.GetButtonDown ("Fire2"))
			nextQuote ();
		else if (currentDialogueQuotes == INTRO_TEXT_QUOTE && Input.GetKeyDown ("p")) {
			quoteNumber = 37;
			GameObject.Find ("chad").GetComponent<SpriteRenderer> ().enabled = true;
			GameObject.Find ("ponz").transform.position = GameObject.Find ("newPos").transform.position;
			GameObject.Find ("ponz").transform.rotation = GameObject.Find ("newPos").transform.rotation;
			/*if (GameObject.Find ("ponz").GetComponent<ponzMove> () != null)
				Destroy (GameObject.Find ("ponz").GetComponent<ponzMove> ());*/
			if (GameObject.Find ("chad").GetComponent<ChadAppears> () != null)
				Destroy (GameObject.Find ("chad").GetComponent<ChadAppears> ());
			if (GetComponent<turnBack> () != null)
				Destroy (GetComponent<turnBack> ());
			nextQuote ();
		}
	}

	public void nextQuote() {
		quoteNumber++;
		if (quoteNumber >= currentDialogueQuotes.Length)
			endTalk ();
		else if (currentDialogueSpeakers [quoteNumber] == Person.EVENT) {
			talking = false;
			GameObject.Find ("InGameGUI").GetComponent<InGameGUI> ().hideChatBar (false);
			sceneEvent (currentDialogueQuotes [quoteNumber]);
		} else
			GameObject.Find ("InGameGUI").GetComponent<InGameGUI> ().chatBarQuote (
				currentDialogueQuotes [quoteNumber], currentDialogueSpeakers [quoteNumber]);
	}

	public void startTalk(string[] quotes, Person[] speakers) {
		quoteNumber = -1;
		currentDialogueQuotes = quotes;
		currentDialogueSpeakers = speakers;
		talking = true;
		debounce = 0.01f;
		nextQuote ();
		if (talkingTo != null) {
			if (talkingTo.GetComponent<Patrol2> ())
				talkingTo.GetComponent<Patrol2> ().talkFreeze = true;
			if (talkingTo.GetComponent<Rigidbody2D> ())
				talkingTo.GetComponent<Rigidbody2D> ().isKinematic = true;
			if (talkingTo.GetComponentInChildren<TalkBubble> ())
				talkingTo.GetComponentInChildren<TalkBubble> ().timeLeft = 0;
		}
		GetComponent<PlayerController> ().talkFreeze = true;
		GetComponent<Rigidbody2D> ().isKinematic = true;
	}

	public void endTalk() {
		talking = false;
		debounce = 0.2f;
		if (talkingTo != null) {
			if (talkingTo.GetComponent<Patrol2> ())
				talkingTo.GetComponent<Patrol2> ().talkFreeze = false;
			if (talkingTo.GetComponent<Rigidbody2D> ())
				talkingTo.GetComponent<Rigidbody2D> ().isKinematic = false;
		}
		GetComponent<PlayerController> ().talkFreeze = false;
		GetComponent<Rigidbody2D> ().isKinematic = false;
		talkingTo = null;
		GameObject.Find ("InGameGUI").GetComponent<InGameGUI> ().hideChatBar ();
	}

	void sceneEvent(string thingHappens) {
		switch (thingHappens) {
		case "chad_appears":
			GameObject.Find ("chad").GetComponent<ChadAppears> ().begin ();
			break;
		case "turn_ponz":
			GetComponent<turnBack> ().begin ();
			break;
		case "none":
			print ("do nothing!");
			talking = true;
			nextQuote ();
			break;
		case "start_game":
			GameObject.Find ("ponz").GetComponent<ponzMove> ().begin ();
			endTalk ();
			GameObject.Find ("GameManager").GetComponent<GameManager> ().StartRound (1000, 1450);
			break;
		case "win_screen":
			endTalk ();
			GameObject.Find ("GameManager").GetComponent<GameManager> ().ToWinScreen ();
			break;
		}
	}

	public void endSceneEvent() {
		talking = true;
		nextQuote ();
	}
}

public enum Person {WITCH, PONZ, CHAD, STOCK, EVENT};