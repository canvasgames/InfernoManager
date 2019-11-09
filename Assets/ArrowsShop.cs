using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ArrowsShop : MonoBehaviour
{
    public static ArrowsShop s;
    //ScrollBar
    // Use this for initialization
    Scrollbar bar;
    public GameObject arrowLeft, arrowRight;
    bool inRightTab = true;
    void Awake()
    {
        s = this;
    }
    void Start()
    {
        bar = transform.GetComponent<Scrollbar>();

    }

    public void recieveTab(int tab)
    {   
        if(tab == 0)
        {
            inRightTab = true;
            arrowLeft.SetActive(true);
            arrowRight.SetActive(true);
        }
        else
        {
            inRightTab = false;
            arrowLeft.SetActive(false);
            arrowRight.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (bar.value > 0 && arrowLeft.activeInHierarchy == false && inRightTab)
        {
            arrowLeft.SetActive(true);
        }
        else if (bar.value <= 0 && arrowLeft.activeInHierarchy == true && inRightTab)
        {
            arrowLeft.SetActive(false);
        }

        if (bar.value < 1 && arrowRight.activeInHierarchy == false && inRightTab)
        {
            arrowRight.SetActive(true);
            
        }
        else if (bar.value >= 1 && arrowRight.activeInHierarchy == true && inRightTab)
        {
            arrowRight.SetActive(false);
        }

    }
}
