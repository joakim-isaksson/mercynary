using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Castle : MonoBehaviour {

	public GameObject Unit;
	public Text Text;
	public int MaxBreaches = 10;
	public float SpawnRate = 1f;

	private Owner owner;

	private int breachCounter = 0;

	// Use this for initialization
	void Start () {
		StartCoroutine (Spawn ());
		owner = GetComponent<ComponentOwner> ().Owner;
	}
	
	IEnumerator Spawn(){
		while (true) {
			float x = transform.position.x - transform.localScale.x / 2 + Random.Range (0, transform.localScale.x);
			Instantiate (Unit, new Vector3 (x, transform.position.y, transform.position.z), Quaternion.identity);
			yield return new WaitForSeconds (SpawnRate);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		Owner unitowner = col.GetComponent<ComponentOwner> ().Owner;
		if (this.owner != unitowner && unitowner != Owner.Player) {
			Destroy (col.gameObject);
			breachCounter++;
			if (Text != null) {
				Text.text = "" + (MaxBreaches - breachCounter);
			}
			if (breachCounter > MaxBreaches) {
				StopAllCoroutines ();
			}
		}
	}
}
