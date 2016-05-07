using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class sequence : MonoBehaviour {
	public GameObject[] Array;
	public GameObject MessageComponent;
	public GameObject[] PossiblePosition;
	private string[] Messages;
	public int[] InstructionDisplay;
	public Sprite[] sprites;
	public Sprite[] useImages;
	public GameObject imageBox;
	private int maxInstruction = 20;
	int j=0;
	int presentInstruction = 0;
	public void checkForImage(int instruction){
		if (useImages [instruction]) {
			imageBox.SetActive(true);
			imageBox.GetComponent<Image> ().sprite = useImages [instruction];
		} else {
			imageBox.SetActive(false);
		}
	}
	public void nextInstruction(){
		
		if (presentInstruction < maxInstruction) {
			presentInstruction++;
		} else {
			SceneManager.LoadScene ("FirstScene");
		}
		if (presentInstruction == 2) {
			MessageComponent.GetComponent<Text> ().color = Color.black;
			MessageComponent.transform.position = PossiblePosition [0].transform.position;
		} else {
			MessageComponent.GetComponent<Text> ().color = Color.white;
			MessageComponent.transform.position = PossiblePosition [2].transform.position;
		}
		MessageComponent.GetComponent<Text> ().text = Messages [presentInstruction];
		for (int i = 0; i < 3; i++) {
			Array [i].SetActive(true);
			if (i == InstructionDisplay [presentInstruction]) {
				Array [i].GetComponent<Image> ().sprite = sprites [0];
				Array [i].GetComponent<Image> ().color = Color.black;
			} else {
				Array [i].GetComponent<Image> ().sprite = sprites [1];
				Array [i].GetComponent<Image> ().color = Color.magenta;
			}
		}
		checkForImage (presentInstruction);
	}
	public void previousInstruction(){
		if (presentInstruction > 0) {
			presentInstruction--;
		}
		if (presentInstruction >= 2) {
			MessageComponent.GetComponent<Text> ().color = Color.black;
			MessageComponent.transform.position = PossiblePosition [0].transform.position;
		} else {
			MessageComponent.GetComponent<Text> ().color = Color.white;
			MessageComponent.transform.position = PossiblePosition [2].transform.position;
		}
		MessageComponent.GetComponent<Text> ().text = Messages [presentInstruction];
		for (int i = 0; i < 3; i++) {
			Array [i].SetActive(true);
			if (i == InstructionDisplay [presentInstruction]) {
				Array [i].GetComponent<Image> ().sprite = sprites [0];
				Array [i].GetComponent<Image> ().color = Color.black;
			} else {
				Array [i].GetComponent<Image> ().sprite = sprites [1];
				Array [i].GetComponent<Image> ().color = Color.magenta;
			}
		}
		checkForImage (presentInstruction);
	}
	// Use this for initialization
	void Start () {
		imageBox.SetActive(false);
		InstructionDisplay = new int[100];
		InstructionDisplay [0] = 0;
		InstructionDisplay [1] = 1;
		InstructionDisplay [2] = 2;
		InstructionDisplay [3] = 2;
		for (int i = 4; i < InstructionDisplay.Length; i++) {
			InstructionDisplay [i] = 4;
		}
		Messages = new string[100];
		MessageComponent.transform.position = PossiblePosition [2].transform.position;
		Messages [0] = "This is Your Avtar throughout the game, Her name is Alex, She will be giving you instruction in game.";
		Messages [1] = "This is Avtar of Computer, Her name is Jennie, She is the one you have to defeat in order to win the game.";
		Messages [2] = "So If you don't know, this is the ground where the battle begins.";
		Messages [3] = "You want to win, Finish first.";
		Messages [4] = "Each element has a color, showing the power it has.";
		Messages [5] = "Oxygen is Green, land on it and get health card.";
		Messages [6] = "Flourine is Red, you don't have a health card, go to last Safe House.";
		Messages [7] = "These are lifts they can be useful and dangerous as well.";
		Messages [8] = "If lift is on ground floor and you visit the element, pay the reward.";
		Messages [9] = "If lift is on top floor and you visit the element, you are just lucky.";
		Messages [10] = "Copper is Gold, land on it and become Richie Rich.";
		Messages [11] = "What is +1 at top right corner of Lithium ? you have power to donate 1 Electron to your opponent.";
		Messages [12] = "What is -2 at top right corner of Chlorine? you have power to collect 2 Electron from your oppnonent.";
		Messages [13] = "If Opponent can collect/donate you the same number of Electron, Control your player movement for one chance and his chance would be skipped.";
		Messages [14] = "Can't pronounce Rutherfordium, No worries, game doesn't incoroporate pronounciation.";
		Messages [15] = "Land on Rutherfordium and get to answer a question, if you choose not to answer, you are just a coward";
		Messages [16] = "Answer Correctly and move 2 step forward.";
		Messages [17] = "Answer Incorrect and go 1 step back";
		Messages [18] = "Don't ever land on Uranium, of course if you are not a suicide bomber. Game just restarts from beginning.";
		Messages [19] = "Now it is the time to Roll the Dice and begin, You are ready soldier. To Roll the Dice hit the Left Mouse Button.";
		MessageComponent.GetComponent<Text> ().text = Messages [0];
	}
	
	// Update is called once per frame
	void Update () {
		//print (presentInstruction);
		if (j % 75 == 0 && presentInstruction <= 2) {
			Array [InstructionDisplay [presentInstruction]].SetActive(!Array [InstructionDisplay [presentInstruction]].activeSelf);
		}
		j = j + 1;
	}
}
