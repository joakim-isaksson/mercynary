using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float Speed;
    public float DecayTime;
    public int ShootDamage;

    [HideInInspector]
    public Vector3 Target;
    [HideInInspector]
    public Owner Owner;

    bool flying;
    bool stopped;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

	void Awake () {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        flying = true;
        stopped = false;
    }

	void Update () {
        if (stopped) return;
        rb.MovePosition(Vector2.MoveTowards(transform.position, Target, Time.deltaTime * Speed));
        if (Vector2.Distance(transform.position, Target) < 0.001f)
        {
            stopped = true;
            StartCoroutine(StopFlying());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!flying || other.tag == "Tower" || other.tag == "Wall" || other.tag == "Arrow") return;

        if (other.tag == "Unit")
        {
            Unit target = other.gameObject.GetComponent<Unit>();
            if (target.Owner != Owner && target.State == UnitState.Alive)
            {
                other.gameObject.GetComponent<Unit>().TakeDamage(ShootDamage);
                Destroy(gameObject);
            }
        }
        else if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage();
            Destroy(gameObject);
        }
        
    }

    IEnumerator StopFlying()
    {
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sortingLayerName = "Details";
        spriteRenderer.sortingOrder = 10;
        flying = false;
    }
}
