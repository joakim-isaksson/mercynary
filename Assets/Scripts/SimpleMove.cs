using UnityEngine;
using System.Collections;

public class SimpleMove : MonoBehaviour {

    public float Speed;

    Rigidbody2D rb;
    Unit unit;
    
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        unit = GetComponent<Unit>();
    }
    
    void Update()
    {
        if (unit.State == UnitState.Alive)
            rb.MovePosition(new Vector2(transform.position.x, transform.position.y + Speed * Time.deltaTime));
    }
}
