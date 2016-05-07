using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class loadBoard: MonoBehaviour {

	public void loadScene(string name){
		SceneManager.LoadScene (name);
	}
}
