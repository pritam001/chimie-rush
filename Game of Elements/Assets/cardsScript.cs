using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class cardsScript : MonoBehaviour {

	public static int trigger;
	private static GameObject t,t1;
	private static string[] cards;
	public Material fade;
	public void buyCard(int i){
		gameObject.GetComponent<playerMovement> ().players [States.turn].GetComponent<playersData> ().cards [gameObject.GetComponent<playerMovement> ().players [States.turn].GetComponent<playersData> ().cardsName [i]] += 1;

	}

	public void useCard(int i){
		gameObject.GetComponent<playerMovement> ().players [States.turn].GetComponent<playersData> ().cards [gameObject.GetComponent<playerMovement> ().players [States.turn].GetComponent<playersData> ().cardsName [i]] -= 1;
		switch (i) {
		case 0:
			gameObject.GetComponent<cardsFunctions> ().useAcid ();
			break;
		case 1:
			gameObject.GetComponent<cardsFunctions> ().useBulb ();
			break;
		case 2:
			gameObject.GetComponent<cardsFunctions> ().useElectric ();
			break;
		case 6:
			gameObject.GetComponent<cardsFunctions> ().useMagnet ();
			break;
		case 7:
			gameObject.GetComponent<cardsFunctions> ().useStrength ();
			break;
		case 9:
			gameObject.GetComponent<cardsFunctions> ().useTrap ();
			break;
		}
		hideuseCards ();
	}
	public void hideuseCards(){
		t1.SetActive(false);
	}
	public void hidebuyCards(){
		t.SetActive(false);
	}
	public void useCards(){
		if (t1.activeSelf) {
			t1.SetActive(false);
			return;
		}
		hidebuyCards ();
		trigger = 1;
		t1.SetActive(true);
		Image[] childs = t1.GetComponentsInChildren<Image> ();
		foreach(Image s in childs){
			if (s.gameObject.name != "CardsPanelUse") {
				for(int i = 0; i < 10; i++){
					if (gameObject.GetComponent<playerMovement> ().players [States.turn].GetComponent<playersData> ().cardsName [i] == s.gameObject.name.Replace ("cards", "")) {
						if (gameObject.GetComponent<playerMovement> ().players [States.turn].GetComponent<playersData> ().cards[s.gameObject.name.Replace ("cards", "")]<1) {
							s.material = fade;
							s.gameObject.GetComponent<Button> ().enabled = false;
						} else {
							s.material = null;
							s.gameObject.GetComponent<Button> ().enabled = true;
						}
					}
				}
			}
		}
	}

	public void buyCards(){
		if (t.activeSelf == true) {
			t.SetActive(false);
			return;
		}
		hideuseCards ();
		trigger = 2;
		t.SetActive(true);
		Image[] childs = t.GetComponentsInChildren<Image> ();
		foreach(Image s in childs){
			if (s.gameObject.name != "CardsPanelBuy") {
				for(int i = 0; i < 10; i++){
					if (gameObject.GetComponent<playerMovement> ().players [States.turn].GetComponent<playersData> ().cardsName [i] == s.gameObject.name.Replace ("cards", "")) {
						if (gameObject.GetComponent<playerMovement> ().players [States.turn].GetComponent<playersData> ().prices [i] > gameObject.GetComponent<playerMovement> ().players [States.turn].GetComponent<playersData> ().chems) {
							s.material = fade;
							s.gameObject.GetComponent<Button> ().enabled = false;
						} else {
							s.material = null;
							s.gameObject.GetComponent<Button> ().enabled = true;
						}
					}
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
		trigger = 0;
		t = GameObject.Find ("CardsPanelBuy");
		t.SetActive(false);
		t1 = GameObject.Find ("CardsPanelUse");
		t1.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
