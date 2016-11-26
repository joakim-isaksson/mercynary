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

    [HideInInspector]
    public UnitState State;

    bool attacking;
    Unit attackingTarget;
    bool attackOnCooldown;

    int hitPoints;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;

    void Awake () {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        hitPoints = MaxHitPoints;
        SetState(UnitState.Alive);
        spriteRenderer.sortingOrder = (int)(transform.position.y * 10000);
    }
    
    void Update()
    {
        if (State == UnitState.Alive)
        {
            if (attacking)
            {
                if (!attackOnCooldown)
                {
                    attackOnCooldown = true;
                    attackingTarget.TakeDamage(Random.Range(MinDamage, MaxDamage));
                    StartCoroutine(AttackCooldown());
                }
                if (attackingTarget.State != UnitState.Alive) attacking = false;
            }
            else if (CanMove)
            {
                rb.MovePosition(new Vector2(transform.position.x, transform.position.y + MovementSpeed * Time.deltaTime));
                spriteRenderer.sortingOrder = (int)(transform.position.y * 10000);
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
                attackingTarget = other;
            }

            if (other.CanAttack && !other.attacking)
            {
                other.attacking = true;
                other.attackingTarget = this;
            }
        }
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
                ObjAlive.SetActive(true);
                ObjDead.SetActive(false);
                ObjExpired.SetActive(false);
                boxCollider.enabled = true;
                break;
            case UnitState.Dead:
                ObjAlive.SetActive(false);
                ObjDead.SetActive(true);
                ObjExpired.SetActive(false);
                boxCollider.enabled = false;
                break;
            case UnitState.Expired:
                ObjAlive.SetActive(false);
                ObjDead.SetActive(false);
                ObjExpired.SetActive(true);
                boxCollider.enabled = false;
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
}
