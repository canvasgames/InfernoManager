using UnityEngine;
using System.Collections;

public class cityOP : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void click()
    {
        if(GLOBALS.s.TUTORIAL_OCCURING == false)
        {
            MenusController.s.destroyMenu("CityOP", null);
        }
    }
}
