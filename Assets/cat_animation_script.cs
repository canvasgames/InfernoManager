using UnityEngine;
using System.Collections;
using DG.Tweening;

public class cat_animation_script : MonoBehaviour {

    float ypos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void start_shaking()
    {
        ypos = transform.localPosition.y;
        transform.DOLocalMoveY(ypos - 0.4f, 0.1f).OnComplete(() => shake_finished());
    }

    void shake_finished()
    {
        transform.DOLocalMoveY(ypos + 0.4f, 0.1f).OnComplete(() => shake_finished_down());
    }

    void shake_finished_down()
    {
        transform.DOLocalMoveY(ypos - 0.4f, 0.1f).OnComplete(() => shake_finished());
    }

}
