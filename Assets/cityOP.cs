using UnityEngine;
using System.Collections;

public class cityOP : MonoBehaviour {

    public void click()
    {
        if(GLOBALS.s.TUTORIAL_OCCURING == false)
        {
            MenusController.s.destroyMenu("CityOP", null);
        }
    }
}
