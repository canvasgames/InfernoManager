using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Arrow3D : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void googogogo()
    {
        transform.DOMoveY(347f, 0.7f).OnComplete(backbackback);
    }

    void backbackback()
    {
        transform.DOMoveY(363f, 0.7f).OnComplete(googogogo);
    }
}
