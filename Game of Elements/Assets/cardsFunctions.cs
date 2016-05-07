using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class cardsFunctions : MonoBehaviour {
	public GameObject[] lifts;
	public static HashSet<int> prevLift;
	public GameObject QuestionBox;
	public static bool afterQuestion;
	public GameObject elementPosition;
	public GameObject TrapButton;
	public GameObject Button;
	public static int hackClicked;
	GameObject[] t;
	public void useAcid(){
		int pos = gameObject.GetComponent<playerMovement> ().positions [1 - States.turn];
		switch (pos) {
		case 3:
		case 11:
		case 19:
		case 37:
		case 12:
		case 20:
		case 38:
		case 56:
			gameObject.GetComponent<playerMovement> ().movePlayerPosition (1 - States.turn, 1);
			break;
		default:
			break;
		}
	}
	public void useBulb(){
		foreach (GameObject s in lifts) {
			s.SetActive(!s.activeSelf);
		}
	}
	public void useElectric(){
		if (prevLift.Count == 0) {
			for (int i = 1; i < BoardState.board.Length; i++) {
				if (BoardState.board [i] == "L") {
					BoardState.board [i] = "N";
					prevLift.Add (i);
				}
			}
		} else {
			foreach (int i in prevLift) {
				BoardState.board [i] = "L";
			}
		}
	}
	public void useHack(int a){
		if (hackClicked != 0) {
			return;
		}
		print ("hack clicked");
		if (a == 1) {
			afterQuestion = false;
			QuestionBox.SetActive(false);
			return;
		}
		if (gameObject.GetComponent<playerMovement> ().players [States.turn].GetComponent<playersData> ().cards ["Hack"] <= 0) {
			afterQuestion = false;
			QuestionBox.SetActive(false);
			return;
		}
		gameObject.GetComponent<playerMovement> ().players [States.turn].GetComponent<playersData> ().cards ["Hack"] -= 1;
		afterQuestion = true;
		QuestionBox.SetActive(false);
		return;
	}
	public void useHealth(int a){
		if (hackClicked != 1) {
			return;
		}
		print ("health clicked");
		if (a == 1) {
			afterQuestion = false;
			QuestionBox.SetActive(false);
			return;
		}
		if (gameObject.GetComponent<playerMovement> ().players [States.turn].GetComponent<playersData> ().cards ["Health"] <= 0) {
			afterQuestion = false;
			QuestionBox.SetActive(false);
			return;
		}
		gameObject.GetComponent<playerMovement> ().players [States.turn].GetComponent<playersData> ().cards ["Health"] -= 1;
		afterQuestion = true;
		QuestionBox.SetActive(false);

		return;
	} 	

	public void useMagnet(){
		for (int i = 0; i < 1; i++) {
			if (Random.Range (0, 1) < 0.33f) {
				gameObject.GetComponent<playerMovement> ().movePlayerPosition (States.turn, 26+i);
				break;
			} 
		}
		gameObject.GetComponent<playerMovement> ().movePlayerPosition (States.turn, 26+2);
	}

	public void useStrength(){
		States.useStrength = true;
	}

	public void OnClick(int a){
		int i = 0;
		while (i < elementPosition.transform.childCount) {
			Destroy (t [i]);
			i = i + 1;
		}
		gameObject.GetComponent<gameAnimation> ().DisplayCustomMessage ("Trap has been placed");
		BoardState.setTrap (a);
		Button.SetActive(true);
	}

	public void OnClickError(){
		gameObject.GetComponent<gameAnimation> ().DisplayCustomMessage ("Trap can't be placed here");
	}

	public void useTrap(){
		int i=0;
		t= new GameObject[200];
		while (i < elementPosition.transform.childCount) {
			if (BoardState.board [i] == "N") {
				t [i] = Instantiate (TrapButton) as GameObject;
				t [i].SetActive(true);
				t [i].transform.parent = TrapButton.transform.parent;
				t [i].GetComponent<RectTransform> ().anchorMax = Camera.main.WorldToViewportPoint (elementPosition.transform.GetChild (i).transform.position);
				t [i].GetComponent<RectTransform> ().anchorMin = Camera.main.WorldToViewportPoint (elementPosition.transform.GetChild (i).transform.position);
				t [i].GetComponent<RectTransform> ().rotation = TrapButton.GetComponent<RectTransform> ().rotation;
				t [i].GetComponent<RectTransform> ().localScale = TrapButton.GetComponent<RectTransform> ().localScale;
				t [i].GetComponent<RectTransform> ().localPosition = new Vector3 (0f, 0f, 0f);
				t [i].GetComponent<RectTransform> ().offsetMax = new Vector2 (0f, 0f);
				t [i].GetComponent<RectTransform> ().offsetMin = new Vector2 (0f, 0f);
				t [i].GetComponent<RectTransform> ().sizeDelta = TrapButton.GetComponent<RectTransform> ().sizeDelta;
				int no = i;
				t [i].GetComponent<Button> ().onClick.AddListener (() => OnClick (no));
				i = i + 1;
			} else {
				t [i] = Instantiate (TrapButton) as GameObject;
				t [i].SetActive(true);
				t [i].transform.parent = TrapButton.transform.parent;
				t [i].GetComponent<RectTransform> ().anchorMax = Camera.main.WorldToViewportPoint (elementPosition.transform.GetChild (i).transform.position);
				t [i].GetComponent<RectTransform> ().anchorMin = Camera.main.WorldToViewportPoint (elementPosition.transform.GetChild (i).transform.position);
				t [i].GetComponent<RectTransform> ().rotation = TrapButton.GetComponent<RectTransform> ().rotation;
				t [i].GetComponent<RectTransform> ().localScale = TrapButton.GetComponent<RectTransform> ().localScale;
				t [i].GetComponent<RectTransform> ().localPosition = new Vector3 (0f, 0f, 0f);
				t [i].GetComponent<RectTransform> ().offsetMax = new Vector2 (0f, 0f);
				t [i].GetComponent<RectTransform> ().offsetMin = new Vector2 (0f, 0f);
				t [i].GetComponent<RectTransform> ().sizeDelta = TrapButton.GetComponent<RectTransform> ().sizeDelta;
				int no = i;
				t [i].GetComponent<Button> ().onClick.AddListener (() => OnClick (no));
				i = i + 1;
			}
		}
		Button.SetActive(false);
		gameObject.GetComponent<gameAnimation> ().DisplayMessage (10);

	}

	IEnumerator clickButton(){
		yield return null;
	}

	// Use this for initialization
	void Start () {
		prevLift = new HashSet<int> ();
		afterQuestion = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
