using System.Collections.Generic;
public class BoardState
{
	//N for nothing on that position.
	//T for trap on that position.
	//D for diversion on that position.
	//B for blast on that position.
	//L for lift on that position.
	//CC for Collecting a Card appending to {"Acid","Bulb","Electric","Hack","Health","Hydro","Magnet","Strength","Survival","Trap"} 	
	//CL for Losing a Card appending to {"Acid","Bulb","Electric","Hack","Health","Hydro","Magnet","Strength","Survival","Trap"} 
	//CE for exchanging a Card appending to pair with "-"
	public static string[] board;
	public static int[] chems;
	public static Dictionary<int,int> reference;
	public static Dictionary<int,int> TrapInfo;
	/// <summary>
	/// all functions that checks and adjust
	/// </summary>

	static public int returnChems(int i){
		return chems [i];
	}

	static public bool checkBlast(int i){
		if (board [i] == "B") {
			return true;
		}
		return false;
	}

	static public bool checkTrap(int i){
		if (!TrapInfo.ContainsKey (i)) {
			return false;
		}
		if (TrapInfo [i] >= 0) {
			removeTrap (i);
			return false;
		}
		TrapInfo [i] = TrapInfo[i] + 1;
		return true;
	}

	static public int checkAndUseLift(int i){
		if (board [i] == "L") {
			board [i] = "N";
			board [reference [i]] = "L";

			return reference [i];
		} else {
			return i;
		}
	}

	static public int checkAndDivert(int i){
		if (board [i] == "D") {
			return reference [i];
		} else {
			return i;
		}
	}

	static public bool checkForQuestion(int i){
		if (board [i] == "Q") {
			return true;
		}
		return false;
	}

	static public string checkAndReturnCard(int i){
		if (board [i].Length > 2 && board [i][1]=='C') {
			return board [i].Substring (2);
		}
		return null;
	}

	static public string checkAndLoseCard(int i){
		if (board [i].Length > 2 && board [i] [1] == 'L') {
			return board [i].Substring (2);
		}
		return null;
	}

	/// <summary>
	/// Sets the trap.
	/// </summary>
	/// <returns><c>true</c>, if trap was set, <c>false</c> otherwise.</returns>
	/// <param name="i">The index.</param>
	static public bool setTrap(int i){
		if (board [i][0] != 'N' && board[i][0] != 'C')
			return false;
		board [i] = "T";
		TrapInfo [i] = -2;
		return true;
	}

	/// <summary>
	/// Removes the trap.
	/// </summary>
	/// <returns><c>true</c>, if trap was removed, <c>false</c> otherwise.</returns>
	/// <param name="i">The index.</param>
	static public bool removeTrap(int i){
		if (board [i] == "T") {
			board [i] = "N";
		}
		return true;
	}

	static public bool checkQuestion(int i){
		if (board [i] == "Q") {
			return true;
		} else
			return false;
	}

	static public void intializeBoard(){
		board = new string[200];
		chems = new int[200];
		reference = new Dictionary<int,int> ();
		TrapInfo = new Dictionary<int,int> ();
		for (int i = 0; i < 200; i++) {
			board [i] = "N";
			chems [i] = 0;
		}
		chems [29] = 20;
		chems [47] = 30;
		chems [78] = 50;
		chems [79] = 50;
		board [1] = "CCHydro";
		board [3] = "CEHydro-Acid";
		board [5] = "CCSurvival";
		board [8] = "CCHealth";
		board [11] = "CEHydro-Acid";
		board [12] = "CEHydro-Acid";
		board [14] = "CCHack";
		board [15] = "CCTrap";
		board [19] = "CLHealth";
		board [20] = "CCStrength";
		board [25] = "CEHydro-Acid";
		board [26] = "CCMagnet";
		board [29] = "CCSurvival";
		board [47] = "CCTrap";
		board [78] = "CCHack";
		board [79] = "CCHydro";
		board [46] = "CLHydro";
		board [30] = "CEHydro-Acid";
		board [32] = "CCElectric";
		board [37] = "CEHydro-Acid";
		board [38] = "CEHydro-Acid";
		board [56] = "CEHydro-Acid";
		board [42] = "CCStrength";
		board [53] = "CCHealth";
		board [74] = "CCBulb";
		board [104] = "Q";
		board [106] = "Q";
		board [109] = "Q";
		board [111] = "Q";
		board [96] = "Q";
		board [99] = "Q";
		board [64] = "CCMagnet";
		board [71] = "CCHealth";

		board [92] = "B";
		reference [92] = 1;
		board [4] = "D";
		reference [4] = 2;
		board [9] = "D";
		reference [9] = 2;
		board [24] = "D";
		reference [24] = 18;
		board [33] = "D";
		reference [33] = 18;
		board [35] = "D";
		reference [35] = 18;
		board [55] = "D";
		reference [55] = 54;
		board [80] = "D";
		reference [80] = 54;
		board [82] = "D";
		reference [82] = 54;
		board [87] = "D";
		reference [87] = 86;
		//^snakes
		//ladders
		board [21] = "L";
		reference [21] = 39;
		reference [39] = 21;
		board [76] = "L";
		reference [76] = 108;
		reference [108] = 76;
		board [13] = "L";
		reference [13] = 31;
		reference [31] = 13;
		board [52] = "L";
		reference [52] = 116;
		reference [116] = 52;
		board [65] = "L";
		reference [65] = 97;
		reference [97] = 65;
	}


};

