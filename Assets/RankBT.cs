using UnityEngine;
using System.Collections;

public class RankBT : MonoBehaviour {
    GameObject tempObject;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void clicked()
    {
        

         if(GLOBALS.s.TUTORIAL_OCCURING == false && GLOBALS.s.DIALOG_ALREADY_OPENED == false)
        {
                
            tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/DemonList/DemonList"));
            MenusController.s.moveMenu(MovementTypes.Left, tempObject, "DemonList", 0, 0);

        }
        else
        {
            if (GLOBALS.s.TUTORIAL_PHASE == 25)
                TutorialController.s.showRankList();
        }
        
    }
}
