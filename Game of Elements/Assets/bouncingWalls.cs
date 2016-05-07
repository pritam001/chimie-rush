using UnityEngine;
using System.Collections;

public class bouncingWalls : MonoBehaviour {
	
	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.name == "d6") {
			Vector3 force = -1*col.gameObject.GetComponent<Rigidbody> ().velocity.normalized * 1000.0f;
			col.gameObject.GetComponent<Rigidbody> ().AddForce (force, ForceMode.Acceleration);
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
