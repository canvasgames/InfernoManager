using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SoulsGroupAlha : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer> ().color = new Color (255, 255, 255, 0);
		GetComponent<SpriteRenderer> ().DOFade (1, 0.6f);

		Animator anim = GetComponent<Animator>();
		AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);//could replace 0 by any other animation layer index
		anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
	}

	public void fadeout() {
		GetComponent<SpriteRenderer> ().DOFade (0, 0.6f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		SendMessageUpwards ("OnClick");
	}
}
