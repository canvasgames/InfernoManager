using UnityEngine;
using System.Collections;

public class yes_bt_hell_diplomacy_2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void clickcc()
    {
        double CapacityTotal = BE.SceneTown.Elixir.ChangeDelta(0);
        BE.SceneTown.Elixir.ChangeDelta(-(CapacityTotal * 0.6f));

        MenusController.s.destroyHellDiplomacyYes2();
    }
}
