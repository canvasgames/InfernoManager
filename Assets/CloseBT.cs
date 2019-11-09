using UnityEngine;
using System.Collections;


public class CloseBT : MonoBehaviour {

    public GameObject myFather;
    public bool waitToClose = false;
    public bool levelUPBT = false;


	// Use this for initialization
	void Start () {
	    if(waitToClose == true)
        {
            Invoke("canCloseNow", 2f);
        }
	}
	
    void canCloseNow()
    {
        waitToClose = false;
    }
	// Update is called once per frame
	void Update () {
	    
	}

    public void clicked()
    {
        if(waitToClose == false && GLOBALS.s.TUTORIAL_PHASE !=102 && GLOBALS.s.TUTORIAL_PHASE != 103 && GLOBALS.s.TUTORIAL_PHASE != 104)
        {
            if (GLOBALS.s.TUTORIAL_PHASE == 11)
            {
                TutorialController.s.blablaQuemEhVcNaFilaDoPao();
            }
            destroyMenu();

        }

    }

    public void destroyMenu()
    {

        MenusController.s.destroyMenu("", myFather);
        if (levelUPBT == true && GLOBALS.s.USER_RANK == 3)
        {
            //MenusController.s.createCatastrophe(1.5f);
            TutorialController.s.catExplanation();
        }

    }
}
