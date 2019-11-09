using UnityEngine;
using System.Collections;

public class hell_diplomacy_bt : MonoBehaviour {

    GameObject tempObject;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void clicked()
    {
        //TutorialController.s.catExplanation();
        if (GLOBALS.s.DIALOG_ALREADY_OPENED == false)
        {

           MenusController.s.createHellDiplomacy();
        }


    }
}
