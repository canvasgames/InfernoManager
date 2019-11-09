using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class catastropheBT : MonoBehaviour {
    GameObject tempObject;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void clicked() {
        /* if (GLOBALS.s.TUTORIAL_OCCURING == false && GLOBALS.s.DIALOG_ALREADY_OPENED == false)
         {
             //MenusController.s.createCatastrophe(0);
             tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/CatastropheList/CatastropheList"));
             MenusController.s.moveMenu(MovementTypes.Left, tempObject, "CatastropheList", 0, 0);
         }*/

        if (GLOBALS.s.DIALOG_ALREADY_OPENED == false || GLOBALS.s.TUTORIAL_OCCURING == true && GLOBALS.s.TUTORIAL_PHASE == 100) { 

            tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/CatastropheList/CatastropheListv2"));
            MenusController.s.moveMenu(MovementTypes.Left, tempObject, "CatastropheList", 0, 0);

            if (!GLOBALS.s.TUT_CAT_ALREADY_OCURRED) {
                TutorialController.s.click_to_spin();
                

            }
        }


    }
}
