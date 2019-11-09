using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ExclamationAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ScaleDown();
	}

    void ScaleDown() {
        transform.DOScale(0.8f, 0.7f).OnComplete(() => ScaleUp());
    }

    void ScaleUp() {
        transform.DOScale(1f, 0.7f).OnComplete(() => ScaleDown());
    }
	// Update is called once per frame
	void Update () {
	
	}
}
