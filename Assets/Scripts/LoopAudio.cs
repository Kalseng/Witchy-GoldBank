using UnityEngine;
using System.Collections;

public class LoopAudio : MonoBehaviour {
	static double START_TIME = 0.5f;
	public AudioSource intro;
	public AudioSource loop;

	// Use this for initialization
	void Start () {
		intro.PlayScheduled(AudioSettings.dspTime + START_TIME);
		loop.PlayScheduled(AudioSettings.dspTime + intro.clip.length + START_TIME);
	}
	
}
