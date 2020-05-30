using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIShopItem
///   Description:    building item in shop dialog
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {
	
	public class UIShopItem : MonoBehaviour {

		private BuildingType 	bt = null;
		private BuildingDef 	bd = null;
		private InAppItem 		ia = null;
		private bool 			CountAvailable = true;
		private bool 			PriceAvailable = true;

		public 	Image 		Border;
		public 	Image 		Background;
		public 	Text 		Name;
		public 	Text 		Info;
		public 	Image 		Icon;
		public 	GameObject 	goDisabled;
		public 	Text 		DisabledInfo;
        public GameObject LockedInterrogation;
		public 	GameObject 	goBuild;
		public 	Image 		PriceIcon;
		public 	Text 		Price;
		public 	Text 		BuildTimeInfo;
		public 	Text 		BuildTime;
		public 	Text 		BuildCountInfo;
		public 	Text 		BuildCount;

		void Start () {
			
		}

		void Update () {

			// change price text color by checking resources
			if(bd != null)
				PriceAvailable = bd.PriceInfoCheck(Price);
		}

		// initialize with building
		public void Init(BuildingType _bt) {
			bt = _bt;
			Name.text = bt.Name;
			Info.text = bt.Info;

			// check if current building count of this type is larger then max count 
			int CountMax = (_bt.ID == 1) ? 5 : BEGround.instance.GetBuildingCountMax(bt.ID);
			int Count = BEGround.instance.GetBuildingCount(bt.ID);
			if(Count >= CountMax) {
				// if can't create more building 
				CountAvailable = false;

				// get next available townhall level
				int NextTownLevel = bt.GetMoreBuildTownLevel(Count);
                if (NextTownLevel == -1) DisabledInfo.text = "Reach Maximum Count";
                else DisabledInfo.text = "Need " + TBDatabase.GetLevelExpTotal(NextTownLevel).ToString() + " Soul Population";
                //else 					DisabledInfo.text = "Need Souls" + NextTownLevel.ToString ();
                goDisabled.SetActive(true);
			}

            //Lock if dont have Town Hall Level enough
            bool locked = false;
            int THRankReq = TBDatabase.TownHallLevelRequired(bt.ID, 0);
            //if (THRankReq > BEGround.instance.GetTownHallLevel())
            if (THRankReq > GLOBALS.s.USER_RANK)
            {
                CountAvailable = false;
                locked = true;
                //int NextTownLevel = bt.GetMoreBuildTownLevel(Count);
                //Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa " + NextTownLevel);
               // Debug.Log("nxsyyyyyyyyyyyyyyyyyyyyyyyyy " + TBDatabase.GetLevelExpTotal(2));
                
                //DisabledInfo.text = "Need Demon Palace Level  " + THRankReq;
                DisabledInfo.text = "Need " + TBDatabase.GetLevelExpTotal(THRankReq).ToString() + " Soul Population";

                goDisabled.SetActive(true);
                LockedInterrogation.SetActive(true);
            }


			bd = bt.Defs[0];

			// if building type is house (house's price is related to current house count)
			// change price
			if(_bt.ID == 1) {
				if(Count == 1) 		bd.BuildGemPrice = 400;
				else if(Count == 2) bd.BuildGemPrice = 800;
				else if(Count == 3) bd.BuildGemPrice = 1600;
				else if(Count == 4) bd.BuildGemPrice = 3200;
				else {}
			}

			// set ui info
			bd.PriceInfoApply(PriceIcon, Price);
			BuildTimeInfo.text = "Build time:";
			BuildTime.text = BENumber.SecToString(bd.BuildTime);
			BuildCountInfo.text = "Built:";
			BuildCount.text = Count.ToString ()+"/"+CountMax.ToString ();

			Border.color = CountAvailable ? new Color32(0,150,186,255) : new Color32(133,119,108,255);
			Icon.sprite = Resources.Load<Sprite>("Icons/Building/"+bt.Name);
            if(locked == true)
                Icon.color = CountAvailable ? new Color32(255, 255, 255, 255) : new Color32(2, 2, 2, 255);
            else
			    Icon.color = CountAvailable ? new Color32(255,255,255,255) : new Color32(133, 119, 108, 255);
        }

		// initialized with inApp item
		public void Init(InAppItem _ia) {
			ia = _ia;
			Name.text = ia.Name;
			Info.text = ia.Gem.ToString ("#,##0");
			Price.text = ia.Price;
			BuildTimeInfo.text = "";
			BuildTime.text = "";
			BuildCountInfo.text = "";
			BuildCount.text = "";

			Border.color = CountAvailable ? new Color32(0,150,186,255) : new Color32(133,119,108,255);
			Icon.color = CountAvailable ? new Color32(255,255,255,255) : new Color32(133,119,108,255);
		}

		public void Clicked() {

			// if item is building
			if(bt != null) {
                Debug.Log ("UIShopItem selected : "+bt.ID
                    );

                //building creation is enabled
                //if(CountAvailable && PriceAvailable && BEWorkerManager.instance.WorkerAvailable()
                if (CountAvailable && PriceAvailable) {
                    if (GLOBALS.s.TUTORIAL_OCCURING == true)
                    {
                        if (GLOBALS.s.TUTORIAL_PHASE == 8)
                        {
                            TutorialController.s.destroySelectPunisher();
                        }
                        else if (GLOBALS.s.TUTORIAL_PHASE == 15)
                        {
                            TutorialController.s.impClicked();
                        }
                    }
                    if (GLOBALS.s.TUTORIAL_PHASE != 21 || bt.ID == 3)
                    {
                        // add building. if buildtime is zero, then create level1, else not, create o level and upgrade start
                        Building script = BEGround.instance.BuildingAdd(bt.ID, (bd.BuildTime == 0) ? 1 : 0);
                        if (script != null)
                        {
                            //script.Move(Vector3.zero);
                            script.Move(new Vector3(-17.5f,0f,-17.5f));
                            BEGround.instance.MoveToVacantTilePos(script);
                            Debug.Log("UIShopItem called check landable ");
                            script.CheckLandable();
                            SceneTown.instance.BuildingSelect(script);
                            UIShop.instance.Hide();
                        }
                    }

                }
                else {
                    if (GLOBALS.s.TUTORIAL_PHASE != 21)
                    { 
                    // show message box
                        if (!CountAvailable) UIDialogMessage.Show("Upgrade Demon Palace to construct a new build", "Ok", "Demon Palace Upper Level Required");
                        else if (!PriceAvailable) UIDialogMessage.Show("More Resource Required", "Ok", "Error");
                        else UIDialogMessage.Show("All workers are working now", "Ok", "Error");
                    }
				}
			}

			// if item is inapp
			if(ia != null) {
				//Debug.Log ("UIShopItem selected : "+ia.Name);
				// add gem
				SceneTown.Gem.ChangeDelta(ia.Gem);
				UIShop.instance.Hide ();
				SceneTown.instance.Save ();
			}
		}
	}
	
}