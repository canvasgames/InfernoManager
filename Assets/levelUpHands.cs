using UnityEngine;
using System.Collections;
using DG.Tweening;

public class levelUpHands : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.DOLocalMoveY(-660f, 0);
        Invoke("handMoveUP", 1f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void handMoveUP()
    {
        transform.DOLocalMoveY(-185, 0.4f).OnComplete(handMoveDown);
    }

    void handMoveDown()
    {
        transform.DOLocalMoveY(-285, 0.4f).OnComplete(handMoveUP);
  
    }
}
