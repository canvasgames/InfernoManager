using UnityEngine;
using System.Collections;
using DG.Tweening;

public class demonMoving : MonoBehaviour {

	// Use this for initialization
	void Start () {
        move();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void move()
    {
        float time;
        time = Random.Range(15f, 25f);
        transform.DOLocalMoveX(transform.position.x - 110, time).OnComplete(destroy);
    }
    void destroy()
    {
        Destroy(transform.gameObject);
    }

}
