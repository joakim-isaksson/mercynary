using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

    public Owner Owner;
    public bool CanAttack;
    public bool CanShoot;
    public bool CanMove;
    public bool Resurrectable;

    public float MovementSpeed;
    public float AttackCooldownTime;
    public int MinDamage;
    public int MaxDamage;
    public int MaxHitPoints;

    public GameObject ObjAlive;
    public GameObject ObjDead;
    public GameObject ObjExpired;

    [Header("Shooting")]
    public float ShootCooldownTime;
    public GameObject ProjectilePrefab;
    public string ShootingTargetMaskName;
    public float ShootingRange;

    [HideInInspector]
    public UnitState State;

    bool attacking;
    bool shooting;
    Unit attackingTarget;
    Unit shootingTarget;
    bool attackOnCooldown;
    bool shootOnCooldown;

    int hitPoints;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
	int startingLayer;
	Animator animator;

    int layerMask;

    void Awake () {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        layerMask |= 1 << LayerMask.NameToLayer(ShootingTargetMaskName);
		animator = GetComponentInChildren<Animator> ();
    }

    void Start()
    {
        hitPoints = MaxHitPoints;
		startingLayer = gameObject.layer;
		SetState(UnitState.Alive);
    }
    
    void Update()
    {
        if (State == UnitState.Alive)
        {
			if (animator != null) {
				animator.SetBool ("Attacking", attacking);
			}
            if (attacking)
            {
                Stop();
                if (attackingTarget != null)
                {
                    if (!attackOnCooldown)
                    {
                        attackOnCooldown = true;
                        attackingTarget.TakeDamage(Random.Range(MinDamage, MaxDamage));
                        StartCoroutine(AttackCooldown());
                    }
                    if (attackingTarget.State != UnitState.Alive)
                        attacking = false;
                }
                else
                {
                    attacking = false;
                }
            }
            else if (shooting && !shootOnCooldown)
            {
                if (shootingTarget == null || shootingTarget.State != UnitState.Alive)
                {
                    shootingTarget = null;
                    Collider2D hit = Physics2D.OverlapCircle(transform.position, ShootingRange, layerMask);
                    if (hit != null) shootingTarget = hit.gameObject.GetComponent<Unit>();
                }

                if (shootingTarget != null)
                {
                    shootOnCooldown = true;
                    GameObject projectile = (GameObject)Instantiate(ProjectilePrefab, transform.position, transform.rotation);
                    projectile.GetComponent<Projectile>().Target = shootingTarget.transform.position;
                    projectile.GetComponent<Projectile>().Owner = Owner;
                    StartCoroutine(ShootCooldown());
                } 
            }
            else if (CanMove)
            {
                rb.MovePosition(new Vector2(transform.position.x, transform.position.y + MovementSpeed * Time.deltaTime));
            }
        }
    }

    void OnCollisionStay2D(Collision2D collider)
    {
        Unit other = collider.gameObject.GetComponent<Unit>();
        if (other.Owner != Owner.Player && other.Owner != Owner)
        {
            if (CanAttack && !attacking)
            {
                attacking = true;
				Stop ();
                attackingTarget = other;
            }

            if (other.CanAttack && !other.attacking)
            {
                other.attacking = true;
				other.Stop ();
                other.attackingTarget = this;
            }
        }
    }

	public void Stop() {
		if (rb != null) rb.velocity = new Vector2 (0f, 0f);
	}

    public void TakeDamage(int amount)
    {
        if (State != UnitState.Alive) return;

        hitPoints -= amount;
        if (hitPoints < 1)
        {
            SetState(UnitState.Dead);
            StartCoroutine(Expire());
        }
    }

	public void Resurrect() {
		if (Resurrectable && State == UnitState.Dead) {
			StopAllCoroutines ();
            hitPoints = MaxHitPoints;
            SetState(UnitState.Alive);
		}
	}

    void SetState(UnitState newState)
    {
        State = newState;
        switch (State)
        {
			case UnitState.Alive:
                if (CanShoot) shooting = true;
                attacking = false;
				ObjAlive.SetActive (true);
				ObjDead.SetActive (false);
				ObjExpired.SetActive (false);
				gameObject.layer = startingLayer;
                break;
			case UnitState.Dead:
                shooting = false;
                ObjAlive.SetActive (false);
				ObjDead.SetActive (true);
				ObjExpired.SetActive (false);
				attacking = false;
				Stop ();
				gameObject.layer = 10;
                shooting = false;
                break;
            case UnitState.Expired:
				attacking = false;
                ObjAlive.SetActive(false);
                ObjDead.SetActive(false);
                ObjExpired.SetActive(true);
                boxCollider.enabled = false;
                shooting = false;
                break;
        }
    }

    IEnumerator Expire()
    {
        yield return new WaitForSeconds(10f);
        SetState(UnitState.Expired);
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(AttackCooldownTime);
        attackOnCooldown = false;
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(ShootCooldownTime);
        shootOnCooldown = false;
    }
}
