using UnityEngine;
using System.Collections;

public class ChadAppears : MonoBehaviour {
	public GameObject ponz;
	public GameObject witch;

	// Use this for initialization
	void Start () {
		// spawn smoke
		GetComponent<MeshRenderer>().enabled = true;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
