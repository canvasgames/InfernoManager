
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class CreateCatastropheScrollView : MonoBehaviour
{
    float initialY, xSpace, ySpace, xMiddle, x_pos, y_pos;

    public GameObject placeholder;
    GameObject myPlaceholder;
    // Use this for initialization
    void Start()
    {

        //xMiddle = 140;
        //initialY = GetComponent<RectTransform>().rect.height / 2 - 1160; //150 spacing, 650 half of contect UI object
        //xSpace = placeholder.GetComponent<RectTransform>().rect.width ;
        xSpace = 20;
        initialY = 696 - 840;
        ySpace = placeholder.GetComponent<RectTransform>().rect.height;

        createLine(initialY - 40 , xSpace, 1);
        createLine(initialY - (ySpace + 40), xSpace, 2);
        createLine(initialY - (ySpace * 2 +40), xSpace, 3);
       createLine(initialY - (ySpace * 3 + 40), xSpace, 4);
        createLine(initialY - (ySpace * 4 + 40), xSpace, 5);
        //transform.DOLocalMoveY(GetComponent<RectTransform>().rect.height, 3f);
    }

    // Update is called once per frame
    void Update()
    {

    }


    void createLine(float yPosition, float xSpacing, int line)
    {

        createObject(line, 0, xSpacing, yPosition);

    }

    void createObject(int line, int collumn, float xPosition, float yPosition)
    {

            myPlaceholder = (GameObject)Instantiate(placeholder, new Vector3(0, 0, 0), Quaternion.identity);
            myPlaceholder.transform.SetParent(gameObject.transform, false);

        myPlaceholder.GetComponent<RectTransform>().localPosition = new Vector3(xPosition, yPosition, 0);

            myPlaceholder.GetComponent<CatastropheItem>().initCatastrophe(line);
    }


}
