using UnityEngine;
using System.Collections;

public class Pentagram : MonoBehaviour {

	public ParticleSystem smoke;

	// Could be triggered by a bounds check when setting down an item
	public void addItem (GameObject item) {
		// print ("adding item");
		Item properties = item.GetComponent<Item>();
		for (int i = 0; i < properties.composition.Length; i++) {
			if (properties.composition [i] > 0)
				GameObject.Find ("InGameGUI").GetComponent<InGameGUI> ().addMaterial (i, properties.composition [i]);
		}
		GameObject.Find ("InGameGUI").GetComponent<InGameGUI> ().convertAll (); // temporary
		item.AddComponent<Disappear> ();
		GetComponent<AudioSource> ().Play ();
		smoke.Play ();
	}

}
