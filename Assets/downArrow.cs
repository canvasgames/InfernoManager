using UnityEngine;
using System.Collections;
using DG.Tweening;
public class downArrow : MonoBehaviour {

	// Use this for initialization
	void Start () {
        tutorialBTBuild();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void tutorialBTBuild()
    {
        if (GLOBALS.s.TUTORIAL_PHASE != 21)
            transform.DOLocalMoveY(-45, 0.7f).OnComplete(back);
        else
            tutorialTabFire();
    }

    void back()
    {
        if (GLOBALS.s.TUTORIAL_PHASE != 21)
            transform.DOLocalMoveY(-115, 0.7f).OnComplete(tutorialBTBuild);
        else
            tutorialTabFire();
    }

    void tutorialTabFire()
    {
        transform.DOLocalMoveY(-207, 0.5f).OnComplete(tutorialTabFireBack);
    }

    void tutorialTabFireBack()
    {
        transform.DOLocalMoveY(-170, 0.5f).OnComplete(tutorialTabFire);
    }
}
