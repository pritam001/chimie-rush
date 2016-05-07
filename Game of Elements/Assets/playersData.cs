using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playersData : MonoBehaviour {
	public string[] cardsName;
	public Dictionary<string,int> cards;
	public int chems;
	public int[] prices;
	// Use this for initialization
	void Start () {
		cards = new Dictionary<string, int> ();
		for (int i = 0; i < cardsName.Length; i++) {
			cards [cardsName [i]] = 1;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
