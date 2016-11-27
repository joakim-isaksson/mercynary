using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

	private int allyCounter = 0;
	private int enemyCounter = 0;

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
		}

		if(allyCounter - enemyCounter < -50){
			print("You Lose");
		}

		print (allyCounter - enemyCounter + " " + (allyCounter + enemyCounter));
	}
}
