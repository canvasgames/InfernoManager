using UnityEngine;
using System.Collections;

public class GLOBALS : MonoBehaviour {
    public static GLOBALS s;

    //GAME STATUS
    [HideInInspector] public bool GAME_RUNNING = false;

    [HideInInspector] public int TUTORIAL_PHASE = 0;
    [HideInInspector] public bool TUTORIAL_OCCURING = false;
    [HideInInspector] public bool LOCK_CAMERA_TUTORIAL = false;
    [HideInInspector] public bool LOCK_CLICK_TUTORIAL = false;
    [HideInInspector]
    public bool TUT_CAT_ALREADY_OCURRED = false;

[HideInInspector] public bool DIALOG_ALREADY_OPENED = false;
    [HideInInspector] public bool SPANKING_OCURRING = false;

    [HideInInspector] public int USER_RANK = 1;
    [HideInInspector] public int BUILDING_MAX_LEVEL = 30;
    [HideInInspector] public int BUILDING_N_TYPES = 30;
    [HideInInspector] public string PUNISHER_COUNT_EVOLUTION = "1,2,3,3,4,4,5,5,6,6,7,7,7,8,8,8,9,9,9,10,10,10,11,11,11,12,12,12,13,13";
    [HideInInspector]
    public float ELIXIR_RESEARCH_EXTRA_PERCENTAGE = 1;

    public int nTimesCollectedSouls = 0;

    public bool already_spinned = false;

   


    void Awake()
    {
        s = this;
        DontDestroyOnLoad(transform.gameObject);
        USER_RANK = 1;
    }

    public string GetRankName(int level) {
        string txt = "";
        if (level == 2)
            txt = "Lord";
        if (level== 2)
            txt = "Baron";
        if (level== 3)
            txt = "Count";
        if (level== 4)
            txt = "Viscount";
        if (level== 5)
            txt = "Marquess";
        if (level== 6)
            txt = "Prince";

        return txt;
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
       // Debug.Log(ELIXIR_RESEARCH_EXTRA_PERCENTAGE);
	}

    void OnDestroy() {
        Debug.Log("GLOBALS ARE BEING DESTROYED!!!! ");
    }
}
