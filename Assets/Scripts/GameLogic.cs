using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameLogic : MonoBehaviour {

	private int allyCounter = 0;
	private int enemyCounter = 0;

	public GameObject gameOver;
	public GameObject youWin;

	public void AddUnit(Owner owner){
		if (owner == Owner.Ally) {
			allyCounter++;
		} else {
			enemyCounter++;
		}
	}

	public void RemoveUnit(Owner owner){
		if (owner == Owner.Ally) {
			allyCounter--;
		} else {
			enemyCounter--;
		}
	}

	void Update(){
		if (allyCounter - enemyCounter > 20) {
			YouWin ();
		}
		if(allyCounter - enemyCounter < -50){
			GameOver ();
			StartCoroutine(LoadCredits ());
		}
	}

	public void GameOver(){
		if (gameOver != null) {
			gameOver.SetActive (true);
			StartCoroutine(LoadCredits ());
		}
	}

	public void YouWin(){
		if (youWin != null) {
			youWin.SetActive (true);
			StartCoroutine(LoadCredits ());
		}
	}

	IEnumerator LoadCredits(){
		yield return new WaitForSeconds (5f);
		SceneManager.LoadScene ("Credits");
	}
}
