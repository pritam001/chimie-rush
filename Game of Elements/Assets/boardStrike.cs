using UnityEngine;
using System.Collections;

public class boardStrike : MonoBehaviour {
	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.name == "d6") {
			gameObject.GetComponentInParent<gameAnimation> ().PlaySound (0);
		}
	}

}
