using UnityEngine;
using System.Collections;
using DG.Tweening;

public class light_glow_scale : MonoBehaviour {

    public float transition_time = 0;
    public float max_scale = 0;
    public float min_scale = 0;

    // Use this for initialization
    void Start () {

        transform.localScale = new Vector3 (min_scale, min_scale, min_scale);
        resize_up();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void resize_up()
    {
        transform.DOScale(max_scale, transition_time).OnComplete(resize_down);
    }

    void resize_down()
    {
        transform.DOScale(min_scale, transition_time).OnComplete(resize_up);
    }
}
