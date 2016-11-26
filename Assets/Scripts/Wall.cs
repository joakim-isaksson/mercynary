using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public GameObject Unit;

	private Owner owner;

	// Use this for initialization
	void Start () {
		StartCoroutine (Spawn ());
		owner = GetComponent<ComponentOwner> ().Owner;
	}
	
	IEnumerator Spawn(){
		while (true) {
			float x = transform.position.x - transform.localScale.x / 2 + Random.Range (0, transform.localScale.x);
			Instantiate (Unit, new Vector3 (x, transform.position.y, transform.position.z), Quaternion.identity);
			yield return new WaitForSeconds (1f);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		Owner unitowner = col.GetComponent<ComponentOwner> ().Owner;
		if (this.owner != unitowner && unitowner != Owner.Player) {
			Destroy (col.gameObject);
		}
	}
}
