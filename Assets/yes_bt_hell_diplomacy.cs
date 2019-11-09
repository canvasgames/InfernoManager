using UnityEngine;
using System.Collections;

public class yes_bt_hell_diplomacy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void click()
    {
        double CapacityTotal = BE.SceneTown.Elixir.ChangeDelta(0);
        BE.SceneTown.Elixir.ChangeDelta(-(CapacityTotal*0.5f));
        Debug.Log(CapacityTotal);

        MenusController.s.destroyHellDiplomacyYes();
    }
}
