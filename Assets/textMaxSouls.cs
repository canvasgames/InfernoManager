using UnityEngine;
using System.Collections;
using DG.Tweening;

public class textMaxSouls : MonoBehaviour
{
    bool pare = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void pulse()
    {
        if(pare == false)
            transform.DOScale(new Vector3(1.4f, 1.4f, 1.4f), 0.7f).OnComplete(repulse);
    }

    void repulse()
    {
        if (pare == false)
            transform.DOScale(new Vector3(1f, 1f, 1f), 0.7f).OnComplete(pulse);
    }

    public void stop()
    {
        transform.DOKill();
        transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
        pare = true;
    }
}
