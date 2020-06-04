using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public static TutorialController s;

    public GameObject missionsBT;

    public GameObject curSatan;

    public GameObject txtMaxSouls, txtMaxSouls1, txtMaxSouls2;

    public GameObject catastrophesBt;

    public GameObject intro;

    public DemonListScrollbar[] DemonListScroll;

    GameObject tempObject;
    public GameObject HUD;

    public GameObject TutFireBlocker;

    BE.Building[] buildings;
    DialogsTexts[] fscreen;

    int firstGame;
    bool atIntro = true;

    float tutorial1Timer;

    void Awake()
    {
        s = this;
    }

    #region Start And First Tutorial Flag
    void Start()
    {

         firstGame = PlayerPrefs.GetInt("firstGame");
		
        if (QA.s.NoTutorial == true) firstGame = 2;

 
        if (firstGame == 0)
        {
            missionsBT.transform.localScale = new Vector3 (0, 0, 0);
            HUD.SetActive(false);
            GLOBALS.s.LOCK_CAMERA_TUTORIAL = true;
            GLOBALS.s.LOCK_CLICK_TUTORIAL = true;
            GLOBALS.s.TUTORIAL_OCCURING     = true;

            if(QA.s.NoSatanEntering == false)
            {
                GLOBALS.s.TUTORIAL_PHASE =  -999;
            }
            else
            {
                // clickRankHUD();
            }
			
            //startTutorial();
            //tutorial1Timer = 2f;
            // 

        }
        else
        {
            BE.SceneTown.instance.createTownHownTutorial();
            createGate();
            BE.SceneTown.instance.CapacityCheck();
            BE.SceneTown.Gold.ChangeDelta((double)300);
        }
        //
        #endregion
        //clickRankHUD();
    }
    void OnDestroy() {
        Debug.Log("TUTORIAL CONTROLLER IS BEING DESTROYED!!!! ");
    }

    #region Update and Timers
    // Update is called once per frame
    void Update() {

        if (atIntro == true && Input.GetMouseButtonDown(0) ){
            atIntro = false;
            if (QA.s.NoSatanEntering == false && QA.s.NoTutorial == false)
                SatanController.s.StartSatanIntro(1.4f);
            else if (QA.s.NoSatanEntering == true && QA.s.NoTutorial == false)
                StartTutorial();
        
            intro.SetActive(false);
        }
    }
    #endregion

    #region Tutorial Phase 1 Welcome

    public void StartTutorial()
    {
        Invoke("tutorial1", 1f);
    }
    //Hi i'm Satan msg
    public void tutorial1()
    {
        //createBuilding();
    
        GLOBALS.s.TUTORIAL_PHASE = 1;
        Debug.Log("TUTORIAL PHASE 1");
        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/Welcome"));
        MenusController.s.moveMenu(MovementTypes.Up, tempObject, "Welcome", 0, 0);
        curSatan = SatanController.s.SatanIntro2;
        curSatan.SetActive(true);
        Invoke("tutorial1EnterOtherStuff", 0.4f);
       // */
    }

    public void tutorial1EnterOtherStuff()
    {
        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SmallScroll"));
        MenusController.s.moveMenu(MovementTypes.Left, tempObject, "SmallScroll", 0, 0);

        Invoke("createNextButton", 2f);
    }

    public void tutorial1Clicked101()
    {
        Debug.Log("TUTORIAL PHASE 101");
        GLOBALS.s.TUTORIAL_PHASE = 101;
        MenusController.s.goOutDestroy("Welcome", null,"up");

        curSatan.SetActive(false);
        curSatan = SatanController.s.SatanExplaning;
        curSatan.SetActive(true);

        fscreen = GameObject.FindObjectsOfType(typeof(DialogsTexts)) as DialogsTexts[];
        fscreen[0].closeAndReopen();
    
        Invoke("createNextButton", 2);
    }
    #endregion

    #region Tutorial Phase 2 Create Town Hall
    //Consturct the Town Hall
    public void tutorial1Clicked()
    {
        GLOBALS.s.TUTORIAL_PHASE = 2;;

        curSatan.GetComponentInChildren<Animator>().Rebind();

        fscreen = GameObject.FindObjectsOfType(typeof(DialogsTexts)) as DialogsTexts[];
        fscreen[0].closeAndReopen();
        
        Invoke("createBuilding", 0.3f);
        Invoke("createNextButton", 2);
    }

    void createBuilding()
    {
        BE.SceneTown.instance.createTownHownTutorial();
    }
    #endregion

    #region Tutorial Phase 3 Create Hell's Gate
    //Create Hells Gate
    public void tutorial1Phase2Clicked()
    {
        GLOBALS.s.TUTORIAL_PHASE = 3;
        curSatan.GetComponentInChildren<Animator>().Rebind();

        BE.SceneTown.instance.CapacityCheck();
        BE.SceneTown.Gold.ChangeDelta((double)300);
        fscreen = GameObject.FindObjectsOfType(typeof(DialogsTexts)) as DialogsTexts[];
        fscreen[0].closeAndReopen();

        Invoke("createGate", 0.3f);
        Invoke("createNextButton", 2);
    }

    void createGate()
    {
        BE.SceneTown.instance.createHellGateTutorial();
    }
    #endregion

    #region Tutorial Phase 4 Collect
    //Click to collect
    public void tutorial1Phase3Clicked()
    {
        Debug.Log("Tutorial phase 4: Tap to collect souls");
        GLOBALS.s.TUTORIAL_PHASE = 4;
        curSatan.GetComponentInChildren<Animator>().Rebind();

        GLOBALS.s.LOCK_CLICK_TUTORIAL = false;
        fscreen = GameObject.FindObjectsOfType(typeof(DialogsTexts)) as DialogsTexts[];
        fscreen[0].closeAndReopen();
        HUD.SetActive(true);
        
        buildings = GameObject.FindObjectsOfType(typeof(BE.Building)) as BE.Building[];
        foreach (BE.Building element in buildings)
        {
            element.activateHandTutorialUI(4, true);
        }
    }
    #endregion

    #region Tutorial Phase 5 Question
    //Full of souls question, what to do?
    public void tutorial1Phase4Clicked()
    {
        Debug.Log("Tut phase 5: Display the question");

        GLOBALS.s.TUTORIAL_PHASE = 5;
        GLOBALS.s.LOCK_CLICK_TUTORIAL = true;

        fscreen = GameObject.FindObjectsOfType(typeof(DialogsTexts)) as DialogsTexts[];
        fscreen[0].closeAndDestroy();
        
        buildings = GameObject.FindObjectsOfType(typeof(BE.Building)) as BE.Building[];
        foreach (BE.Building element in buildings)
        {
            element.unactivateHandTutorialUI(4);
        }

        Invoke("tutorial1Phase4ClickedPart2", 2);

    }

    void tutorial1Phase4ClickedPart2()
    {
        curSatan.SetActive(false);
        curSatan = SatanController.s.SatanBragging;
        curSatan.SetActive(true);

        HUD.SetActive(false);
        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/BigScrollQuestion"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "BigScrollQuestion", 0, 0);

    }
    #endregion

    #region Tutorial Phase 6 Are you Stupid?
    //Chicachicabum, começa e não para. Avisa todo mundo que meu nome é Sara
    public void questionAnswered()
    {
        Debug.Log("[TUT] 6 - BLA BLA ");
        GLOBALS.s.TUTORIAL_PHASE = 6;

        curSatan.SetActive(false);
        curSatan = SatanController.s.SatanComplaining;
        curSatan.SetActive(true);

        fscreen = GameObject.FindObjectsOfType(typeof(DialogsTexts)) as DialogsTexts[];
        fscreen[0].closeAndDestroy();

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SmallScroll"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "SmallScroll", 0, 0, "", false, false, 0.5f);
        Invoke("createNextButton", 2);

    }
    #endregion

    #region Tutorial Phase 7 Indicate The Build Bt
    //Indicate the build Bt
    public void indicateBuildBT()
    {
        Debug.Log("[TUT] 7 TOUCH THE BUILD BUTTON | tutorial occuring: " + GLOBALS.s.TUTORIAL_OCCURING);

        GLOBALS.s.TUTORIAL_PHASE = 7;

        curSatan.SetActive(false);
        curSatan = SatanController.s.SatanChallenging;
        curSatan.SetActive(true);

        fscreen = GameObject.FindObjectsOfType(typeof(DialogsTexts)) as DialogsTexts[];
        fscreen[0].closeAndReopen();
        MenusController.s.repositeMenu("SmallScroll", null, 252, 120);
        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/DownArrow"));
        MenusController.s.moveMenu(MovementTypes.Right,tempObject, "DownArrow", 0, 0);
        HUD.SetActive(true);
         
    }
    #endregion

    #region Tutorial Phase 8 Choose a Building
    //Clicked build bt
    public void clickedBuildBt()
    {
        Debug.Log("[TUT] 8 SELECT A BUILDING");

        BE.SceneTown.instance.move_camera_to_building(new Vector3(13, 3, 1),0.5f,10);
        curSatan.SetActive(false);

        GLOBALS.s.LOCK_CLICK_TUTORIAL = false;
        GLOBALS.s.LOCK_CAMERA_TUTORIAL = false;
        GLOBALS.s.TUTORIAL_PHASE = 8;
        fscreen = GameObject.FindObjectsOfType(typeof(DialogsTexts)) as DialogsTexts[];
        fscreen[0].closeAndReopen("m");
        MenusController.s.destroyMenu("DownArrow", null);
        MenusController.s.repositeMenu("SmallScroll", null, 0f, 155f, 0.9F);
        //Invoke("createNextButton", 2);

    }
    #endregion

    #region Tutorial Phase 9 Place The Building
    //Place it building msg
    public void destroySelectPunisher()
    {
        GLOBALS.s.TUTORIAL_PHASE = 9;
        fscreen = GameObject.FindObjectsOfType(typeof(DialogsTexts)) as DialogsTexts[];
        fscreen[0].closeAndReopen("m");

    }

    //Destroy small scroll
    public void destructSmallScroll()
    {
        MenusController.s.destroyMenu("SmallScroll", null);
    }
    #endregion

    #region Tutorial Phase 10 Punisher Capacity Explanation (blablabla)
    //Indicate souls HUD
    public void punisherCapacityExplanation()
    {
        GLOBALS.s.LOCK_CLICK_TUTORIAL = true;
        GLOBALS.s.LOCK_CAMERA_TUTORIAL = true;

        GLOBALS.s.TUTORIAL_PHASE = 10;

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SmallScroll"));
        MenusController.s.moveMenu(MovementTypes.Left,tempObject, "SmallScroll", -292f, -113f);

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/GreenCircle"));
        MenusController.s.moveMenu(MovementTypes.Left, tempObject, "GreenCircle", -382.8f, 289);
        tempObject.GetComponent<GreenCircle>().pulse();

        Invoke("createNextButton", 2);

    }
    #endregion

    #region Tutorial Phase 11 Collect Souls Again
    //Indicate to collect souls again
    public void collectSoulsAgain()
    {
        GLOBALS.s.TUTORIAL_PHASE = 11;
        GLOBALS.s.LOCK_CLICK_TUTORIAL = false;
        txtMaxSouls.GetComponent<textMaxSouls>().stop();
        txtMaxSouls1.GetComponent<textMaxSouls>().stop();
        txtMaxSouls2.GetComponent<textMaxSouls>().stop();
        MenusController.s.destroyMenu("3DArrow", null);
        MenusController.s.destroyMenu("GreenCircle", null);

        curSatan = SatanController.s.SatanExplaning;
        curSatan.SetActive(true);

        fscreen = GameObject.FindObjectsOfType(typeof(DialogsTexts)) as DialogsTexts[];
        fscreen[0].closeAndReopen();
        
        MenusController.s.repositeMenu("SmallScroll", null, 256f, -303f);

        buildings = GameObject.FindObjectsOfType(typeof(BE.Building)) as BE.Building[];
        foreach (BE.Building element in buildings)
        {
            element.activateHandTutorialUI(4, true);

        }
    }
    #endregion

    #region Tutorial Phase 11 (11 again, because of reasons) Souls Collected Level UP
    //Souls Collected
    public void soulReallyCollected()
    {
        GLOBALS.s.TUTORIAL_PHASE = 11;
        MenusController.s.destroyMenu("SmallScroll", null);

        GLOBALS.s.LOCK_CLICK_TUTORIAL = true;
       
        buildings = GameObject.FindObjectsOfType(typeof(BE.Building)) as BE.Building[];
        foreach (BE.Building element in buildings)
        {
            element.unactivateHandTutorialUI(4);
        }

        Invoke("StartLevelUpAnimation", 2.5f);
    }

    void StartLevelUpAnimation()
    {
        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/LevelUp"));
        MenusController.s.apearAlphaCanvasGroup(tempObject, "LevelUp");
        BE.BEAudioManager.SoundPlay(10);

        Invoke("createNextButton", 2);
    }

    #endregion

    #region Tutorial Phase 12 Satan Talking Shit About You
    //Satan talking shit about you (voce é uma merda garoto)
    public void blablaQuemEhVcNaFilaDoPao()
    {
        MenusController.s.destroyMenu("LevelUp", null);
        MenusController.s.destroyMenu("ArowNext", null);
        GLOBALS.s.TUTORIAL_PHASE = 12;

        curSatan.SetActive(false);
        curSatan = SatanController.s.SatanComplaining;
        curSatan.SetActive(true);

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/BigScroll"));
        MenusController.s.moveMenu(MovementTypes.Left, tempObject, "BigScroll", 0, 0);

        Invoke("createNextButton", 2);
    }
    #endregion

    #region Tutorial Phase 25 Click Rank HUD
    public void clickRankHUD()
    {
        HUD.SetActive(true);
        GLOBALS.s.TUTORIAL_PHASE = 25;
        MenusController.s.destroyMenu("BigScroll", null);

        curSatan.SetActive(false);
        curSatan = SatanController.s.SatanChallenging;
        curSatan.SetActive(true);

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SmallScroll"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "SmallScroll", -223f, 126f);

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SatanHand"));
        MenusController.s.addToGUIAndRepositeObject(tempObject, "SatanHand");

        SatanHand script;
        script = (SatanHand)tempObject.GetComponent(typeof(SatanHand));
        script.initRankTutorial();
    }
    #endregion

    #region Tutorial Phase 13 Show Rank List
    public void showRankList()
    {
        GLOBALS.s.TUTORIAL_PHASE = 13;
        curSatan.SetActive(false);
        MenusController.s.destroyMenu("SmallScroll", null);
        MenusController.s.destroyMenu("SatanHand", null);

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/DemonList/DemonList"));
        MenusController.s.moveMenu(MovementTypes.Left, tempObject, "DemonList", 0, 0);

        Invoke("autoMoveList", 2);
    }


    public void autoMoveList()
    {
        CreateDemonsScrollView[] list;
        list = GameObject.FindObjectsOfType(typeof(CreateDemonsScrollView)) as CreateDemonsScrollView[];
        list[0].moveList();
		Invoke("MovePlayerAvatar", 5);

       // DemonListScroll = GameObject.FindObjectsOfType(typeof(DemonListScrollbar)) as DemonListScrollbar[];
       // DemonListScroll[0].gameObject.SetActive(false);
    }

    #endregion

	void MovePlayerAvatar(){
		HellHierarchyMenu.s.advance_player ();
	}

    public void clickDemonLordToOpenCity()
    {
        GLOBALS.s.TUTORIAL_PHASE = -13;
        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SmallScroll"));
        MenusController.s.moveMenu(MovementTypes.Left, tempObject, "SmallScroll", -22f, 112);

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SatanHandCatBT"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "SatanHandCatBT", 368, 35f);
    }

    public void niceCity()
    {
        GLOBALS.s.TUTORIAL_PHASE = -14;
        MenusController.s.destroyMenu("SatanHandCatBT",null);

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SmallScroll"));
        MenusController.s.moveMenu(MovementTypes.Left, tempObject, "SmallScroll", -403, -328);
        //MenusController.s.repositeMenu("SmallScroll", null,-403, -328,0.8f);
        tempObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        Invoke("createNextButton", 2);
    }

    public void backPhase13()
    {
        GLOBALS.s.TUTORIAL_PHASE = 13;
        MenusController.s.destroyMenu("SmallScroll", null);
        MenusController.s.destroyMenu("CityOP", null);

        Invoke("createNextButton", 2);
    }

    #region Chicken 

    public void start_chicken_tutorial()
    {
        GLOBALS.s.TUTORIAL_PHASE = -1;
        GLOBALS.s.LOCK_CAMERA_TUTORIAL = true;

        MenusController.s.destroyMenu("SmallScroll", null);
        MenusController.s.destroyMenu("CityOP", null);
        MenusController.s.destroyMenu("DemonList", null);
        MenusController.s.destroyMenu("SatanHand", null);

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SmallScroll"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "SmallScroll", -101f, 131);
        fscreen = GameObject.FindObjectsOfType(typeof(DialogsTexts)) as DialogsTexts[];
        fscreen[0].closeAndReopen("m");

        ChickenController.s.start_animation();
    }

    public void after_chicken_kicked()
    {
        Debug.Log("[TUT] -2 AFTER CHICKEN KICKED");
        GLOBALS.s.TUTORIAL_PHASE = -2;

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SmallScroll4Lines"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "SmallScroll4Lines", 0, 0);

        curSatan.SetActive(false);
        curSatan = SatanController.s.SatanChallenging;
        curSatan.SetActive(true);

        Invoke("createNextButton", 2);
    }

    #endregion

    #region Tutorial Phase 14 Indicate Build Bt
    //Construct imp pit msg, indicate to press build
    public void pressBuildBtConstructImp()
    {
        MenusController.s.destroyMenu("SatanHand", null);
        MenusController.s.destroyMenu("SmallScroll4Lines", null);

        GLOBALS.s.TUTORIAL_PHASE = 14;
        
        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SmallScroll"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "SmallScroll", 252, 120);
        fscreen = GameObject.FindObjectsOfType(typeof(DialogsTexts)) as DialogsTexts[];
        fscreen[0].closeAndReopen();

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/DownArrow"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "DownArrow", 0, 0);

        curSatan.SetActive(false);
        curSatan = SatanController.s.SatanExplaning;
        curSatan.SetActive(true);
    }
    #endregion

    #region Tutorial Phase 21 Indicate Tab of Fire Mine
    //Indicate tab of fire mine
    public void indicateTabFireMine()
    {
        BE.SceneTown.instance.move_camera_to_building(new Vector3(6.5f, 0, 8.5f),0.5f,14f);
        curSatan.SetActive(false);

        MenusController.s.repositeMenu("SmallScroll", null, 0f, 155f, 0.9F);
        MenusController.s.repositeMenu("DownArrow", null, -190f, -215f);

        fscreen = GameObject.FindObjectsOfType(typeof(DialogsTexts)) as DialogsTexts[];
        fscreen[0].closeAndReopen();

        GLOBALS.s.TUTORIAL_PHASE = 21;
    }
    #endregion

    #region Tutorial Phase 15 Choose Fire Mine
    //Clicked tab
    public void pressBuildImpCasePressed()
    {
        GLOBALS.s.TUTORIAL_PHASE = 15;
        TutFireBlocker.SetActive(true);
        fscreen = GameObject.FindObjectsOfType(typeof(DialogsTexts)) as DialogsTexts[];
        fscreen[0].closeAndReopen();
        MenusController.s.destroyMenu("DownArrow", null);
    }
    #endregion

    #region Tutorial Phase 16 Place The Building
    //Imp pit pressed, place it msg
    public void impClicked()
    {
        TutFireBlocker.SetActive(false);
        GLOBALS.s.LOCK_CAMERA_TUTORIAL = false;
        GLOBALS.s.LOCK_CLICK_TUTORIAL = false;
        GLOBALS.s.TUTORIAL_PHASE = 16;
        fscreen = GameObject.FindObjectsOfType(typeof(DialogsTexts)) as DialogsTexts[];
        fscreen[0].closeAndReopen();
    }
    #endregion

    #region Tutorial Phase 17 Collect Fire
    //Collect fire
    public void CollectFirePhase()
    {
        GLOBALS.s.LOCK_CAMERA_TUTORIAL = true;
        GLOBALS.s.TUTORIAL_PHASE = 17;
        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SmallScroll"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "SmallScroll", -18f, -302f);
        BE.SceneTown.instance.move_camera_to_building(new Vector3(1.5f, 0, 3.5f),0.5f,14);

        buildings = FindObjectsOfType(typeof(BE.Building)) as BE.Building[];
        foreach (BE.Building element in buildings)
        {
            element.activateHandTutorialUI(3);
        }
    }
    #endregion

    #region Tutorial Phase 18 Satan Goodbye
    //That's it, end of tutorial
    public void endOfTutorial()
    {
        GLOBALS.s.LOCK_CAMERA_TUTORIAL = true;
        GLOBALS.s.LOCK_CLICK_TUTORIAL = true;
        GLOBALS.s.TUTORIAL_PHASE = 18;
        MenusController.s.destroyMenu("SmallScroll", null);
        Invoke("satanGoodJob", 2);
    }

    void satanGoodJob()
    {
        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/BigScroll"));
        MenusController.s.moveMenu(MovementTypes.Left, tempObject, "BigScroll", 0, 0);

        curSatan.SetActive(false);
        curSatan = SatanController.s.SatanExplaning;
        curSatan.SetActive(true);

        buildings = GameObject.FindObjectsOfType(typeof(BE.Building)) as BE.Building[];
        foreach (BE.Building element in buildings)
        {
            element.unactivateHandTutorialUI(3);
        }

        BE.UIShop.instance.activateTabs();

        Invoke("createNextButton", 1);
    }
    #endregion

    public void ILlBeThereForYou()
    {
        BE.SceneTown.instance.move_camera_to_building(new Vector3(3, 0, 15),0.5f,12);
        GLOBALS.s.TUTORIAL_PHASE = 19;
        fscreen = GameObject.FindObjectsOfType(typeof(DialogsTexts)) as DialogsTexts[];
        fscreen[0].closeAndReopen();

        curSatan.SetActive(false);
        curSatan = SatanController.s.SatanExplaning;
        curSatan.SetActive(true);

        Invoke("createNextButton", 1);
    }
    #region Tutorial Phase 19 I will be there for you

    #endregion

    #region Tutorial 1 End
    public void realEndTutorial()
    {
        Debug.Log("[TUT] REAL END TUTORIAL! ");
        GLOBALS.s.LOCK_CAMERA_TUTORIAL = false;
        GLOBALS.s.LOCK_CLICK_TUTORIAL = false;
        GLOBALS.s.TUTORIAL_PHASE = 0;
        GLOBALS.s.TUTORIAL_OCCURING = false;

        curSatan.GetComponent<Satan_HUD>().moveSatan();
        MenusController.s.goOutDestroy("BigScroll", null,"right");

        // PlayerPrefs.SetInt("firstGame", 1);
    }
    #endregion

    #region Catastrophes
    public void catExplanation()
    {
        GLOBALS.s.TUTORIAL_PHASE = 100;
        GLOBALS.s.TUTORIAL_OCCURING = true;
        catastrophesBt.SetActive(true);

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SmallScroll"));
        MenusController.s.moveMenu(MovementTypes.Left, tempObject, "SmallScroll", -85, -58);

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SatanHandCatBT"));
        MenusController.s.moveMenu(MovementTypes.Left, tempObject, "SatanHandCatBT", 0, 0);
    }

    public void click_to_spin()
    {
        MenusController.s.destroyMenu("SmallScroll", null);
        MenusController.s.destroyMenu("SatanHandCatBT", null);
        GLOBALS.s.TUTORIAL_PHASE = 102;

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SmallScroll"));
        MenusController.s.moveMenu(MovementTypes.Left, tempObject, "SmallScroll", 242, -58);

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SatanHandCatBT"));
        MenusController.s.moveMenu(MovementTypes.Left, tempObject, "SatanHandCatBT", -65.27f, -175f);
    }


    public void endCatBT()
    {
        MenusController.s.destroyMenu("SmallScroll", null);
        MenusController.s.destroyMenu("SatanHandCatBT", null);

       GLOBALS.s.TUTORIAL_PHASE = 0;
       GLOBALS.s.TUTORIAL_OCCURING = false;
    }

    public void click_to_spin_again_gems()
    {
        GLOBALS.s.TUTORIAL_PHASE = 104;
        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SmallScroll"));
        MenusController.s.moveMenu(MovementTypes.Left, tempObject, "SmallScroll", 242, -58);

        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SatanHandCatBT"));
        MenusController.s.moveMenu(MovementTypes.Left, tempObject, "SatanHandCatBT", -65.27f, -175f);

        
        //GLOBALS.s.TUTORIAL_OCCURING = false;
    }

    public void end_tutorial_spin()
    {
        MenusController.s.destroyMenu("SmallScroll", null);
        MenusController.s.destroyMenu("SatanHandCatBT", null);

        GLOBALS.s.TUTORIAL_OCCURING = false;
            GLOBALS.s.TUTORIAL_PHASE = 0;
    }
    #endregion

    #region Create Next Bt for Tutorial
    //Create the arrow
    void createNextButton()
    {
        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/ArowNext"));
        MenusController.s.moveMenu(MovementTypes.Right, tempObject, "ArowNext", 0, 0);
        if (GLOBALS.s.TUTORIAL_PHASE == 13)
        {
            Invoke("SatanHand", 0.7f);
        }
    }
    void SatanHand()
    {
            tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/SatanHand"));
            MenusController.s.addToGUIAndRepositeObject(tempObject, "SatanHand");

            SatanHand script;
            script = (SatanHand)tempObject.GetComponent(typeof(SatanHand));
            script.tutorialList();
        
    }

#endregion
}
