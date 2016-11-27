using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Castle : MonoBehaviour {

	public GameObject Unit;
	public Text Text;
	public int MaxBreaches = 10;
	public float SpawnRate = 1f;
	public Owner Owner;

	private int breachCounter = 0;
	private GameLogic game;

	// Use this for initialization
	void Start () {
		game = FindObjectOfType<GameLogic>();
		StartCoroutine (Spawn ());
	}
	
	IEnumerator Spawn(){
		while (true) {
			float x = transform.position.x - transform.localScale.x / 2 + Random.Range (0, transform.localScale.x);
			Instantiate (Unit, new Vector3 (x, transform.position.y, transform.position.z), Quaternion.identity);
			game.AddUnit (Owner);
			yield return new WaitForSeconds (SpawnRate);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Player" || col.tag == "Arrow") return;
		Owner unitowner = col.GetComponent<Unit> ().Owner;
		if (this.Owner != unitowner) {
			game.RemoveUnit (unitowner);
			Destroy (col.gameObject);
			breachCounter++;
			if (Text != null) {
				if (MaxBreaches - breachCounter < 0) {
					Text.text = "0";
				} else {
					Text.text = "" + (MaxBreaches - breachCounter);
				}
			}
			if (breachCounter > MaxBreaches) {
				StopAllCoroutines ();
			}
		}
	}
}
