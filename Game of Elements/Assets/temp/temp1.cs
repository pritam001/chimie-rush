using UnityEngine;
using System.Collections;

public class temp1 : MonoBehaviour {
	public GameObject t;
	// Use this for initialization
	void Start () {
		t.GetComponent<Rigidbody>().AddForce(new Vector3(10f,10f,10f));

	}
	
	// Update is called once per frame
	void Update () {
		print(t.GetComponent<Rigidbody>().velocity.sqrMagnitude);
	}
}
