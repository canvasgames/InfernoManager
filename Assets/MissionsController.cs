using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MissionType {
    None = 0,
    Spank = 1,
    Build = 2,
    LevelUp = 3,
    CollectSouls = 4
}

public enum CollectSoulsType {
    MissionSpank = 0,
    MissionBuild = 1,
    Catastrophe = 2,
    MissionCollect = 3
}

public class Mission {
    public int targetQuantity = 0;
    public int curQuantity = 0;
    public MissionType type;
    public bool isComplete = false;

    public Mission( MissionType _type, int _targetQuantity) {
        targetQuantity = _targetQuantity;
        type = _type;
        curQuantity = 0;
    }
}

public class MissionsController : MonoBehaviour {
    public static MissionsController s;
    int PunisherBuildingsCont = 0;
    MissionOption[] MissionOptions;
    public List<Mission> allMissions = new List<Mission>();
    public float soulsCollected = 0;
    GameObject exclamationMissions;
    // Use this for initialization

    void Awake() {
        s = this;  //}

        // ADDING MISSIONS TEMPORALY
        allMissions.Add(new Mission(MissionType.Spank, 1));
        allMissions.Add(new Mission(MissionType.Build, 1));
        allMissions.Add(new Mission(MissionType.CollectSouls, 1000));
    }
    
	
	// Update is called once per frame
	void Update () {
        // GLOBALS.s.TUTORIAL_OCCURING = true ;
    }

    public void GetMissionInfoForList(MissionType type) { 
        if(type == MissionType.Spank)
        {

        }

        if (type == MissionType.LevelUp) {

        }
    }

    #region Open and Close
    public void OpenMissionDialog()
    {
        if(GLOBALS.s.DIALOG_ALREADY_OPENED == false)
        {
            Debug.Log(" OPENING MISSION DIALOG");
            GameObject tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/DialogMissions"));

            MenusController.s.moveMenu(MovementTypes.Down, tempObject, "DialogMission", 0, 0);

            tempObject.GetComponent<DialogMission>().CreateMissionsList(allMissions);

            //DialogMission a = (DialogMission)tempObject;
        }

    }

    public void CloseMissionDialog() {
        Debug.Log(" closing MISSION DIALOG");

        MenusController.s.destroyMenu("DialogMission", null);
    }

    #endregion

    public void ActMission(MissionType type)
    {
        CloseMissionDialog();

        if (type == MissionType.Spank) {
            SpankController.s.StartAnimation();
        }
        else if (type == MissionType.Build) {
            BE.SceneTown.instance.OnButtonShop();
        }
        else if (type == MissionType.CollectSouls) {
            //Vector3 pos = new Vector3(-10f, 0f, 13f);
			Vector3 pos = new Vector3(21f, 0f, 10f);
            BE.SceneTown.instance.move_camera_to_building(pos,0.5f,9);
        }

    }

    //Reward Mission Dialog
    public void RewardMisison(MissionType type) {
        Debug.Log("[MC] REWARD MISSION! TYPE: " + type);

        GameObject tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SmallScroll"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "SmallScroll", 0, 0, "MissionCollect");

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/Satan/SatanProud"));
        MenusController.s.moveMenu(MovementTypes.Left, tempObject, "Satan", 0, 0);
        tempObject.GetComponent<Animator>().Rebind();

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/Dialogs/MissionTitle"));
        MenusController.s.moveMenu(MovementTypes.Left, tempObject, "MissionTitle", 0, 0);

        GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/Buttons/CollectSoulsBt"));
        MenusController.s.moveMenu(MovementTypes.Right, temp, "Bt", 0, 0);


        if (type == MissionType.Spank) {
            temp.GetComponent<CollectSoulsBt>().myBtType = CollectSoulsType.MissionSpank;
        }

        if (type == MissionType.Build) {
            temp.GetComponent<CollectSoulsBt>().myBtType = CollectSoulsType.MissionBuild;
            Debug.Log("reward type build!");
        }

        if (type == MissionType.CollectSouls) {
            temp.GetComponent<CollectSoulsBt>().myBtType = CollectSoulsType.MissionCollect;
        }

        
    }

    public void OnSoulsCollected(CollectSoulsType btType) {
        MenusController.s.destroyMenu("Satan", null);
        MenusController.s.destroyMenu("MissionTitle", null);
        MenusController.s.destroyMenu("SmallScroll", null);
        MenusController.s.destroyMenu("Bt", null);

        Debug.Log("[MC] on souls collect, BtType: " + btType);

        if (btType == CollectSoulsType.MissionSpank)
            allMissions[0].isComplete = true;
        else if (btType == CollectSoulsType.MissionBuild)
            allMissions[1].isComplete = true;
        else if (btType == CollectSoulsType.MissionCollect) {
            allMissions[2].isComplete = true;      
        }

        checkIfAllMissionsAreCompleted();
    }

    public void OnBuildingComplete(int buildingType) {
        Debug.Log("[MC] On Building Complete! Tutorial ocurring: " + GLOBALS.s.TUTORIAL_OCCURING);
        if (!GLOBALS.s.TUTORIAL_OCCURING && buildingType >= 11 && buildingType <= 15) { // if the building is a punisher type
            PunisherBuildingsCont++;
            if(PunisherBuildingsCont == 2 && allMissions[1].isComplete == false) {
                RewardMisison(MissionType.Build);
            }
        }
    }

    public void OnSoulsCollected(float quant) {
        soulsCollected += quant;
        allMissions[2].curQuantity += (int)quant;
        if (soulsCollected >= allMissions[2].targetQuantity && allMissions[2].isComplete == false)
            RewardMisison(MissionType.CollectSouls);
    }

    void checkIfAllMissionsAreCompleted()
    {
        Debug.Log("[MC] Missions collect compelted " + allMissions[1].isComplete);
        if(allMissions[0].isComplete == true && allMissions[1].isComplete == true && allMissions[2].isComplete == true)
        {
            exclamationMissions = GameObject.Find("MissionExcl");
            exclamationMissions.SetActive(false);
        }
            
    }
}
