using UnityEngine;
using System.Collections;
using DG.Tweening;

public class tap_animation : MonoBehaviour {

    // Use this for initialization
    void Start() {
        fadeout();
    }

    void fadeout() {
        GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete(() => fadein());
        //GetComponent<CanvasGroup>().dof

    }

    void fadein() {
        GetComponent<CanvasGroup>().DOFade(1, 1f).OnComplete(() => fadeout());

    }


    // Update is called once per frame
    void Update() {

    }
}
