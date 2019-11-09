using UnityEngine;
using System.Collections;
using DG.Tweening;

public class demonCreateAvatar : MonoBehaviour {
    
    public GameObject a, b, c, d, e, f, g, h, i, j, k, l, m;
    GameObject temp;
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setMyAvatar(int line, int collum)
    {
        if (line == 1)
        {
            temp = (GameObject)Instantiate(a, new Vector3(0, 0, 0), Quaternion.identity);
        }
        else if(line == 2)
        {
            if (collum == 1)
            {
                temp = (GameObject)Instantiate(b, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else if (collum == 2)
            {
                temp = (GameObject)Instantiate(c, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                temp = (GameObject)Instantiate(d, new Vector3(0, 0, 0), Quaternion.identity);
            }
            
        }
        else if (line == 3)
        {
            if (collum == 1)
            {
                temp = (GameObject)Instantiate(e, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else if (collum == 2)
            {
                temp = (GameObject)Instantiate(f, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                temp = (GameObject)Instantiate(g, new Vector3(0, 0, 0), Quaternion.identity);
            }
        }
        else if (line == 4)
        {
            if (collum == 1)
            {
                temp = (GameObject)Instantiate(h, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else if (collum == 2)
            {
                temp = (GameObject)Instantiate(i, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                temp = (GameObject)Instantiate(j, new Vector3(0, 0, 0), Quaternion.identity);
            }
        }
        else
        {
            
            if (collum == 1)
            {
                temp = (GameObject)Instantiate(k, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else if (collum == 2)
            {
                temp = (GameObject)Instantiate(l, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                temp = (GameObject)Instantiate(m, new Vector3(0, 0, 0), Quaternion.identity);
            }
        }

        temp.transform.SetParent(gameObject.transform, false);
        temp.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    }

    public void click()
    {
        if (GLOBALS.s.TUTORIAL_OCCURING == false)
        {
            GameObject tempObject;
            tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/CityOP"));
            MenusController.s.addToGUIAndRepositeObject(tempObject, "CityOP");
        }
        else if (GLOBALS.s.TUTORIAL_PHASE == -13)
        {
            GameObject tempObject;
            tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/CityOP"));
            MenusController.s.addToGUIAndRepositeObject(tempObject, "CityOP");
            MenusController.s.destroyMenu("SmallScroll", null);

            Invoke("invokeTut", 1f);
        }
    }

    void invokeTut()
    {
        TutorialController.s.niceCity();
    }
}
