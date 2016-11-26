﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float Speed = 5;
	public float DashDistance = 1f;
	public float DashingSpeed = 10f;
	public float DashCooldownTime = 5f;

	Rigidbody2D rb;
	bool dashing;
	bool dashOnCooldown;
	Vector2 dashingTarget;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		dashing = false;
		dashOnCooldown = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!dashing) {
			rb.MovePosition (new Vector2 (
				transform.position.x + Input.GetAxis ("Horizontal") * Time.deltaTime * Speed,
				transform.position.y + Input.GetAxis ("Vertical") * Time.deltaTime * Speed
			));

			if (!dashOnCooldown && Input.GetButtonDown ("Dash")) {
				dashing = true;
				dashOnCooldown = true;
				Vector3 dashingDirection = new Vector3 (
					Input.GetAxis ("Horizontal"),
					Input.GetAxis ("Vertical"),
					0
				).normalized;
				dashingTarget = transform.position + dashingDirection * DashDistance;
				StartCoroutine (DashCooldown ());
			}
		} else {
			rb.MovePosition (Vector2.MoveTowards (transform.position, dashingTarget, Time.deltaTime * DashingSpeed));
			if (Vector2.Distance (transform.position, dashingTarget) < 0.1) {
				dashing = false;
			}
		}
		if (Input.GetButtonDown("Resurrect")) {
			Collider2D[] hits = Physics2D.OverlapCircleAll (transform.position, 2f);
			foreach (Collider2D hit in hits) {
				Unit hitUnit = hit.gameObject.GetComponent<Unit> ();
				if (hitUnit != null) {
					hitUnit.Resurrect ();
				}
			}
		}

	}

	IEnumerator DashCooldown(){
		yield return new WaitForSeconds (DashCooldownTime);
		dashOnCooldown = false;
	}
}
