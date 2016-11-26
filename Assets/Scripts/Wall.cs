using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public GameObject Unit;

	public Owner Owner;

	// Use this for initialization
	void Start () {
		StartCoroutine (Spawn ());
	}
	
	IEnumerator Spawn() {
		while (true) {
			float x = transform.position.x - transform.localScale.x / 2 + Random.Range (0, transform.localScale.x);
			Instantiate (Unit, new Vector3 (x, transform.position.y, transform.position.z), Quaternion.identity);
			yield return new WaitForSeconds (1f);
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
        Unit other = col.gameObject.GetComponent<Unit>();
        if (other.Owner != Owner.Player && other.Owner != Owner) {
            Destroy (col.gameObject);
		}
	}
}