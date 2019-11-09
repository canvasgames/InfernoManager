using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class EnterFromDown : MonoBehaviour {
    // Use this for initialization
    void Start() {
        transform.DOLocalMoveY(-620f, 0);
        Invoke("moveDown", 1f);

        Debug.Log("======== RANK TEXT: " + GLOBALS.s.USER_RANK);
        Text myText = GetComponent<Text>();

        myText.text = "Rank\n"+GLOBALS.s.GetRankName(GLOBALS.s.USER_RANK);
    }
    

    // Update is called once per frame
    void Update()
    {

    }

    void moveDown()
    {
        transform.DOLocalMoveY(-313, 0.4f);
    }


}