using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {
	public string[] board;
	public int[] chems;
	public Dictionary<int,int> reference;
	public Dictionary<int,int> TrapInfo;
	public int Strength = 1;
	public static int TrapPosition;

	public void backUpEverything(){
		board = new string[BoardState.board.Length];
		System.Array.Copy (BoardState.board, board, BoardState.board.Length);
		chems = new int[BoardState.chems.Length];
		System.Array.Copy ( BoardState.chems, chems, BoardState.chems.Length);
		reference = new Dictionary<int,int >(BoardState.reference);
		TrapInfo = new Dictionary<int,int >(BoardState.TrapInfo);
	}

	public void reLoadEverything(){
		BoardState.board = board;
		BoardState.chems = chems;
		BoardState.reference = reference;
		BoardState.TrapInfo = TrapInfo;
	}

	public void recursionFromUser(ref int goodPaths,int CutOffLevel, int CurrentLevel, int CPU, int Player){
		if (BoardState.checkTrap(Player)) {
			recursionFromCPU (ref goodPaths, CutOffLevel, CurrentLevel, CPU, Player);
		}

		if (CurrentLevel == CutOffLevel) {
			if (CPU >= Player)
				goodPaths++;
		} else {
			for (int i = 1; i <= 6; i++) {
				int value = Player + i;
				if (value >= 119) {
					return;
				}
				if (BoardState.checkBlast (value)) {
					value = 1;
				}
				value = BoardState.checkAndUseLift (value);
				value = BoardState.checkAndDivert (value);
				recursionFromCPU (ref goodPaths, CutOffLevel, CurrentLevel + 1, CPU, value);
			}
		}
	}

	public void recursionFromCPU(ref int goodPaths,int CutOffLevel, int CurrentLevel, int CPU, int Player){
		if (BoardState.checkTrap(CPU)) {
			recursionFromUser (ref goodPaths, CutOffLevel, CurrentLevel, CPU, Player);
		}

		if (CurrentLevel == CutOffLevel) {
			if (CPU >= Player) {
				goodPaths++;
			}
		} else {
			for (int i = 1; i <= 6; i++) {
				int value = CPU + i;
				if (value == 119) {
					goodPaths++;
					return;
				}
				if (BoardState.checkBlast (value)) {
					value = 1;
				}
				value = BoardState.checkAndUseLift (value);
				value = BoardState.checkAndDivert (value);
				recursionFromUser (ref goodPaths, CutOffLevel, CurrentLevel + 1, value,Player);			
			}
		}
	}

	public int computeTotalPaths(int a){
		int val = 1;
		for (int i = 0; i < a; i++) {
			val *= 6;
		}
		return val;
	}

	public float tryAcid(int CPU,int Player){
		int[] Position = new int[]{ 3, 11, 19, 37, 12, 20, 38, 56 };
		bool flag = false;
		foreach (int p in Position) {
			if (p == Player) {
				flag = true;
				break;
			}
		}
		if (flag == true) {
			int goodPaths = 0;
			int cutOffLevel = 4;
			recursionFromCPU (ref goodPaths, cutOffLevel, 0, CPU,1);
			int totalPaths = computeTotalPaths (cutOffLevel);
			return (float)(totalPaths-goodPaths)/(float)(totalPaths);
		} else {
			return float.PositiveInfinity;
		}
	}
	public float tryElectric(int CPU,int Player){
		gameObject.GetComponent<cardsFunctions> ().useElectric();
		int goodPaths = 0;
		int curOffLevel = 4;
		recursionFromCPU (ref goodPaths, curOffLevel, 0, CPU, Player);
		int totalPaths = computeTotalPaths (curOffLevel);
		gameObject.GetComponent<cardsFunctions> ().useElectric ();		
		return (float)(totalPaths-goodPaths)/(float)(totalPaths);
	}

	public float tryMagnet(int CPU,int Player){
		CPU = 26;
		int goodPaths = 0;
		int curOffLevel = 4;
		recursionFromCPU (ref goodPaths, curOffLevel, 0, CPU, Player);
		int totalPaths = computeTotalPaths (curOffLevel);
		return (float)(totalPaths-goodPaths)/(float)(totalPaths);
	}

	public float tryStrength(int CPU,int Player){
		int goodPaths = 0;
		for (int i = 1; i <= 6; i++) {
			int value = CPU + (i*2);
			if (value == 119) {
				goodPaths++;
				break;
			}
			if (BoardState.checkBlast (value)) {
				value = 1;
			}
			value = BoardState.checkAndUseLift (value);
			value = BoardState.checkAndDivert (value);
			recursionFromUser (ref goodPaths, 3, 1, value,Player);			
		}
		int totalPaths = computeTotalPaths (4);
		return (float)(totalPaths-goodPaths)/(float)(totalPaths);
	}

	public float tryTrap(int CPU,int Player){
		int cutofflevel = 4;
		float globalFail = float.PositiveInfinity;
		int pos = Player;
		int totalpaths = computeTotalPaths (cutofflevel);
		for (int i = Player + 1; i <= Player+12; i++) {
			if (BoardState.setTrap (i)) {
				int goodPaths = 0;
				recursionFromCPU (ref goodPaths, cutofflevel, 0, CPU, Player);
				float fail = totalpaths - goodPaths;
				fail /= totalpaths;
				if (fail < globalFail) {
					globalFail = fail;
					pos = i;
				}
				BoardState.removeTrap (i);
			}
		}
		TrapPosition = pos;
		return globalFail;
	}

	public int getPositionOfCard(string s){
		int i = 0;
		foreach(string s1 in gameObject.GetComponent<playerMovement>().players[1].GetComponent<playersData>().cardsName){
			if (s == s1) {
				return i;
			}
			i = i + 1;
		}
		return i;
	}

	public string whatShouldITry(int CPU, int Player){
		float globalFail = float.PositiveInfinity;
		string globalCard = "";
		string[] funcs = new string[]{ "tryAcid","tryElectric","tryMagnet","tryStrength","tryTrap"};
		foreach( string s in funcs){
			int cardPos = getPositionOfCard (s.Substring(3));
			int amountHas = gameObject.GetComponent<playerMovement> ().players [1].GetComponent<playersData> ().chems;
			int amountReq = gameObject.GetComponent<playerMovement> ().players [1].GetComponent<playersData> ().prices [cardPos];
			if (amountHas >= amountReq || gameObject.GetComponent<playerMovement> ().players [1].GetComponent<playersData> ().cards [s.Substring (3)] > 0) {
				System.Reflection.MethodInfo t1 = GetType ().GetMethod (s);
				object[] t = new object[2]{ CPU, Player };
				object fail2 = (t1.Invoke (this, t));
				float fail = System.Convert.ToSingle(fail2);
				if (fail < globalFail) {
					globalFail = fail;
					globalCard = s.Substring(3);
				}
			}
		}
		return globalCard;
	}

	public void useit(string card){
		if (card == "Trap") {
			gameObject.GetComponent<gameAnimation> ().DisplayCustomMessage ("Trap has been placed");
			BoardState.setTrap (TrapPosition);
		} else {
			int cardPos = getPositionOfCard (card);
			int i = cardPos;
			gameObject.GetComponent<gameAnimation> ().DisplayCustomMessage ("I will use " + card);
			if (gameObject.GetComponent<playerMovement> ().players [1].GetComponent<playersData> ().cards [card] > 0) {
				gameObject.GetComponent<cardsScript> ().useCard (i);
			} else {
				gameObject.GetComponent<cardsScript> ().buyCard (i);
				gameObject.GetComponent<cardsScript> ().useCard (i);
			}
		}
	}

	public void computeBefore(){
		backUpEverything ();
		int goodPaths = 0;
		int cutOffLevel = 4;
		int CPUpos = gameObject.GetComponent<playerMovement> ().positions [1];
		int Playerpos = gameObject.GetComponent<playerMovement> ().positions [0];
		int totalPaths = computeTotalPaths(cutOffLevel);
		recursionFromCPU (ref goodPaths, cutOffLevel, 0, CPUpos,Playerpos);
		float probFail = (totalPaths - goodPaths) ;
		probFail /= totalPaths;
		print (totalPaths);
		print (goodPaths);
		print (probFail);
		reLoadEverything ();
		if (probFail > 0.5) {
			string card = whatShouldITry (CPUpos,Playerpos);
			print ("Can't Win If you don't anything now");
			print (card);
			if (card != "") {
				useit (card);
			}
		} else {
			print ("Can Win Move Ahead");
		}
	}

	// Use this for initialization
	void Start () {
		//computeBefore ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
