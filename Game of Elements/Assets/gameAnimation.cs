
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class gameAnimation : MonoBehaviour {
	public string[] message;
	public GameObject[] messageBox;
	public Animation[] anim;
	public AudioClip[] audios;
	private Sprite[] ElementImages;
	private Dictionary<String,Sprite> mapElementImages = new Dictionary<String,Sprite>();
	public GameObject[] Lifts;
	private int time=0;
	private int t = 0;
	public GameObject Qpanel;

	public GameObject explosion;


	public void setLifts(){
		int li = 0;
		for (int i = 0; i < BoardState.board.Length; i++) {
			if (BoardState.board [i] == "L") {
				Lifts [li].transform.position = GameObject.Find ("GameObject (" + i.ToString () + ")").transform.position + new Vector3 (0f,0.3f,0);
//				print (li);
				li++;
			}
		}
	}


	public void GreetOrBored(bool isGreet){
		if(isGreet){
			anim [States.turn].GetClip ("greet_00").wrapMode = WrapMode.Once;
//			print("Greet_00"+ WrapMode.Once.ToString() + anim [States.turn].GetClip ("greet_00").wrapMode.ToString());
			anim [States.turn].clip = anim [States.turn].GetClip ("greet_00");
			anim [States.turn].Play ();
		}
		else {
			anim [States.turn].PlayQueued ("thinking_00");
		}
	}

	public void PlayAnimation(bool isSad){
		if (isSad) {
			anim [States.turn].GetClip ("refuse_00").wrapMode = WrapMode.Once;
			anim [States.turn].clip = anim [States.turn].GetClip ("refuse_00");
			anim [States.turn].Play ();
		} else {
			anim [States.turn].GetClip ("pose_02").wrapMode = WrapMode.Once;
			anim [States.turn].clip = anim [States.turn].GetClip ("pose_02");
			anim [States.turn].Play ();
		}
		GreetOrBored (false);
	}

	public void PlayAnimation(String animationname){
		anim [States.turn].GetClip (animationname).wrapMode = WrapMode.Once;
		anim [States.turn].clip = anim [States.turn].GetClip (animationname);
		GreetOrBored (false);
	}

	public void PlayAnimationL(int turn){
		messageBox [2 * States.turn].gameObject.SetActive(false);
		messageBox [(2 * States.turn) + 1].gameObject.SetActive(false);
		messageBox [2 * (1 - States.turn)].gameObject.SetActive(false);
		messageBox [(2 * (1 - States.turn))+1].gameObject.SetActive(false);
		messageBox [(States.turn)+4].gameObject.SetActive(false);
		messageBox [(1-States.turn)+4].gameObject.SetActive(false);

		anim [States.turn].gameObject.SetActive(true);
		anim [1-States.turn].gameObject.SetActive(true);
		anim [turn].GetClip ("greet_04").wrapMode = WrapMode.Loop;
		anim [turn].clip = anim [turn].GetClip ("greet_04");
		anim [turn].Play ();
		anim [1-turn].GetClip ("down_23").wrapMode = WrapMode.Loop;
		anim [1 - turn].clip = anim [1 - turn].GetClip ("down_23");
		anim [1 - turn].Play ();
	}



	public void setImage(){
		messageBox [4 + States.turn].gameObject.GetComponent<Image> ().sprite = mapElementImages[gameObject.GetComponent<playerMovement>().positions[States.turn].ToString()];
	}

	public void TurnChange()
	{
		messageBox [2 * States.turn].gameObject.SetActive(false);
		messageBox [(2 * States.turn) + 1].gameObject.SetActive(false);
		messageBox [2 * (1 - States.turn)].gameObject.SetActive(false);
		messageBox [(2 * (1 - States.turn))+1].gameObject.SetActive(false);
		messageBox [(States.turn)+4].gameObject.SetActive(true);
		messageBox [(1-States.turn)+4].gameObject.SetActive(false);
		anim [States.turn].gameObject.SetActive(true);
		anim [1-States.turn].gameObject.SetActive(false);
	}

	public void PlayExplosion()
	{
		explosion.GetComponent<ParticleSystem>().Play();
	}

	public void PlaySound(int i){
		gameObject.GetComponent<AudioSource> ().clip = audios [i];
		gameObject.GetComponent<AudioSource> ().Play ();
	}

	public void DisplayQuestionPanel(string msg){
		
	}


	public void DisplayCustomMessage(string msg){
		messageBox [(States.turn)+4].gameObject.SetActive(false);
		messageBox [(2 * States.turn) + 1].gameObject.SetActive(true);
		messageBox [2 * States.turn].gameObject.SetActive(true);
		messageBox [(2 * States.turn) + 1].gameObject.GetComponent<Text> ().text = msg;
		print (msg);
	}

	public void DisplayMessage(int i){
		if (i >= 0 && i < message.Length) {
			messageBox [(States.turn)+4].gameObject.SetActive(false);
			messageBox [(2 * States.turn) + 1].gameObject.SetActive(true);
			messageBox [2 * States.turn].gameObject.SetActive(true);
			messageBox [(2 * States.turn) + 1].gameObject.GetComponent<Text> ().text = message [i];
			print (message [i]);
		} else {
			messageBox [(2 * States.turn) + 1].gameObject.SetActive(false);
			messageBox [2 * States.turn].gameObject.SetActive(false);
		}
	}
	// Use this for initialization
	void Start () {
		
		for (int i = 0; i < 2; i++) {
			anim [i].GetClip ("thinking_00").wrapMode = WrapMode.Loop;
			anim [i].clip = anim [States.turn].GetClip ("thinking_00");
			anim [i].Play ();
		}
		GreetOrBored (true);
		GreetOrBored (false);
		TurnChange ();
		messageBox [(States.turn)+4].gameObject.SetActive(false);
		messageBox [2 * States.turn].gameObject.SetActive(true);
		messageBox [(2 * States.turn) + 1].gameObject.SetActive(true);

		ElementImages = Resources.LoadAll<Sprite> ("");
		foreach (Sprite s in ElementImages){
			mapElementImages.Add (s.name, s);
		}
		setImage ();
		setLifts ();
	}
	
	// Update is called once per frame
	void Update () {
		if (time == 0) {
			for (int i = 0; i < Lifts.Length; i++) {
				if (t == 0) {
					Lifts [i].GetComponent<SpriteRenderer> ().color = Color.green;
				} else {
					Lifts [i].GetComponent<SpriteRenderer> ().color = Color.white;
				}
			}
			t = (t+1) %2;
		}
		time = (time + 1) % 75;

	}
}
