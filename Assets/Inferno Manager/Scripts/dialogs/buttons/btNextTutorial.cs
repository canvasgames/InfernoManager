using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class btNextTutorial : ButtonCap
{

    // Use this for initialization
    void Start()
    {
        Debug.Log("ARROW CREATED! TIME: " + Time.time);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == true && GLOBALS.s.TUTORIAL_PHASE != 13)
            {
                ActBT();
            }
            else
            {
                if(GLOBALS.s.TUTORIAL_PHASE != 13)
                    ActBT();
            }
        }

    }

    public override void ActBT()
    {

        base.ActBT();
        if(GLOBALS.s.TUTORIAL_PHASE == 1)
        {
            Debug.Log("TUTO 101 CLICKED " + Time.time);
            MenusController.s.destroyMenu("ArowNext", null);
            TutorialController.s.tutorial1Clicked101();
        }
        else if (GLOBALS.s.TUTORIAL_PHASE == 101)
        {
            Debug.Log("TUTO SDASDASD CLICKED" + Time.time);
            MenusController.s.destroyMenu("ArowNext", null);
            TutorialController.s.tutorial1Clicked();
        }
        else if (GLOBALS.s.TUTORIAL_PHASE == 2)
        {
            MenusController.s.destroyMenu("ArowNext", null);
            TutorialController.s.tutorial1Phase2Clicked();

        }
        else if (GLOBALS.s.TUTORIAL_PHASE == 3)
        {
            MenusController.s.destroyMenu("ArowNext", null);
            TutorialController.s.tutorial1Phase3Clicked();

        }
       else if (GLOBALS.s.TUTORIAL_PHASE == 4)
        {
            MenusController.s.destroyMenu("ArowNext", null);
            TutorialController.s.tutorial1Phase4Clicked();

        }
        else if (GLOBALS.s.TUTORIAL_PHASE == 6)
        {
            MenusController.s.destroyMenu("ArowNext", null);
            TutorialController.s.indicateBuildBT();

        }
        else if (GLOBALS.s.TUTORIAL_PHASE == 8)
        {

            MenusController.s.destroyMenu("ArowNext", null);
            TutorialController.s.destroySelectPunisher();

        }


        else if (GLOBALS.s.TUTORIAL_PHASE == 10)
        {
            MenusController.s.destroyMenu("ArowNext", null);
            TutorialController.s.collectSoulsAgain();

        }

        // After Back to serious

        else if(GLOBALS.s.TUTORIAL_PHASE == 11)
        {
            TutorialController.s.blablaQuemEhVcNaFilaDoPao();
        }
        else if (GLOBALS.s.TUTORIAL_PHASE == 12)
        {

            MenusController.s.destroyMenu("ArowNext", null);
            TutorialController.s.clickRankHUD();

        }
        else if (GLOBALS.s.TUTORIAL_PHASE == 13)
        {

            MenusController.s.destroyMenu("ArowNext", null);
            TutorialController.s.start_chicken_tutorial();
           

        }
        else if (GLOBALS.s.TUTORIAL_PHASE == -14)
        {

            MenusController.s.destroyMenu("ArowNext", null);
            //TutorialController.s.backPhase13();
            TutorialController.s.start_chicken_tutorial();

        }

        else if (GLOBALS.s.TUTORIAL_PHASE == -2)
        {
            MenusController.s.destroyMenu("ArowNext", null);
            TutorialController.s.pressBuildBtConstructImp();

        }

        else if (GLOBALS.s.TUTORIAL_PHASE == 18)
        {
            MenusController.s.destroyMenu("ArowNext", null);
            TutorialController.s.ILlBeThereForYou();

        }
        else if (GLOBALS.s.TUTORIAL_PHASE == 19)
        {
            MenusController.s.destroyMenu("ArowNext", null);
            TutorialController.s.realEndTutorial();

        }
    }


}