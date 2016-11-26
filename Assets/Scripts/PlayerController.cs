using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float Speed = 5;
	public float DashDistance = 1f;
	public float MaxDashingSpeed = 3f;
	public float DashCooldownTime = 5f;
	public AnimationCurve DashCurve;

	Rigidbody2D rb;
	bool dashing;
	bool dashOnCooldown;
	Vector2 dashingTarget;
	float dashTimer = 0; 
	int layerMask;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		dashing = false;
		dashOnCooldown = false;
		layerMask |= 1 << LayerMask.NameToLayer ("Resurrectable");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Horizontal") > 0) {
			GetComponentInChildren<SpriteRenderer> ().flipX = true;
		} else {
			GetComponentInChildren<SpriteRenderer> ().flipX = false;
		}

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
			dashTimer += Time.deltaTime;
			rb.MovePosition (Vector2.MoveTowards (transform.position, dashingTarget, DashCurve.Evaluate(dashTimer) * MaxDashingSpeed));
			if (Vector2.Distance (transform.position, dashingTarget) < 0.1) {
				dashing = false;
			}
		}
		if (Input.GetButtonDown("Resurrect")) {
			Collider2D[] hits = Physics2D.OverlapCircleAll (transform.position, 2f, layerMask); // We kind of need the colliders on
			foreach (Collider2D hit in hits) {
				Unit hitUnit = hit.gameObject.GetComponent<Unit> ();
				if (hitUnit != null && hitUnit.Owner == Owner.Ally) {
					hitUnit.Resurrect ();
				}
			}
		}
	}

	IEnumerator DashCooldown(){
		yield return new WaitForSeconds (DashCooldownTime);
		dashOnCooldown = false;
		dashTimer = 0;
	}
}
