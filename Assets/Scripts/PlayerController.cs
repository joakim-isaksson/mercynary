using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float Speed = 5;
	public float DashDistance = 1f;
	public float MaxDashingSpeed = 3f;
	public float DashCooldownTime = 5f;
	public AnimationCurve DashCurve;
	public Text Text;
	public GameObject halo;

	Rigidbody2D rb;
	bool dashing;
	bool dashOnCooldown;
	Vector2 dashingTarget;
	float dashTimer = 0; 
	int layerMask;
	int playerMaxHealth = 10;
	int playerCurrentHealth = 10;
	float resurrectRingRange = 0.1f;
	SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		dashing = false;
		dashOnCooldown = false;
		layerMask |= 1 << LayerMask.NameToLayer ("Resurrectable");
		spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
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
			float curveValue = DashCurve.Evaluate (dashTimer);
			rb.MovePosition (Vector2.MoveTowards (transform.position, dashingTarget, curveValue * MaxDashingSpeed));
			spriteRenderer.color = new Color (1 - curveValue, 1 - curveValue, 1 - curveValue);
			if (Vector2.Distance (transform.position, dashingTarget) < 0.1) {
				dashing = false;
				spriteRenderer.color = new Color (1, 1, 1);
			}
		}
		if (Input.GetButtonDown("Resurrect")) {
			Collider2D[] hits = Physics2D.OverlapCircleAll (transform.position, resurrectRingRange, layerMask); // We kind of need the colliders on
			foreach (Collider2D hit in hits) {
				Unit hitUnit = hit.gameObject.GetComponent<Unit> ();
				if (hitUnit != null && hitUnit.Owner == Owner.Ally) {
					hitUnit.Resurrect ();
				}
			}
			resurrectRingRange = 0.1f;
		}
		resurrectRingRange += Time.deltaTime / 2;
		if (resurrectRingRange > 2f) {
			resurrectRingRange = 2f;
		}
		halo.transform.localScale = new Vector3 (resurrectRingRange * 2 * Mathf.PI, resurrectRingRange * 2 * Mathf.PI, 1);
	}

	IEnumerator DashCooldown(){
		yield return new WaitForSeconds (DashCooldownTime);
		dashOnCooldown = false;
		dashTimer = 0;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Unit") {
			if (col.GetComponent<Unit> ().Owner == Owner.Enemy && !dashing) {
				TakeDamage ();
			}
		}
	}

	void TakeDamage(){
		playerCurrentHealth--;
		Text.text = "Health: " + playerCurrentHealth;
	}
}
