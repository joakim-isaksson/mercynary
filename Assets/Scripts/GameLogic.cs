using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameLogic : MonoBehaviour {

	private int allyCounter = 0;
	private int enemyCounter = 0;

	private bool allyFallen = false;
	private bool enemyFallen = false;

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

	public void SideFallen(Owner owner){
		if (owner == Owner.Ally) {
			allyFallen = true;
		} else {
			enemyFallen = true;
		}
	}

	void Update(){
		if (allyCounter - enemyCounter > 25 && enemyFallen) {
			YouWin ();
		}
		if(allyCounter - enemyCounter < -25 && allyFallen){
			GameOver ();
		}

		if (allyFallen && enemyFallen) {
			YouWin ();
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
