using UnityEngine;
using System.Collections;

// Unity polygon colliders get messed up whenever they are in the same object as literally
// any script with public sprite variables, so such variables are defined here instead
public class SpriteStore : MonoBehaviour {
	public Sprite[] sprites;
}
