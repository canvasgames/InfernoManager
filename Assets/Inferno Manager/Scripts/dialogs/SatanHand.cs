using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;

public class SatanHand : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (GLOBALS.s.TUTORIAL_PHASE == 4 || GLOBALS.s.TUTORIAL_PHASE == 17)
            tutorialCollect();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (GLOBALS.s.TUTORIAL_PHASE == 13)
            {
                destroyMe();
            }
        }
    }
    void tutorialCollect()
    {
        // transform.DOLocalMove(new Vector3(128, -112, 52), 0.7f).OnComplete(moveback);
        transform.DOLocalMove(new Vector3(112, -267, -200), 0.7f).OnComplete(moveback);
    }
    void moveback()
    {
        //transform.DOLocalMove(new Vector3(110, -87, 8.6f), 0.7f).OnComplete(tutorialCollect);
        transform.DOLocalMove(new Vector3(75, -227, -200), 0.7f).OnComplete(tutorialCollect);
    }


    public void initHandSoulsTutorial()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        transform.localPosition = new Vector3(-316, 226, 0);

        tutorialSouls();
    }

    void tutorialSouls()
    {
        transform.DOLocalMoveY(180, 0.7f).OnComplete(tutorialsSoulsBack);
    }
    void tutorialsSoulsBack()
    {
        transform.DOLocalMoveY(240, 0.7f).OnComplete(tutorialSouls);
    }

    public void initRankTutorial()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        transform.localPosition = new Vector3(-557, 180, 0);

        rankTutorial();
    }

    void rankTutorial()
    {
        transform.DOLocalMoveY(110, 0.7f).OnComplete(rankTutorialBack);
    }

    void rankTutorialBack()
    {
        transform.DOLocalMoveY(180, 0.7f).OnComplete(rankTutorial);
    }

    public void tutorialList()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        reposite();
    }

    void reposite()
    {
        transform.localPosition = new Vector3(205, 105, 0);
        Invoke("move", 1f);
    }
    void move()
    {
        transform.DOMoveY(-10f, 1f).OnComplete(reposite);
    }

    void destroyMe()
    {
        MenusController.s.destroyMenu("SatanHand", null);
    }

}
