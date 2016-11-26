using UnityEngine;
using System.Collections;

public class VerticalSortOrder : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.sortingOrder = (int)(transform.position.y * -30000);
	}
	
	// Update is called once per frame
	void Update () {
		spriteRenderer.sortingOrder = (int)(transform.position.y * -30000);
	}
}
