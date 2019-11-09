using UnityEngine;
using System.Collections;
using DG.Tweening;

public class enterFromUp : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        transform.DOLocalMoveY(770f, 0);
        Invoke("moveDown", 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void moveDown()
    {
        transform.DOLocalMoveY(40, 0.4f);
    }


}