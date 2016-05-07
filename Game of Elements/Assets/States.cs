using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class States : MonoBehaviour {
	public Material[] Mat;
	public GameObject[] forQuestion; 
	public GameObject button;
	private int i = 0;
	public GameObject dice;
	public GameObject spawnPoint;
	public static int turn;
	public GameObject buyingPanel;
	public GameObject[] PanelPosition;
	public GameObject EndOfMoveQuestion;
	public static bool useStrength = false;
	public static bool hasAsked;
	public static bool answerofQuestion;
	public static bool waitForAnswer;
	public string[] elementsName = {"None","Hydrogen Green","Helium Purple","Lithium Blue","Beryllium Red","Boron Green","Carbon Green","Nitrogen Green","Oxygen Green","Fluorine Red","Neon Purple","Sodium Blue","Magnesium Green","Aluminium SkyBlue","Silicon Green","Phosphorus Green","Sulfur Green","Chlorine Orange","Argon Purple","Potassium Orange","Calcium Green","Scandium Pink","Titanium Pink","Vanadium Pink","Chromium Red","Manganese Blue","Iron Green","Cobalt Pink","Nickel Pink","Copper Yellow","Zinc Blue","Gallium SkyBlue","Germanium Green","Arsenic Red","Selenium Green","Bromine Red","Krypton Purple","Rubidium Blue","Strontium Blue","Yttrium Pink ","Zirconium Pink","Niobium Pink","Molybdenum Pink","Technetium Pink","Ruthenium Pink","Rhodium Pink","Palladium Orange","Silver Yellow","Cadmium Green","Indium SkyBlue","Tin SkyBlue","Antimony Grey","Tellurium Grey","Iodine Green","Xenon Purple","Caesium Red","Barium Blue","Lanthanum Yellow","Cerium Yellow","Praseodymium Yellow","Neodymium Yellow","Promethium Yellow","Samarium Yellow","Europium Yellow","Gadolinium Yellow","Terbium Yellow","Dysprosium Yellow","Holmium Yellow","Erbium Yellow","Thulium Yellow","Ytterbium Yellow","Lutetium Yellow","Hafnium Pink","Tantalum Pink","Tungsten Green","Rhenium Pink","Osmium Pink","Iridium Pink","Platinum Yellow","Gold Yellow","Mercury Red","Thallium SkyBlue","Lead Red","Bismuth SkyBlue","Polonium Grey ","Astatine Orange","Radon Purple","Francium Red","Radium Green","Actinium Pink","Thorium Pink","Protactinium Pink","Uranium Black","Neptunium Pink","Plutonium Pink","Americium Pink","Curium Pink","Berkelium Pink","Californium Pink","Einsteinium Pink","Fermium Pink","Mendelevium Pink","Nobelium Pink","Lawrencium Pink","Rutherfordium Pink","Dubnium Pink","Seaborgium Pink","Bohrium Pink","Hassium Pink","Meitnerium Pink","Darmstadtium Pink","Roentgenium Pink","Copernicium Pink","Ununtrium Green","Flerovium Green","Ununpentium Green","Livermorium Green","Ununseptium Orange","Ununoctium Black","End"};

	public void ChangePanelPosition(){
		buyingPanel.transform.position = PanelPosition [1-turn].transform.position;
	}
	public void ChangeColor(){
		dice.GetComponent<Renderer> ().enabled = true;
		dice.GetComponent<Renderer> ().sharedMaterial = Mat [1 - i];
		i = 1 - i;
	}
	public void ChangePosition(){
		dice.GetComponent<Rigidbody> ().useGravity = false;
		dice.transform.position = spawnPoint.transform.position;
		dice.transform.Rotate(new Vector3(Random.value * 360, Random.value * 360, Random.value * 360));
	}
	public void ApplyForce(){
		dice.GetComponent<Rigidbody> ().useGravity = true;
		dice.GetComponent<Rigidbody> ().AddForce (new Vector3 (1000f, 1000f,0f));
		dice.GetComponent<Rigidbody> ().AddTorque (new Vector3 (10000f, 10000f, 10000f));

	}
	public bool checkRolling(){
		return !(dice.GetComponent<Rigidbody> ().velocity.sqrMagnitude < 0.1f && dice.GetComponent<Rigidbody> ().angularVelocity.sqrMagnitude < 0.1f);
	}
	public void RolltheDice(){
		//to be deleted in mulitplayer.
		if (turn == 1) {
			gameObject.GetComponent<AI> ().computeBefore ();
		}
		gameObject.GetComponent<gameAnimation> ().DisplayMessage (-1);
		button.SetActive(false);
		StartCoroutine (waitTillend ());
	}
	public void hideShowGameObjects(bool a){
		foreach (GameObject t in forQuestion) {
			t.GetComponent<Renderer> ().enabled = a;
		}
	}
	IEnumerator waitTillend(){
		ChangeColor ();
		ChangePosition ();
		ApplyForce ();

		yield return new WaitForFixedUpdate();
		yield return new WaitForFixedUpdate();

		while (checkRolling ()) {
			yield return null;
		}

		Die die = dice.GetComponent<Die>();
		while (die.value == 0) {
			yield return null;
		}

		if (useStrength == true) {
			die.value = die.value * 2;
			useStrength = false;
		}

		gameObject.GetComponent<playerMovement> ().movePlayer (turn, die.value);
		int prev_pos = gameObject.GetComponent<playerMovement> ().positions [turn];
		if (prev_pos == 119) {
			gameObject.GetComponent<gameAnimation> ().PlayAnimationL (turn);
			if (turn == 0) {
				EndOfMoveQuestion.GetComponentInChildren<Text> ().text = "You Won ! Play Again ?";
				cardsFunctions.hackClicked = 3;
				EndOfMoveQuestion.SetActive (true);
				hasAsked = true;
			} else {
				EndOfMoveQuestion.GetComponentInChildren<Text> ().text = "You Lost ! Play Again ?";
				cardsFunctions.hackClicked = 3;
				EndOfMoveQuestion.SetActive (true);
				hasAsked = true;
			}
		} else {

			yield return new WaitForSeconds (2);
			yield return new WaitForFixedUpdate ();
			yield return new WaitForFixedUpdate ();

			while (!gameObject.GetComponent<playerMovement> ().hasPlayerReached (turn)) {
				yield return null;
			}

			int position = gameObject.GetComponent<playerMovement> ().positions [turn];
			Transform board = gameObject.transform.Find ("Board");

			while (BoardState.checkQuestion (position)) {
				hideShowGameObjects (false);
				board.gameObject.GetComponent<MeshRenderer> ().enabled = false;
				waitForAnswer = false;
				cardsFunctions.hackClicked = 2;
				hasAsked = true;
				string t = "Do you want to answer a Question ?";
				EndOfMoveQuestion.GetComponentInChildren<Text> ().text = t;
				answerofQuestion = true;
				EndOfMoveQuestion.SetActive (true);
				while (waitForAnswer == false) {
					yield return null;
				}

				if (answerofQuestion) {
					waitForAnswer = false;
					cardsFunctions.hackClicked = 2;
					hasAsked = true;
					string t1 = GenerateQuestion ();
					EndOfMoveQuestion.GetComponentInChildren<Text> ().text = t1;
					EndOfMoveQuestion.SetActive (true);
					while (waitForAnswer == false) {
						yield return null;	
					}
					if (turn != 1) {
						if (answerofQuestion == true) {
							gameObject.GetComponent<playerMovement> ().movePlayerPosition (turn, position + 2);
						} else {
							gameObject.GetComponent<playerMovement> ().movePlayerPosition (turn, position - 1);
						}
					} else {
						if (answerofQuestion == false) {
							gameObject.GetComponent<playerMovement> ().movePlayerPosition (turn, position + 2);
						} else {
							gameObject.GetComponent<playerMovement> ().movePlayerPosition (turn, position - 1);
						}
					}
					position = gameObject.GetComponent<playerMovement> ().positions [turn];
					hideShowGameObjects (true);
					board.gameObject.GetComponent<MeshRenderer> ().enabled = true;
				} else {
					EndOfMoveQuestion.SetActive (false);
					hasAsked = false;
					cardsFunctions.afterQuestion = false;
					break;
				}	
				EndOfMoveQuestion.SetActive (false);
				hasAsked = false;
				cardsFunctions.afterQuestion = false;
				yield return new WaitForSeconds (5);
			}

			hideShowGameObjects (true);
			board.gameObject.GetComponent<MeshRenderer> ().enabled = true;

			if (checkAfterMove ()) {
				yield return new WaitForFixedUpdate ();
				yield return new WaitForFixedUpdate ();
				while (!gameObject.GetComponent<playerMovement> ().hasPlayerReached (turn)) {
					yield return null;
				}
			}

			while (EndOfMoveQuestion.activeSelf) {
				yield return null;
			}

			if (hasAsked && cardsFunctions.afterQuestion == true) {
				hasAsked = false;
				cardsFunctions.afterQuestion = false;
				gameObject.GetComponent<playerMovement> ().movePlayerPosition (turn, prev_pos);
			}

			while (!gameObject.GetComponent<playerMovement> ().hasPlayerReached (turn)) {
				yield return null;
			}

			gameObject.GetComponent<gameAnimation> ().setImage ();
			turn = 1 - turn;
			if (BoardState.checkTrap (gameObject.GetComponent<playerMovement> ().positions [turn])) {
				turn = 1 - turn;
				yield return new WaitForSeconds (2);
				ChangePanelPosition ();
				gameObject.GetComponent<gameAnimation> ().setImage ();
				gameObject.GetComponent<gameAnimation> ().TurnChange ();
				gameObject.GetComponent<gameAnimation> ().GreetOrBored (false);
				button.SetActive (true);
			} else {
				yield return new WaitForSeconds (2);
				ChangePanelPosition ();
				gameObject.GetComponent<gameAnimation> ().setImage ();
				gameObject.GetComponent<gameAnimation> ().TurnChange ();
				gameObject.GetComponent<gameAnimation> ().GreetOrBored (false);
				button.SetActive (true);
			}

			//code to be deleted in multiplayer
			if (turn == 1) {
				button.SetActive (false);
				RolltheDice ();
			}
		}
	}


	public void RestartGame(bool answer){
		if (cardsFunctions.hackClicked != 3) {
			return;
		}
		if (answer == true) {
			SceneManager.LoadScene ("Board");
		}
		else{
			SceneManager.LoadScene ("FirstScene");
		}
	}



	public void canAskQuestion(){
		int position = gameObject.GetComponent<playerMovement> ().positions [turn];
		if (BoardState.checkForQuestion (position)) {
			//highlight message to look below.
			//ask Question Gui.
			canAskQuestion();
		} else
			return;
	}
	IEnumerator waitforlastQuestion(){
		while (EndOfMoveQuestion.activeSelf) {
			yield return null;
		}
	}
	public bool checkAfterMove(){
		int position = gameObject.GetComponent<playerMovement> ().positions [turn];
		playerMovement referencePlayer = gameObject.GetComponent<playerMovement> ();
		if (BoardState.checkAndDivert (position) != position) {
			gameObject.GetComponent<gameAnimation> ().DisplayMessage (3);
			gameObject.GetComponent<gameAnimation> ().PlayAnimation (true);
			if (turn != 1) {
				EndOfMoveQuestion.GetComponentInChildren<Text> ().text = "Do you want to use health card ?";
				cardsFunctions.hackClicked = 1;
				EndOfMoveQuestion.SetActive (true);
				hasAsked = true;
			} else {
				int goodpaths = 0;
				int playerPos = gameObject.GetComponent<playerMovement> ().positions [1 - turn];
				gameObject.GetComponent<AI> ().recursionFromUser (ref goodpaths, 4, 0, position, playerPos);
				int total = gameObject.GetComponent<AI> ().computeTotalPaths (4);
				float fail = total - goodpaths;
				fail = fail / total;
				if (fail > 0.5f) {
					int count = gameObject.GetComponent<playerMovement> ().players [1].GetComponent<playersData> ().cards ["Health"];
					int cardPos = gameObject.GetComponent<AI> ().getPositionOfCard ("Health");
					int amountHas = gameObject.GetComponent<playerMovement> ().players [1].GetComponent<playersData> ().chems;
					int amountReq = gameObject.GetComponent<playerMovement> ().players [1].GetComponent<playersData> ().prices [cardPos];
					if (count > 0) {
						cardsFunctions.hackClicked = 1;
						hasAsked = true;
						gameObject.GetComponent<cardsFunctions> ().useHealth (0);	
					} else if (amountHas > amountReq) {
						cardsFunctions.hackClicked = 1;
						hasAsked = true;
						gameObject.GetComponent<cardsScript> ().buyCard (cardPos);
						gameObject.GetComponent<cardsFunctions> ().useHealth (0);
					}
				}
			}
			referencePlayer.movePlayerPosition (turn, BoardState.checkAndDivert (position));
			return true;
		}

		if (BoardState.checkAndLoseCard (position) != null) {
			gameObject.GetComponent<gameAnimation> ().DisplayMessage (4);
			gameObject.GetComponent<gameAnimation> ().PlayAnimation (true);
			referencePlayer.players [turn].GetComponent<playersData> ().cards [BoardState.checkAndLoseCard (position)] -= 1;
			return true;
		}

		if (BoardState.checkAndReturnCard (position) != null) {
			gameObject.GetComponent<gameAnimation> ().DisplayMessage (5);
			gameObject.GetComponent<gameAnimation> ().PlayAnimation (false);
			referencePlayer.players [turn].GetComponent<playersData> ().cards [BoardState.checkAndReturnCard (position)] += 1;
			return true;
		}

		if (BoardState.checkBlast (position)) {
			gameObject.GetComponent<gameAnimation> ().DisplayMessage (6);
			gameObject.GetComponent<gameAnimation> ().PlaySound (1);
			gameObject.GetComponent<gameAnimation> ().PlayExplosion ();
			gameObject.GetComponent<gameAnimation> ().PlayAnimation (true);
			BoardState.intializeBoard ();
			referencePlayer.reset ();
			if (!(gameObject.GetComponent<playerMovement> ().players [0].GetComponent<playersData> ().cards ["Survival"] > 0 || gameObject.GetComponent<playerMovement> ().players [0].GetComponent<playersData> ().cards ["Hydro"] > 0)) {
				referencePlayer.movePlayerPosition (0, 0);
				if (gameObject.GetComponent<playerMovement> ().players [0].GetComponent<playersData> ().cards ["Survival"] > 0) {
					gameObject.GetComponent<playerMovement> ().players [0].GetComponent<playersData> ().cards ["Survival"] -= 1;
				} else {
					gameObject.GetComponent<playerMovement> ().players [0].GetComponent<playersData> ().cards ["Hydro"] -= 1;
				}
			}
			if (!(gameObject.GetComponent<playerMovement> ().players [1].GetComponent<playersData> ().cards ["Survival"] > 0 || gameObject.GetComponent<playerMovement> ().players [1].GetComponent<playersData> ().cards ["Hydro"] > 0)) {
				referencePlayer.movePlayerPosition (1, 0);
				if (gameObject.GetComponent<playerMovement> ().players [1].GetComponent<playersData> ().cards ["Survival"] > 0) {
					gameObject.GetComponent<playerMovement> ().players [1].GetComponent<playersData> ().cards ["Survival"] -= 1;
				} else {
					gameObject.GetComponent<playerMovement> ().players [1].GetComponent<playersData> ().cards ["Hydro"] -= 1;
				}
			}
			return true;
		}


		int lift = BoardState.checkAndUseLift (position);
		if (lift != position) {
			if (lift > position) {
				gameObject.GetComponent<gameAnimation> ().PlayAnimation (false);
				gameObject.GetComponent<gameAnimation> ().DisplayMessage (2);
				gameObject.GetComponent<gameAnimation> ().setLifts ();
			} else {
				gameObject.GetComponent<gameAnimation> ().PlayAnimation (true);
				gameObject.GetComponent<gameAnimation> ().DisplayMessage (8);
				gameObject.GetComponent<gameAnimation> ().setLifts ();
				if (turn != 1) {
					hasAsked = true;
					EndOfMoveQuestion.GetComponentInChildren<Text> ().text = "Do you want to use Hack Card ?";
					EndOfMoveQuestion.SetActive (true);
					cardsFunctions.hackClicked = 0;
				} else {
					int goodpaths = 0;
					int playerPos = gameObject.GetComponent<playerMovement> ().positions [1 - turn];
					gameObject.GetComponent<AI> ().recursionFromUser (ref goodpaths, 4, 0, position, playerPos);
					int total = gameObject.GetComponent<AI> ().computeTotalPaths (4);
					float fail = total - goodpaths;
					fail = fail / total;
					if (fail > 0.5f) {
						int count = gameObject.GetComponent<playerMovement> ().players [1].GetComponent<playersData> ().cards ["Hack"];
						int cardPos = gameObject.GetComponent<AI> ().getPositionOfCard ("Hack");
						int amountHas = gameObject.GetComponent<playerMovement> ().players [1].GetComponent<playersData> ().chems;
						int amountReq = gameObject.GetComponent<playerMovement> ().players [1].GetComponent<playersData> ().prices [cardPos];
						if (count > 0) {
							cardsFunctions.hackClicked = 1;
							hasAsked = true;
							gameObject.GetComponent<cardsFunctions> ().useHack (0);
						} else if (amountHas > amountReq) {
							cardsFunctions.hackClicked = 1;
							hasAsked = true;
							gameObject.GetComponent<cardsScript> ().buyCard (cardPos);
							gameObject.GetComponent<cardsFunctions> ().useHack (0);
						}
					}
				}
			}
			referencePlayer.movePlayerPosition (turn, lift);
			return true;
		}

		return false;
	}

	public void AnswerOfQuestion(bool a){
		if (cardsFunctions.hackClicked != 2) {
			return;
		}
		print ("hello");
		waitForAnswer = true;
		answerofQuestion = !(answerofQuestion ^ a);
	}

	string returnRandomColorName(){
		int t = Random.Range (0, 10);
		switch (t) {
		case 0:
			return "Red";
		case 1: 
			return "Green";
		case 2: 
			return "Blue";
		case 3:
			return "Orange";
		case 4:
			return "Yellow";
		case 5:
			return "SkyBlue";
		case 6:
			return "Black";
		case 7:
			return "Pink";
		case 8:
			return "Purple";
		default: 
			return "Red";
		}
	}

	string GenerateQuestion(){
		int selectElement = Random.Range (1, 118);
		string Color = returnRandomColorName ();
		answerofQuestion = false;
		if (elementsName [selectElement].Split (' ') [1] == Color) {
			answerofQuestion = true;
		}
		return "is "+elementsName[selectElement].Split(' ')[0]+" "+Color+" ?";
	}

	// Use this for initialization
	void Start () {
		BoardState.intializeBoard ();
		if (Random.Range (0.0f, 1.0f) > 0.5f) {
			turn = 0;
		} else {
			turn = 1;
		}
		hasAsked = false;
		EndOfMoveQuestion.SetActive(false);
		ChangePanelPosition ();
		if (turn == 1) {
			RolltheDice ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
