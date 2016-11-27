using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadMainSceneOnAnyKey : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) {
			SceneManager.LoadScene ("Main");
		}
	}
}
