using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DialogMission : MonoBehaviour {

    public GameObject content;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreateMissionsList(List<Mission> allMissions) {
        //MissionOption item;
        GameObject item;
        foreach(Mission missionItem in MissionsController.s.allMissions) {
            //item = (MissionOption)Instantiate(Resources.Load("Prefabs/OptionMission"));
            item = (GameObject)Instantiate(Resources.Load("Prefabs/OptionMission"));

            item.transform.SetParent(MenusController.s.bigDaddy, false);
            item.transform.SetParent(content.transform,false);

            Debug.Log("[MCD] Init item! type: " + missionItem.type + " IS COMPLETE: " + missionItem.isComplete);

            item.GetComponent<MissionOption>().Init(missionItem);

            //item.Init(missionItem);
        }
    }
}
