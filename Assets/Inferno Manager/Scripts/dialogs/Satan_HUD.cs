using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class Satan_HUD : MonoBehaviour
{
     GameObject finalPos;
    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }
    public void moveSatan()
    {
        finalPos = GameObject.Find("ButtonMissions");
        transform.DOMove(finalPos.transform.position, 0.7f);
        transform.DOScale(0.15f, 1f ).OnComplete(createButton);
    }

    void createButton()
    {
        finalPos.transform.localScale = new Vector3(1, 1, 1);
        MenusController.s.destroyMenu("Satan", null);

    }


}