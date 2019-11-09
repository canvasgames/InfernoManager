using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MissionOption : ButtonCap
{
    public MissionType missionType;

    public GameObject check;
    public GameObject targetQuantity;
    public GameObject text;


    /*public MissionOption (Mission mission) {
        //targetQuantity = mission.
        missionType = mission.type;
        Init();
    }*/

    public override void ActBT()
    {
        MissionsController.s.ActMission(missionType);
    }

    public void Init(Mission mission) {
        missionType = mission.type;

        if (missionType == MissionType.Spank) {
            text.GetComponent<Text>().text = "Spank Hitler's Ass";
            targetQuantity.GetComponent<Text>().text = "0/1";
        }
        else if (missionType == MissionType.Build) {
            text.GetComponent<Text>().text = "Build 2 Punisher Buildings Type";
            targetQuantity.GetComponent<Text>().text = "0/2";
        }

        else if (missionType == MissionType.CollectSouls) {
            
            text.GetComponent<Text>().text = "Collect "+mission.targetQuantity +" Souls from Hell's Gate";
            int a;
            if (mission.curQuantity >= mission.targetQuantity) a = mission.targetQuantity;
            else a = mission.curQuantity;
            targetQuantity.GetComponent<Text>().text = a+"/"+mission.targetQuantity;
        }

        Debug.Log("[MCO] tYPE: " + missionType + " is complete: " + mission.isComplete);
        if (mission.isComplete) {
            
            targetQuantity.SetActive(false);
            check.SetActive(true);
        }
    }

}

