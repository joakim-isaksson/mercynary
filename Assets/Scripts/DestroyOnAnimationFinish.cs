using UnityEngine;
using System.Collections;

public class DestroyOnAnimationFinish : MonoBehaviour {

	private Animator animator;
	public AnimationClip Clip;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		StartCoroutine (DestroyOnFinish ());
	}


	IEnumerator DestroyOnFinish(){
		yield return new WaitForSeconds (Clip.length);
		Destroy (gameObject);
	}
}
