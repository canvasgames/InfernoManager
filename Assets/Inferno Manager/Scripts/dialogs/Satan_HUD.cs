using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class Satan_HUD : MonoBehaviour
{
     GameObject finalPos;
    float originalScale;
    public void moveSatan()
    {
        originalScale = transform.localScale.x;
        finalPos = GameObject.Find("ButtonMissions");
        transform.DOMove(new Vector3(-6.37f, 4.36f,0f), 0.7f);
        transform.DOScale(0.15f, 1f ).OnComplete(createButton);
    }

    void createButton()
    {
        finalPos.transform.localScale = new Vector3(1, 1, 1);
        transform.DOScale(originalScale, 1f).OnComplete(createButton);
        transform.gameObject.SetActive(false);
    }
}