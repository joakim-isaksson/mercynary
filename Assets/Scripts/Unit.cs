using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

    public int MinDamage;
    public int MaxDamage;
    public int MaxHitPoints;

    public GameObject ObjAlive;
    public GameObject ObjDead;
    public GameObject ObjExpired;

    [HideInInspector]
    public UnitState State;

    int hitPoints;
    SpriteRenderer spriteRenderer;
    
	void Start () {
        SetState(UnitState.Alive);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    
    void Update()
    {
        spriteRenderer.sortingOrder = (int)(transform.position.y * 10000);

		if (Random.Range (1, 1000) < 5) {
			TakeDamage (5);
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

    IEnumerator Expire()
    {
        yield return new WaitForSeconds(10f);
        SetState(UnitState.Expired);
    }

	public void Resurrect(){
		if (State == UnitState.Dead) {
			StopAllCoroutines ();
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
                break;
            case UnitState.Dead:
                ObjAlive.SetActive(false);
                ObjDead.SetActive(true);
                ObjExpired.SetActive(false);
                break;
            case UnitState.Expired:
                ObjAlive.SetActive(false);
                ObjDead.SetActive(false);
                ObjExpired.SetActive(true);
                break;
        }
    }
}
