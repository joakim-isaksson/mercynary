using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	public float Speed = 5;

	Rigidbody2D rb;
	private float dashMultiplier = 1f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void Update () {
		rb.MovePosition(new Vector2(transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * Speed * dashMultiplier, transform.position.y + Input.GetAxis("Vertical") * Time.deltaTime * Speed * dashMultiplier));
		if (Input.GetButtonDown("Resurrect")) {
			print ("Resurrecting!");
			Collider2D[] hits = Physics2D.OverlapCircleAll (transform.position, 2f);
			foreach (Collider2D hit in hits) {
				Unit hitUnit = hit.gameObject.GetComponent<Unit> ();
				if (hitUnit != null) {
					hitUnit.Resurrect ();
				}
			}
		}
		if (Input.GetButton ("Dash")) {
			dashMultiplier = 2f;
		} else {
			dashMultiplier = 1f;
		}
	}
}
