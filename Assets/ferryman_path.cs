using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ferryman_path : MonoBehaviour {
    int ferry_animation = 0;
    float ferry_time = 1f;
	// Use this for initialization
	void Start () {
        set_position_initial();
        
        //transform.DOLocalPath(positionArray,4).OnComplete(oncompletttttttt);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void wait_for_it()
    {
        Invoke("play_ferry_animation",0.9f);
    }

    void play_ferry_animation()
    {
        transform.GetComponent<Animator>().Play("ferry");

        if (ferry_animation == 0)
        {
            ferry_animation++;
            Invoke("set_position_initial", 1);
        }
        else if (ferry_animation == 1)
        {
            ferry_animation++;
            Invoke("part_1", ferry_time);
        }
        else if (ferry_animation == 2)
        {
            ferry_animation++;
            Invoke("part_2", ferry_time);
        }
        else if (ferry_animation == 3)
        {
            ferry_animation++;
            Invoke("part_3", ferry_time);
        }
        else if (ferry_animation == 4)
        {
            ferry_animation++;
            Invoke("part_4", ferry_time);
        }
        else if (ferry_animation == 5)
        {
            ferry_animation++;
            Invoke("part_5", ferry_time);
        }
        else if (ferry_animation == 6)
        {
            ferry_animation = 0;
            Invoke("part_6", ferry_time);
        }

    }
    void part_1()
    {
        transform.DOLocalMove(new Vector3(7.08f, -0.83f, 4.22f), 8f).OnComplete(wait_for_it);
    }

    void part_2()
    {
        transform.DOLocalMove(new Vector3(5.3f, -0.83f, 4.22f), 4f).OnComplete(wait_for_it);
    }

    void part_3()
    {
        transform.DOLocalMove(new Vector3(3.35f, -2.32f, 4.22f), 8f).OnComplete(wait_for_it);
    }
    void part_4()
    {
        transform.DOLocalMove(new Vector3(1.75f, -2.83f, 4.22f), 4f).OnComplete(wait_for_it);
    }
    void part_5()
    {
        transform.DOLocalMove(new Vector3(-0.21f, -3.96f, 4.22f), 7f).OnComplete(wait_for_it);
    }
    void part_6()
    {
        transform.DOLocalMove(new Vector3(-2.78f, -5.93f, 4.22f), 8f).OnComplete(wait_for_it);
    }

    void set_position_initial()
    {
        transform.localPosition = new Vector3(9.22f, 0.29f, 4.22f);
        Invoke("wait_for_it", ferry_time);

    }
    //   
}
