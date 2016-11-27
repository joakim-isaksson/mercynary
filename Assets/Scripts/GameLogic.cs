using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameLogic : MonoBehaviour {

	private int allyCounter = 0;
	private int enemyCounter = 0;

	public GameObject gameOver;

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
		if (allyCounter - enemyCounter > 10) {
			print ("You Win!");
			if (gameOver != null) {
				gameOver.SetActive (true);
				StartCoroutine(LoadCredits ());
			}
		}

		if(allyCounter - enemyCounter < -50){
			print("You Lose");
			gameOver.SetActive (true);
			StartCoroutine(LoadCredits ());
		}

		print (allyCounter - enemyCounter + " " + (allyCounter + enemyCounter));
	}

	IEnumerator LoadCredits(){
		yield return new WaitForSeconds (5f);
		SceneManager.LoadScene ("Credits");
	}
}
