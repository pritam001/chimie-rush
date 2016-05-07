using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {

	public int[] positions;
	public GameObject[] players;
	public GameObject startingPosition;

	public bool hasPlayerReached(int turn){
		players[turn].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
		return (players[turn].GetComponent<Rigidbody>().velocity.sqrMagnitude == 0.0f);
	}
	public int returnPosition(int turn){
		return positions [turn];	
	}
	public void reset(){
		for (int i = 0; i < players.Length; i++) {
			players [i].transform.position = startingPosition.transform.position;
		}
		for (int i = 0; i < positions.Length; i++) {
			positions [i] = 0;
		}
	}

	public void movePlayerPosition(int turn, int position){
		players [turn].SendMessage ("SetTarget", GameObject.Find ("GameObject (" + position.ToString () + ")").transform);
		positions [turn] = position;
	}

	public void movePlayer(int turn,int value){
			int next = positions [turn] + value;
			if (next <= 119) {
				players [turn].SendMessage ("SetTarget", GameObject.Find ("GameObject (" + next.ToString () + ")").transform);
				positions [turn] = positions [turn] + value;
			}
	}

	// Use this for initialization
	void Start () {
		reset ();
		for (int i = 0; i < players.Length; i++) {
			players[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
