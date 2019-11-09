using UnityEngine;
using System.Collections;

public class callCatastrophe : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ActBT()
    {
        MenusController.s.createCatastrophe(0);
        MenusController.s.destroyMenu ("CatastropheList", null);
    }
}
