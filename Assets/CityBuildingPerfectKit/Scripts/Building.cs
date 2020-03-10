using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.UI;
using DG.Tweening;
///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          Building
///   Description:    class of building
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE
{
    #region !================= Training unity class =====================
    // if building can training unit
    // use this class
    public class GenQueItem
    {
        public Building building;           // owner building 
        public int unitID;              // unit id
        public ArmyType at = null;          // definition of unit
        public int Count;               // allocated count of unit
        public float timeLeft = 0.0f;   // training time left of current training unit

        public GenQueItem(Building _building, int _unitID, int _Count)
        {
            building = _building;
            unitID = _unitID;
            at = TBDatabase.GetArmyType(unitID);
            Count = _Count;
            timeLeft = at.TrainingTime;
        }

        public float Update(float deltaTime)
        {

            if (Count <= 0) return deltaTime;

            // if timeleft is lower then zero
            while (deltaTime > timeLeft)
            {
                deltaTime -= timeLeft;
                // unit created, so decrease count
                Count--;
                building.UnitCreated(unitID);
                timeLeft = at.TrainingTime;
                if (Count == 0)
                    return deltaTime;
            }

            timeLeft -= deltaTime;
            //Debug.Log ("GenQueItem unitID:"+unitID.ToString ()+" Count:"+Count.ToString ()+" timeLeft:"+timeLeft.ToString () );
            return 0.0f;
        }

        public int GetGenLeftTime()
        {
            if (Count == 0)
                return 0;

            return at.TrainingTime * (Count - 1) + (int)timeLeft;
        }

    }

    #endregion

    #region ========================= Bulding Class Definitions
    // building is common class of all building
    // it has capacity relative functions and
    // production relative functions and training relative functions.
    public class Building : MonoBehaviour
    {

        private Vector2 tilePosOld = new Vector2(0, 0); // remember old tile pos while drag building
        public Vector2 tilePos = new Vector2(0, 0);     // tile position
        public Vector2 tileSize = new Vector2(1, 1);    // building's size (forexample, 1x1, 2x2, 3x3 tiles)
        [HideInInspector]
        public bool OnceLanded = false;             // if newly created building, oncelanded is false
        [HideInInspector]
        public bool Landed = false;                 // current building's land state
        public int Type = 0;                        // building type. distinguish building from others with this value
        public int Level = 1;						// level of building
        public float soulsQuant = 0;                          // souls quantity stored

        //ganeobjects of building base prefab
        public GameObject goGrid = null;                    // show how many tiles building has occupied	
        public GameObject goArea = null;                    // yellow area 
        public GameObject goArrowRoot = null;
        public GameObject goArrowXMinus = null;
        public GameObject goArrowXPlus = null;
        public GameObject goArrowZMinus = null;
        public GameObject goArrowZPlus = null;
        public GameObject goCenter = null;              // building mesh
        public GameObject goXInc = null;                    // if building is wall, has more mesh next wall in direction x
        public GameObject goZInc = null;                    // if building is wall, has more mesh next wall in direction z
        public GameObject goRangeIn = null;             // for tower, show range
        public GameObject goRangeOut = null;
        [HideInInspector]
        public BEGround ground = null;
        [HideInInspector]
        public bool Landable = true;

        public GameObject prefUIInfo;                   // prefab for building info ui
        public GameObject prefUICollect;                // prefab for resource collect ui
        private UIInfo uiInfo = null;

        public BuildingType bt = null;                  // building type class of this building
        public BuildingDef def = null;                  // current building's def
        public BuildingDef defNext = null;              // def of next level 
        public BuildingDef defLast = null;              // last def of certain building type


        [HideInInspector]
        public bool Collectable = false;            // if building can produce resource, whether collect is enable 
        public float Production = 0.0f;             // production value 
        public float[] Capacity = new float[(int)PayType.Max];  // if building can store resources, this is resource count being stored
        public float[] StorageCapacity = new float[(int)PayType.Max];   // if building can store resources, this is resource count being stored

        [HideInInspector]
        public bool UpgradeCompleted = false;       // is upgrading completed?
        [HideInInspector]
        public bool InUpgrade = false;              // in upgrading?
        public float UpgradeTimeTotal = 0.0f;
        public float UpgradeTimeLeft = 0.0f;

        //private List<int>	WorkList = new List<int>();
        //private float		WorkTimeTotal = 0.0f;
        //private float		WorkTimeLeft = 0.0f;

        [HideInInspector]
        public List<int> GenUnitCount = new List<int>();
        [HideInInspector]
        public List<GenQueItem> queUnitGen = new List<GenQueItem>();

        List<GameObject> myParticles = new List<GameObject>();
        List<GameObject> myParticles2 = new List<GameObject>();

        GameObject bigDaddy, finalPos;
        bool appearedCollectIcon = false;
        string textColor = "";
        int CapacityTotal;
        double AllProduction;
        bool scaleSoulsAlreadyCalled = false;
        public GameObject explosion;
        GameObject tempObject;

        void Awake()
        {

            // if building can hold trained units
            // set count of unit by type
            for (int i = 0; i < 10; ++i)
            {
                GenUnitCount.Add(0);
            }
        }

        #endregion ====================

        void Start()
        {

        }

        void Update()
        {

            // use BETime to revise times
            float deltaTime = BETime.deltaTime;

            // if building is in selected state, keep color animation
            if (!Landed && (goCenter != null))
            {
                float fColor = Mathf.PingPong(Time.time * 1.5f, 1) * 0.5f + 0.5f;
                Color clrTemp = new Color(fColor, fColor, fColor, 1);
                BEUtil.SetObjectColor(goCenter, clrTemp);
                BEUtil.SetObjectColor(goXInc, clrTemp);
                BEUtil.SetObjectColor(goZInc, clrTemp);
            }

            // if building can produce resources
            if (Landed && !InUpgrade && (def != null) && (def.eProductionType != PayType.None))
            {
                //if(def.eProductionType == PayType.Elixir)
                   // Production = 100;
                if (GLOBALS.s != null && (GLOBALS.s.TUTORIAL_PHASE == 4 || GLOBALS.s.TUTORIAL_PHASE == 17))
                {
                    //Automatically generate souls or fire for tutorial
                    if (GLOBALS.s.TUTORIAL_PHASE == 4)
                        Production = 100;
                    else if (GLOBALS.s.TUTORIAL_PHASE == 17 && def.eProductionType == PayType.Gold)
                        Production = 700;
                }
                else
                {
                    if (GLOBALS.s == null) Debug.Log("GLOBALS IS NULL!!!!!!!!!!");
                    if (def.eProductionType == PayType.Gold)
                    {
                        Production += (float)def.ProductionRate * deltaTime * QA.s.ProductionMultiplier;
                    }

                    else if (def.eProductionType == PayType.Elixir)
                    {
                        Production += (float)def.ProductionRate * deltaTime * GLOBALS.s.ELIXIR_RESEARCH_EXTRA_PERCENTAGE * QA.s.ProductionMultiplier;
                    }
                    
                    else if (def.eProductionType == PayType.Sulfur)
                    {
                        Production += ((float)def.ProductionRate * deltaTime * QA.s.ProductionMultiplier)/5;
                    }
                }


                // check maximum capacity
                if ((int)Production >= def.Capacity[(int)def.eProductionType])
                    Production = def.Capacity[(int)def.eProductionType];

                // is minimum resources generated, then ser collectable flagand show dialog
                if (((int)Production >= 10) && !uiInfo.groupCollect.gameObject.activeInHierarchy)
                {
                    if (appearedCollectIcon == false)
                    {
                        appearedCollectIcon = true;

                        //Automatically appear collect icon in tutorial
                        if (GLOBALS.s.TUTORIAL_PHASE == 4 || (GLOBALS.s.TUTORIAL_PHASE == 17 && def.eProductionType == PayType.Gold))
                        {
                            delayCollect();
                        }
                        else
                        {
                            Invoke("delayCollect", 2f);
                        }
                    }
                }

                if (Collectable)
                {
                    // when production count is reach to max count, then change dialog color to red
                    uiInfo.CollectDialog.color = (Production == def.Capacity[(int)def.eProductionType]) ? Color.red : Color.white;
                }
            }

            // if in upgrade
            if (InUpgrade)
            {

                // if upgrading proceed
                if (!UpgradeCompleted)
                {
                    // decrease left time
                    UpgradeTimeLeft -= deltaTime;

                    // if upgrading done
                    if (UpgradeTimeLeft < 0.0f)
                    {
                        UpgradeTimeLeft = 0.0f;
                        UpgradeCompleted = true;

                        // if building is selected, then update command dialog
                        if (UICommand.Visible && (SceneTown.buildingSelected == this))
                            UICommand.Show(this);
                    }
                }

                // update ui info
                uiInfo.TimeLeft.text = UpgradeCompleted ? "Completed!" : BENumber.SecToString(Mathf.CeilToInt(UpgradeTimeLeft));
                uiInfo.Progress.fillAmount = (UpgradeTimeTotal - UpgradeTimeLeft) / UpgradeTimeTotal;
                uiInfo.groupProgress.alpha = 1;
                uiInfo.groupProgress.gameObject.SetActive(true);
                if (UpgradeCompleted) { UpgradeEnd(); }
            }

            UnitGenUpdate(deltaTime);
        }

        //For the user can click on building to upgrade
        void delayCollect()
        {
            Collectable = true;
            BETween.alpha(uiInfo.groupCollect.gameObject, 0.2f, 0.0f, 1.0f);
            uiInfo.CollectIcon.sprite = TBDatabase.GetPayTypeIcon(def.eProductionType);

            uiInfo.groupCollect.gameObject.SetActive(true);
            uiInfo.groupInfo.gameObject.SetActive(false);
            uiInfo.soulsValue.SetActive(false);
            uiInfo.soulsValueTxt.SetActive(false);
        }

		public void activateHandTutorialUI(int type, bool hell_gate = false)
        {
            if (type == Type)
            {
                uiInfo.SatanHand.gameObject.SetActive(true);
                Vector3 pos = new Vector3(goCenter.transform.position.x + 1.5f, 0f, goCenter.transform.position.z + 1.5f);
				if (!hell_gate) SceneTown.instance.move_camera_to_building(pos, 0.5f, 11);
				else SceneTown.instance.move_camera_to_building(pos, 0.5f, 11, -6f, -6f);
            }

        }
        public void unactivateHandTutorialUI(int type)
        {
            if (type == Type)
                uiInfo.SatanHand.gameObject.SetActive(false);
        }
        // get gem count to finish current upgrading
        public int GetFinishGemCount()
        {
            if (!InUpgrade || UpgradeCompleted) return 0;

            int FinishGemCount = ((int)UpgradeTimeLeft + 1) / 60;
            return FinishGemCount;
        }

        public bool InUpgrading()
        {
            return (InUpgrade && !UpgradeCompleted) ? true : false;
        }

        // initialize building
        public void Init(int type, int level)
        {
            Debug.Log("Init Building");
            Type = type;
            Level = level;

            // delete old meshes
            if (goCenter != null) { Destroy(goCenter); goCenter = null; }
            if (goXInc != null) { Destroy(goXInc); goXInc = null; }
            if (goZInc != null) { Destroy(goZInc); goZInc = null; }

            bt = TBDatabase.GetBuildingType(type);

            // get mesh path type and level
            int displayLevel = (level == 0) ? 1 : level;
            //string meshPath = "Prefabs/Building/"+bt.Name+"_"+displayLevel.ToString ();
            string meshPath;
            if (type != 0)
            {
                meshPath = "Prefabs/Building/" + bt.Name + "_" + 1;
            }
            else
            {
                if (level <= 4)
                {
                    meshPath = "Prefabs/Building/" + bt.Name + "_" + displayLevel.ToString();
                }
                else
                {
                    meshPath = "Prefabs/Building/" + bt.Name + "_" + 4;
                }
            }
            //Debug.Log ("Loading Mesh "+meshPath);

            // instantiate mesh and set to goCenter
            GameObject prefabMesh = Resources.Load(meshPath) as GameObject;

            // goCenter = (GameObject)Instantiate(prefabMesh, Vector3.zero, Quaternion.identity);
            goCenter = (GameObject)Instantiate(prefabMesh, Vector3.zero, Quaternion.identity);

            goCenter.transform.SetParent(gameObject.transform);
            //goCenter.transform.localPosition = Vector3.zero;
            goCenter.transform.localPosition = prefabMesh.transform.localPosition;
            goCenter.transform.localRotation = Quaternion.Euler(0, -90, 0);

            // if wall
            if (type == 2)
            {

                // create x,z side mesh
                string meshSidePath = meshPath + "Side";
                GameObject prefabMeshSide = Resources.Load(meshSidePath) as GameObject;

                goXInc = (GameObject)Instantiate(prefabMeshSide, Vector3.zero, Quaternion.identity);
                goXInc.transform.SetParent(gameObject.transform);
                goXInc.transform.localPosition = Vector3.zero;
                goXInc.transform.localRotation = Quaternion.Euler(0, 180, 0); // rotate to x direction

                goZInc = (GameObject)Instantiate(prefabMeshSide, Vector3.zero, Quaternion.identity);
                goZInc.transform.SetParent(gameObject.transform);
                goZInc.transform.localPosition = Vector3.zero;
                goZInc.transform.localRotation = Quaternion.Euler(0, 90, 0); // rotate to z direction
            }

            // set tile size
            tileSize = new Vector2(bt.TileX, bt.TileZ);
            // set proper material to gogrid
            goGrid.GetComponent<Renderer>().material = Resources.Load("Materials/Tile" + bt.TileX.ToString() + "x" + bt.TileZ.ToString()) as Material;
            goGrid.transform.localScale = new Vector3(bt.TileX, 1, bt.TileZ);
            goArrowXMinus.transform.localPosition = new Vector3(-(0.4f + (float)bt.TileX * 0.5f), 0.01f, 0);
            goArrowXPlus.transform.localPosition = new Vector3((0.4f + (float)bt.TileX * 0.5f), 0.01f, 0);
            goArrowZMinus.transform.localPosition = new Vector3(0, 0.01f, -(0.4f + (float)bt.TileX * 0.5f));
            goArrowZPlus.transform.localPosition = new Vector3(0, 0.01f, (0.4f + (float)bt.TileX * 0.5f));

            goArea.transform.localScale = new Vector3(bt.TileX, 1, bt.TileZ);

            // get ui
            if (transform.Find("UIBuilding"))
            {
                uiInfo = transform.Find("UIBuilding").transform.Find("UIInfo").GetComponent<UIInfo>();
            }
            else
            {
                uiInfo = UIInGame.instance.AddInGameUI(prefUIInfo, transform, new Vector3(0, 1.5f, 0)).GetComponent<UIInfo>();
            }
            uiInfo.groupInfo.gameObject.SetActive(false);
            uiInfo.groupProgress.gameObject.SetActive(false);
            uiInfo.groupCollect.gameObject.SetActive(false);
            uiInfo.Name.text = TBDatabase.GetBuildingName(Type);
            uiInfo.Level.text = "Level " + displayLevel.ToString();
            uiInfo.building = this;

            uiInfo.soulsValue.SetActive(false);
            uiInfo.soulsValueTxt.SetActive(false);

            // currently not used
            goRangeIn.SetActive(false);
            goRangeOut.SetActive(false);

            // initialize values 
            tilePosOld = tilePos;

            CheckLandable();
            CheckNeighbor();
            def = TBDatabase.GetBuildingDef(Type, Level);
            defNext = bt.GetDefine(Level + 1);
            defLast = bt.GetDefLast();
            UpgradeTimeTotal = (defNext != null) ? defNext.BuildTime : 0;
            if (SceneTown.buildingSelected != null)
            {

                Building temp = SceneTown.buildingSelected;

                if (GLOBALS.s.TUTORIAL_OCCURING == false && temp == this)
                {
                    SceneTown.instance.BuildingLandUnselect(true, true);
                    //Descomentar se quiser q o prédio seja selecionado pós build
                    //Se descomentar tem  o bug do Rect qndo dá upgrade
                    // SceneTown.instance.BuildingSelect(this);
                }
                else if (GLOBALS.s.TUTORIAL_OCCURING == true)
                {
                    SceneTown.instance.BuildingLandUnselect(true, true);
                }
            }

            //uiInfo.groupCollect.gameObject.SetActive(true); //APAGAR
        }

        public void UpjustYByState()
        {
            Vector3 vPos = transform.localPosition;
            vPos.y = Landed ? 0.0f : 0.1f;
            transform.localPosition = vPos;
        }

        public void Move(Vector3 vTarget)
        {
            tilePos = ground.GetTilePos(vTarget, tileSize);
            ground.Move(gameObject, tilePos, tileSize);
            //Debug.Log ("Move Pos: "+Landed.ToString ()+" "+TilePosX.ToString()+","+TilePosY.ToString());

            CheckLandable();
            UpjustYByState();
        }

        public void Move(int TileX, int TileZ)
        {
            tilePos = new Vector2(TileX, TileZ);
            ground.Move(gameObject, tilePos, tileSize);
            //Debug.Log ("Move Pos: "+Landed.ToString ()+" "+TilePosX.ToString()+","+TilePosY.ToString());

            CheckLandable();
            UpjustYByState();
        }

        // check tiles of building is vacant and set color of grid object
        public void CheckLandable()
        {
            bool IsVacant = ground.IsVacant(tilePos, tileSize);
            //Debug.Log("CheckLandable Vacant " + IsVacant.ToString() + "Change Color of building");
            Color clrTemp = IsVacant ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
            BEUtil.SetObjectColor(goGrid, "_TintColor", clrTemp);
            Landable = IsVacant;
        }

        public void Land(bool landed, bool animate, bool flagMarota = false)
        {

            Debug.Log("Land called = PARAMETERS");
            Debug.Log("landed = " + landed);
            Debug.Log("animate = " + animate);
            Debug.Log(" ");

            if (Landed == landed)
            {
                Debug.Log("Landed by landed flag");

                return;

            }
            if (landed && !Landable)
            {
                if (((int)tilePosOld.x == -1) && ((int)tilePosOld.y == -1))
                {
                    Debug.Log("Return because of reasons");
                    return;
                }


                tilePos = tilePosOld;
                //Debug.Log ("Land RecoverOldPos: "+TilePosOldX.ToString()+","+TilePosOldY.ToString());
                ground.Move(gameObject, tilePos, tileSize);
                CheckLandable();

                if (!Landable)
                {
                    Debug.Log("Not Landable");
                    return;
                }

            }

            Debug.Log(Landed + " Landed state antes");
            Landed = landed;
            Debug.Log(Landed + " Landed state depois");
            ground.OccupySet(this);

            if (!Landed)
            {
                tilePosOld = tilePos;
                //Debug.Log ("Land Save OldPos: "+TilePosOldX.ToString()+","+TilePosOldY.ToString());
            }
            else
            {
                if (!OnceLanded)
                {
                    OnceLanded = true;

                }

            }


            CheckLandable();
           // Debug.Log("Activating or Not Grid, Is Landed Flag is?  " + Landed);
            if (Type != 0 && Type != 4)
            {
                if (flagMarota == false)
                {
                    goGrid.SetActive(Landed ? false : true);
                }

                if (goArrowRoot != null && flagMarota == false)
                {
                    Debug.Log("Activating goArrowRoot, by Land Flag");
                    goArrowRoot.SetActive(Landed ? false : true);
                }
                uiInfo.soulsValue.SetActive(false);
                uiInfo.soulsValueTxt.SetActive(false);
            }
            else
            {
                goGrid.SetActive(false);
                goArrowRoot.SetActive(false);
                uiInfo.soulsValue.SetActive(false);
                uiInfo.soulsValueTxt.SetActive(false);
            }

            if (uiInfo != null && flagMarota == false)
            {
                //uiInfo.groupProgress.alpha = 0;
                Debug.Log("uiinfo animate = " + animate);
                if (animate)
                {
                    if (Landed)
                    {
                        //Debug.Log("Landed, Desapear UI Txt Build Info");
                        BETween.alpha(uiInfo.groupInfo.gameObject, 0.1f, 1.0f, 0.0f);
                        BETween.enable(uiInfo.groupInfo.gameObject, 0.1f, true, false);
                    }
                    else
                    {
                        //Debug.Log("Not Landed Appear UI Txt Build Info");
                        BETween.alpha(uiInfo.groupInfo.gameObject, 0.1f, 0.0f, 1.0f);
                        uiInfo.groupInfo.gameObject.SetActive(true);

                        BuildingType bt = TBDatabase.GetBuildingType(Type);
                        if (bt.ID >= 11 && bt.ID <= 15 && def != null)

                        {
                            uiInfo.soulsValue.SetActive(true);
                            uiInfo.soulsValueTxt.SetActive(true);
                            uiInfo.soulsValueTxt.GetComponent<Text>().text = (int)soulsQuant + "/" + def.StorageCapacity[(int)PayType.Elixir];
                            if ((int)soulsQuant == def.StorageCapacity[(int)PayType.Elixir])
                            {
                                uiInfo.soulsValueTxt.GetComponent<Text>().color = new Color(1, 0, 0);
                            }
                            else
                            {
                                uiInfo.soulsValueTxt.GetComponent<Text>().color = new Color(1, 1, 1);
                            }

                        }

                    }
                }
                else
                {
                   // Debug.Log("else do sei lá o q");
                    uiInfo.groupInfo.alpha = Landed ? 0 : 1;
                    uiInfo.groupInfo.gameObject.SetActive(Landed ? false : true);
                }
            }

            // if building is wall, check neighbor
            if (Type == 2)
                CheckNeighbor();

            if (Landed && (goCenter != null) && flagMarota == false)
            {
                SceneTown.instance.Save();

               // Debug.Log("Landed Back to original color");
                BEUtil.SetObjectColor(goCenter, Color.white);
                BEUtil.SetObjectColor(goXInc, Color.white);
                BEUtil.SetObjectColor(goZInc, Color.white);
            }

            UpjustYByState();
            if (!SceneTown.instance.InLoading && (BEWorkerManager.instance != null))
                BEWorkerManager.instance.OnTileInfoChanged();
        }


        public void CheckNeighbor()
        {
            if ((goXInc == null) || (goZInc == null)) return;

            // in x and y coordination, next or prev building is samely wall, then show side mesh
            Building bdNeighbor = null;
            bdNeighbor = ground.GetBuilding((int)tilePos.x - 1, (int)tilePos.y); if ((bdNeighbor != null) && (bdNeighbor.Type == 2)) bdNeighbor.CheckNeighbor();
            bdNeighbor = ground.GetBuilding((int)tilePos.x + 1, (int)tilePos.y); goXInc.SetActive((Landed && (bdNeighbor != null) && (bdNeighbor.Type == 2)) ? true : false);
            bdNeighbor = ground.GetBuilding((int)tilePos.x, (int)tilePos.y - 1); if ((bdNeighbor != null) && (bdNeighbor.Type == 2)) bdNeighbor.CheckNeighbor();
            bdNeighbor = ground.GetBuilding((int)tilePos.x, (int)tilePos.y + 1); goZInc.SetActive(((Landed && bdNeighbor != null) && (bdNeighbor.Type == 2)) ? true : false);
        }

        // save building info to xml format
        public void Save(XmlDocument d)
        {
            XmlElement ne = d.CreateElement("Building");
            ne.SetAttribute("Type", Type.ToString());
            ne.SetAttribute("Level", Level.ToString());
            ne.SetAttribute("TilePosX", tilePos.x.ToString());
            ne.SetAttribute("TilePosY", tilePos.y.ToString());

            ne.SetAttribute("InUpgrade", InUpgrade.ToString());
            ne.SetAttribute("UpgradeCompleted", UpgradeCompleted.ToString());
            ne.SetAttribute("UpgradeTimeLeft", UpgradeTimeLeft.ToString());

            ne.SetAttribute("Production", Production.ToString());

            d.DocumentElement.AppendChild(ne);

            // if barrack
            if (Type == 7)
            {
                for (int i = 0; i < queUnitGen.Count; ++i)
                {
                    GenQueItem item = queUnitGen[i];
                    XmlElement neUnit = d.CreateElement("GenQue");
                    neUnit.SetAttribute("unitID", item.unitID.ToString());
                    neUnit.SetAttribute("Count", item.Count.ToString());
                    neUnit.SetAttribute("timeLeft", item.timeLeft.ToString());
                    ne.AppendChild(neUnit);
                }
            }
            // if army camp
            else if (Type == 8)
            {
                for (int i = 0; i < 10; ++i)
                {
                    XmlElement neUnit = d.CreateElement("Unit");
                    neUnit.SetAttribute("Type", i.ToString());
                    neUnit.SetAttribute("Count", GenUnitCount[i].ToString());
                    ne.AppendChild(neUnit);
                }
            }
            else { }
        }

        // load building info to xml format
        public void Load(XmlElement e)
        {
            Type = int.Parse(e.GetAttribute("Type"));
            Level = int.Parse(e.GetAttribute("Level"));
            tilePos.x = float.Parse(e.GetAttribute("TilePosX"));
            tilePos.y = float.Parse(e.GetAttribute("TilePosY"));

            InUpgrade = bool.Parse(e.GetAttribute("InUpgrade"));
            UpgradeCompleted = bool.Parse(e.GetAttribute("UpgradeCompleted"));
            UpgradeTimeLeft = float.Parse(e.GetAttribute("UpgradeTimeLeft"));

            Production = float.Parse(e.GetAttribute("Production"));

            ground.Move(gameObject, tilePos, tileSize);
            CheckLandable();
            Land(true, false);

            for (int i = 0; i < 10; ++i)
            {
                GenUnitCount[i] = 0;
            }

            queUnitGen.Clear();

            XmlNodeList list = e.ChildNodes;
            foreach (XmlElement ele in list)
            {
                if (ele.Name == "Unit")
                {
                    int type = int.Parse(ele.GetAttribute("Type"));
                    int Count = int.Parse(ele.GetAttribute("Count"));
                    GenUnitCount[type] = Count;
                }
                else if (ele.Name == "GenQue")
                {
                    int unitID = int.Parse(ele.GetAttribute("unitID"));
                    int Count = int.Parse(ele.GetAttribute("Count"));
                    float timeLeft = float.Parse(ele.GetAttribute("timeLeft"));

                    GenQueItem item = UnitGenAdd(unitID, Count);
                    item.timeLeft = timeLeft;
                }
                else { }
            }
        }

        // when user clicked this building
        // if building has any kind of completed job
        // do complete and return true
        public bool HasCompletedWork()
        {

            if (UpgradeCompleted) { UpgradeEnd(); return true; }

            if (Collectable)
            {
                Collect();
                defineCapacityTotalAndAllProduction();
                if (AllProduction < CapacityTotal || GLOBALS.s.TUTORIAL_PHASE == 4)
                {

                    return true;
                }
            }

            return false;
        }

        void defineCapacityTotalAndAllProduction()
        {
            if (def.eProductionType == PayType.Elixir)
            {
                CapacityTotal = BEGround.instance.GetCapacityTotal(PayType.Elixir);
                AllProduction = SceneTown.Elixir.Target();
            }
            else if (def.eProductionType == PayType.Sulfur)
            {
                CapacityTotal = BEGround.instance.GetCapacityTotal(PayType.Sulfur);
                AllProduction = SceneTown.Sulfur.Target();
            }
            else
            {
                CapacityTotal = BEGround.instance.GetCapacityTotal(PayType.Gold);
                AllProduction = SceneTown.Gold.Target();
            }

        }
        // collect resources
        public void Collect()
        {

            Debug.Log("[Collect] BEGIN!");
            if ((GLOBALS.s.TUTORIAL_OCCURING == false || GLOBALS.s.TUTORIAL_PHASE == 4 || GLOBALS.s.TUTORIAL_PHASE == 11 || (GLOBALS.s.TUTORIAL_PHASE == 17 && def.eProductionType == PayType.Gold)))
            {
                defineCapacityTotalAndAllProduction();
                if (AllProduction < CapacityTotal)
                {
                    if (GLOBALS.s.nTimesCollectedSouls == 0) {
                        Production = 100;
                        GLOBALS.s.nTimesCollectedSouls++;
                    }

                        initializeTxtAndParticle(Production);


                        //Verify if the production exceeded the capacity
                    if (AllProduction + Production <= CapacityTotal)
                    {
                        Debug.Log("[COLLECT] Production is less than capacity total! " + Production);
                        createParticleUIandCollect(Production, false, 0);
                        /*if (GLOBALS.s.nTimesCollectedSouls == 0) {
                            createParticleUIandCollect(100, false, 0);
                            GLOBALS.s.nTimesCollectedSouls++;
                            Debug.Log("COLLECT FIRST TIME!! ");
                        }
                        else if (GLOBALS.s.TUTORIAL_OCCURING && GLOBALS.s.nTimesCollectedSouls == 1) {
                            createParticleUIandCollect(Production, false, 0);
                            GLOBALS.s.nTimesCollectedSouls++;
                        }
                        else
                            createParticleUIandCollect(Production, false, 0);
                            */



                    }
                    else
                    {
                        Debug.Log("[COLLECT] Production is GREATER than capacity total! " + Production);
                        createParticleUIandCollect(CapacityTotal - (float)AllProduction, true, 1);
                    }

                    if (GLOBALS.s.TUTORIAL_PHASE == 11)
                    {
                        TutorialController.s.soulReallyCollected();
                    }
                    else if (GLOBALS.s.TUTORIAL_PHASE == 17)
                    {
                        TutorialController.s.endOfTutorial();
                    }
                }
                else
                {
                    fullTxt();
                    if (GLOBALS.s.TUTORIAL_PHASE == 4)
                    {
                        TutorialController.s.tutorial1Phase4Clicked();
                    }
                }
            }
        }

        void initializeTxtAndParticle(float production)
        {
            int i;
            int count = (int)(production / 10);

            if (count < 4)
            {
                count = 4;
            }
            else if (count > 15)
            {
                count = 4;
            }

            textColor = "";
            if (def.eProductionType == PayType.Elixir)
            {
                SceneTown.Elixir.ChangeDelta((double)Production);
                textColor = "<color=purple>";
                finalPos = GameObject.Find("ElixirIcon");
                for (i = 0; i < count; i++)
                {
                    myParticles.Add((GameObject)Instantiate(Resources.Load("Prefabs/Elixir")));
                }

            }
            else if (def.eProductionType == PayType.Gold)
            {
                SceneTown.Gold.ChangeDelta((double)Production);
                textColor = "<color=orange>";
                finalPos = GameObject.Find("GoldIcon");
                for (i = 0; i < count; i++)
                {
                    myParticles.Add((GameObject)Instantiate(Resources.Load("Prefabs/Gold")));
                }
            }
            else if (def.eProductionType == PayType.Sulfur)
            {
                SceneTown.Sulfur.ChangeDelta((double)Production);
                textColor = "<color=orange>";
                finalPos = GameObject.Find("SulfurIcon");
                for (i = 0; i < count; i++)
                {
                    //Trocar para icone de SUlfur
                    myParticles.Add((GameObject)Instantiate(Resources.Load("Prefabs/Sulfur")));
                }
            }
            SceneTown.instance.CapacityCheck();

        }

        void createParticleUIandCollect(float discountValue, bool outOfBounds, int gainExpCase)
        {

            appearedCollectIcon = false;
            int particlesQuant = myParticles.Count;
            int i;
            //Particle moving
            // Create the particle off collect
            bigDaddy = GameObject.Find("Canvas");

            for (i = 0; i < particlesQuant; i++)
            {
                myParticles[i].transform.SetParent(bigDaddy.transform, false);
                myParticles[i].transform.localPosition = transform.localPosition;
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

                Vector3 initialpos = new Vector3(screenPoint.x, screenPoint.y, bigDaddy.transform.position.z);
                myParticles[i].GetComponent<particlesLogic>().move(bigDaddy.transform, finalPos.transform, initialpos, i, particlesQuant);
            }

            myParticles.Clear();


            //Gain Exp
            if (gainExpCase == 0)
            {
                if (def.eProductionType == PayType.Elixir)
                {
                    //SceneTown.instance.GainExp((int)Production);
                    //MissionsController.s.OnSoulsCollected(Production);
                    SceneTown.instance.GainExp((int)discountValue);
                    MissionsController.s.OnSoulsCollected(discountValue);
                }
            }
            else
            {
                if (def.eProductionType == PayType.Elixir)
                {
                    SceneTown.instance.GainExp((CapacityTotal - (int)AllProduction));
                    MissionsController.s.OnSoulsCollected(CapacityTotal - (int)AllProduction);
                }
            }

            // show collect ui to show how many resources was collected
            UICollect script = UIInGame.instance.AddInGameUI(prefUICollect, transform, new Vector3(0, 1.5f, 0)).GetComponent<UICollect>();
            script.Name.text = textColor + ((int)discountValue).ToString() + "</color>";
            script.Init(transform, new Vector3(0, 1.0f, 0));
            float scale = (12 / (Camera.main.orthographicSize));

            script.transform.localScale = new Vector3(scale, scale, scale);
            // reset values related to production

            Production = Production - discountValue;
            // debug purposes
            if (Production < 0) Production = 0;

            if (def.eProductionType == PayType.Elixir)
            {
                distributeSouls(discountValue);
            }


            if (outOfBounds == false)
            {
                Collectable = false;
                // hide collect dialog
                BETween.alpha(uiInfo.groupCollect.gameObject, 0.3f, 1.0f, 0.0f);
                BETween.enable(uiInfo.groupCollect.gameObject, 0.3f, true, false);

            }


            // save game - save game when action is occured. not program quit moment
            SceneTown.instance.Save();
        }



        void fullTxt()
        {

            UICollect script = UIInGame.instance.AddInGameUI(prefUICollect, transform, new Vector3(0, 1.5f, 0)).GetComponent<UICollect>();
            script.Name.text = ("FULL!");
            float scale = (12 / (Camera.main.orthographicSize));
            script.transform.localScale = new Vector3(scale, scale, scale);
            script.Init(transform, new Vector3(0, 1.0f, 0));
            if (def.eProductionType == PayType.Elixir) {
                Building[] buildings;
                buildings = GameObject.FindObjectsOfType(typeof(Building)) as Building[];
                int lenght = buildings.Length;
                int i;

                for (i = 0; i < lenght; i++) {
                    buildings[i].scaleBuildingFullCase();
                }

                if (!GLOBALS.s.TUTORIAL_OCCURING) { 
                    GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/Text/WarningMessage"));
                    temp.transform.SetParent(MenusController.s.bigDaddy, false);
                    temp.GetComponent<Text>().text = "Build more Punisher Buildings!";
                    temp.SendMessage("Init");
                }
            }

            if (def.eProductionType == PayType.Gold) {
                if (!GLOBALS.s.TUTORIAL_OCCURING) {
                    GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/Text/WarningMessage"));
                    temp.transform.SetParent(MenusController.s.bigDaddy, false);
                    temp.GetComponent<Text>().text = "Upgrade your Demon Palace!";
                    temp.SendMessage("Init");
                }
            }


        }

        void distributeSouls(float value)
        {
            Building[] buildings;
            buildings = GameObject.FindObjectsOfType(typeof(Building)) as Building[];
            int lenght = buildings.Length;

            float discountEach = 1;
            int i = 0, cont=0;
            float temp;
            while (value > 0)
            {
                for (i = 0; i < lenght; i++)
                {
                    if (value >= 1)
                    {
                        temp = buildings[i].storeSouls(discountEach, transform.position.x, transform.position.y, transform.position.z);
                    }

                    else
                    {
                        temp = buildings[i].storeSouls(value, transform.position.x, transform.position.y, transform.position.z);
                    }

                    value = value - temp;
                    if (value == 0f)
                    {
                        break;
                    }

                }

                cont++;
                if(cont > 1000)
                {
                    break;
                }
                i = 0;
            }

        }

        public float storeSouls(float quant, float portal_x, float portal_y, float portal_z)
        {

            BuildingType bt = TBDatabase.GetBuildingType(Type);
            if (def != null && bt.ID >= 11 && bt.ID <= 15)
            {
                int capacity = def.StorageCapacity[(int)PayType.Elixir];
       

                if (soulsQuant + quant <= capacity)
                {
                    if((portal_x !=666 || portal_y !=666) && myParticles2.Count < 30)
                    {
                        myParticles2.Add((GameObject)Instantiate(Resources.Load("Prefabs/Elixir_rend")));
                        myParticles2[myParticles2.Count - 1].transform.localPosition = new Vector3(portal_x, portal_y, portal_z);
                        myParticles2[myParticles2.Count - 1].GetComponent<particle_soul>().vaiviado(transform.position.x, transform.position.y, transform.position.z);
                        Invoke("clearMyParticles2", 2);
                    }
 
                    soulsQuant += quant;
                    if(scaleSoulsAlreadyCalled == false)
                    {
                        scaleSoulsAlreadyCalled = true;
                        Invoke("scaleBuildingAndSouls", 0.1f);
                    }
                    
                    
                    return quant;

                }
                else if (soulsQuant < capacity)
                {
                    float dif = capacity - soulsQuant;
                    soulsQuant = capacity;
                    if (scaleSoulsAlreadyCalled == false)
                    {
                        scaleSoulsAlreadyCalled = true;
                        Invoke("scaleBuildingAndSouls", 0.1f);
                    }
                    return dif;
                }
                else
                {
                    if (scaleSoulsAlreadyCalled == false)
                    {
                        scaleSoulsAlreadyCalled = true;
                        Invoke("scaleBuildingAndSouls", 0.1f);
                    }
                    return 0f;
                }


            }
            else
            {
                return 0f;
            }

        }

        void clearMyParticles2()
        {
            myParticles2.Clear();
        }
        public void scaleBuildingFullCase()
        {
            if (def != null && bt.ID >= 11 && bt.ID <= 15)
            {
                Invoke("scaleBuildingAndSouls", 0.1f);
            }
        }

        void scaleBuildingAndSouls()
        {
            uiInfo.soulsValueTxt.SetActive(true);
            uiInfo.soulsValue.SetActive(true);
            BETween.scale(gameObject, 0.1f, new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.4f, 1.4f, 1.4f)).loopStyle = BETweenLoop.pingpong;

            uiInfo.soulsValueTxt.GetComponent<Text>().text = (int)soulsQuant + " / " + def.StorageCapacity[(int)PayType.Elixir];
            if ((int)soulsQuant == def.StorageCapacity[(int)PayType.Elixir])
            {
                uiInfo.soulsValueTxt.GetComponent<Text>().color = new Color(1, 0, 0);
            }
            else
            {
                uiInfo.soulsValueTxt.GetComponent<Text>().color = new Color(1, 1, 1);
            }        
            Invoke("clearTxtSouls", 1.4f);
        }

        void clearTxtSouls()
        {
            uiInfo.soulsValue.GetComponent<Image>().DOFade(0, 0.7f);
            uiInfo.soulsValueTxt.GetComponent<CanvasGroup>().DOFade(0, 0.7f).OnComplete(realyClear);
            
        }

        void realyClear()
        {
            scaleSoulsAlreadyCalled = false;
            uiInfo.soulsValueTxt.SetActive(false);
            uiInfo.soulsValue.SetActive(false);
            uiInfo.soulsValue.GetComponent<Image>().DOFade(1, 1f);
            uiInfo.soulsValueTxt.GetComponent<CanvasGroup>().DOFade(1, 1f);
        }

        // whether upgrading is enable
        public bool IsUpgradeEnable()
        {

            BuildingType bt = TBDatabase.GetBuildingType(Type);
            // if this level is max level
            if (bt.LevelMax <= Level) return false;
            // if no next level
            if (defNext == null) return false;

            // if user don't have enough resources to upgrade
            if (((defNext.BuildGoldPrice != 0) && (SceneTown.Gold.Target() < defNext.BuildGoldPrice)) &&
               ((defNext.BuildElixirPrice != 0) && (SceneTown.Elixir.Target() < defNext.BuildElixirPrice)) &&
               ((defNext.BuildElixirPrice != 0) && (SceneTown.Sulfur.Target() < defNext.BuildSulfurPrice)))
            {
                return false;
            }

            return true;
        }
        public void createExplosion()
        {
            tempObject = (GameObject)Instantiate(explosion, new Vector3(gameObject.transform.position.x - 2, gameObject.transform.position.y + 2, gameObject.transform.position.z - 2), Quaternion.Euler(89f, 0f, 0f));
           
        }
        // start upgrade
        public bool Upgrade()
        {

            if (InUpgrade) return false;
            if (bt.LevelMax <= Level) return false;
            if (defNext == null) return false;

            if (Level == 0)
                createExplosion();
            // appearBuildButtons();

            // check whether user has enough resources or not
            // decrease user's resource
            PayType payTypeReturn = PayforBuild(defNext);
            if (payTypeReturn != PayType.None)
            {
                if (payTypeReturn == PayType.Gold) UIDialogMessage.Show("Insufficient Gold", "Ok", "Error");
                else if (payTypeReturn == PayType.Elixir) UIDialogMessage.Show("Insufficient Elixir", "Ok", "Error");
                else if (payTypeReturn == PayType.Sulfur) UIDialogMessage.Show("Insufficient Sulfur", "Ok", "Error");
                //else if(payTypeReturn == PayType.Gem) 		UIDialogMessage.Show("Insufficient Gem", "Ok", "Error");
                else { }

                return false;


            }

            // prepare upgrade
            UpgradeCompleted = false;
            InUpgrade = true;

            // if upgrade time is zero(like wall) then upgrade immediately
            if (UpgradeTimeTotal > 0)
            {
                UpgradeTimeLeft = UpgradeTimeTotal;
                uiInfo.TimeLeft.text = BENumber.SecToString(Mathf.CeilToInt(UpgradeTimeLeft));
                uiInfo.Progress.fillAmount = (UpgradeTimeTotal - UpgradeTimeLeft) / UpgradeTimeTotal;
                BETween.alpha(uiInfo.groupProgress.gameObject, 0.3f, 0.0f, 1.0f);
                uiInfo.groupProgress.gameObject.SetActive(true);
            }
            else
            {
                UpgradeEnd();
            }

            return true;
        }

        // cancel current upgrade
        public void UpgradeCancel()
        {
            InUpgrade = false;
            UpgradeCompleted = false;

            uiInfo.groupProgress.gameObject.SetActive(false);
            uiInfo.soulsValue.SetActive(false);
            uiInfo.soulsValueTxt.SetActive(false);

            RefundBuild(defNext);
            SceneTown.instance.Save();
        }

        // when upgraded ended, user clicked this building
        public void UpgradeEnd()
        {
            if (GLOBALS.s.TUTORIAL_PHASE == 9)
            {
                TutorialController.s.punisherCapacityExplanation();
            }
            else if (GLOBALS.s.TUTORIAL_PHASE == 16)
            {
                TutorialController.s.collectDemonsPhase();
            }
            // initialize with next level
            Init(Type, Level + 1);
            InUpgrade = false;
            UpgradeCompleted = false;


            if (Level == 1)
            {

                MissionsController.s.OnBuildingComplete(Type);

                GLOBALS.s.ELIXIR_RESEARCH_EXTRA_PERCENTAGE = GLOBALS.s.ELIXIR_RESEARCH_EXTRA_PERCENTAGE + (float)def.SoulProdutionInc / 100;

                if(bt.ID == 15) BE.BEAudioManager.SoundPlay(12);

            }


            // increase experience
            //SceneTown.instance.GainExp(def.RewardExp);
            // if building has capacity value, then recalc capacity of all resources

            if ((def.StorageCapacity[(int)PayType.Gold] != 0) || (def.StorageCapacity[(int)PayType.Elixir] != 0) || (def.StorageCapacity[(int)PayType.Sulfur] != 0))
            {
                SceneTown.instance.CapacityCheck();
            }

            //if(def)
            // save game - save game when action is occured. not program quit moment
            SceneTown.instance.Save();
        }

        // check user has enough resources to upgrade.
        // incase yes, decrease resources and return Paytype.None
        // othewise return the type of resource needed.
        public PayType PayforBuild(BuildingDef _bd)
        {
            Debug.Log("PAY FOR BUILD CALLED !! ");
            PayType payTypeReturn = PayType.None;
            if ((payTypeReturn == PayType.None) && (_bd.BuildGoldPrice != 0) && (SceneTown.Gold.Target() < _bd.BuildGoldPrice)) payTypeReturn = PayType.Gold;
            if ((payTypeReturn == PayType.None) && (_bd.BuildElixirPrice != 0) && (SceneTown.Elixir.Target() < _bd.BuildElixirPrice)) payTypeReturn = PayType.Elixir;
            if ((payTypeReturn == PayType.None) && (_bd.BuildSulfurPrice != 0) && (SceneTown.Sulfur.Target() < _bd.BuildSulfurPrice)) payTypeReturn = PayType.Sulfur;
            if ((payTypeReturn == PayType.None) && (_bd.BuildGemPrice != 0) && (SceneTown.Gem.Target() < _bd.BuildGemPrice)) payTypeReturn = PayType.Gem;

            if (payTypeReturn == PayType.None)
            {
                if (_bd.BuildGoldPrice != 0) SceneTown.Gold.ChangeDelta(-_bd.BuildGoldPrice);
                if (_bd.BuildElixirPrice != 0) SceneTown.Elixir.ChangeDelta(-_bd.BuildElixirPrice);
                if (_bd.BuildSulfurPrice != 0) SceneTown.Sulfur.ChangeDelta(-_bd.BuildSulfurPrice);
                //if(_bd.BuildGemPrice != 0) 		SceneTown.Gem.ChangeDelta(-_bd.BuildGemPrice);

                SceneTown.instance.CapacityCheck();
            }

            return payTypeReturn;
        }

        // when user cancel upgrade, refund half of resources
        public void RefundBuild(BuildingDef _bd)
        {
            if (_bd == null) return;

            // when user cancel upgrade, refund half of upgrade price 
            if (_bd.BuildGoldPrice != 0) { SceneTown.Gold.ChangeDelta(_bd.BuildGoldPrice / 2); }
            if (_bd.BuildElixirPrice != 0) { SceneTown.Elixir.ChangeDelta(_bd.BuildElixirPrice / 2); }
            if (_bd.BuildSulfurPrice != 0) { SceneTown.Sulfur.ChangeDelta(_bd.BuildSulfurPrice / 2); }
            if (_bd.BuildGemPrice != 0) { SceneTown.Gem.ChangeDelta(_bd.BuildGemPrice / 2); }
        }

        // if building can training unit
        // add unit to training aue
        public GenQueItem UnitGenAdd(int unitID, int Count)
        {
            //Debug.Log ("Building::UnitGenAdd "+unitID.ToString()+"x"+Count.ToString());
            // search unit que list with given unit id
            int idx = queUnitGen.FindIndex(x => x.unitID == unitID);
            if (idx == -1)
            {
                GenQueItem item = new GenQueItem(this, unitID, Count);
                queUnitGen.Add(item);
                //Debug.Log ("Building::UnitGenAdd create GenQueItem unitID:"+unitID.ToString());
                return item;
            }
            else
            {
                GenQueItem item = queUnitGen[idx];
                item.Count += Count;
                //Debug.Log ("Building::UnitGenAdd increase GenQueItem unitID:"+unitID.ToString()+" Count:"+item.Count.ToString());
                return item;
            }
        }

        // if user cancel training unit, decrease count of training unit
        public bool UnitGenRemove(int unitID, int Count)
        {
            //Debug.Log ("Building::UnitGenAdd "+unitID.ToString()+"x"+Count.ToString());
            // search unit que list with given unit id
            int idx = queUnitGen.FindIndex(x => x.unitID == unitID);
            if (idx != -1)
            {
                GenQueItem item = queUnitGen[idx];
                item.Count -= Count;
                //Debug.Log ("Building::UnitGenRemove decrease unitID:"+unitID.ToString()+" Count:"+item.Count.ToString());
                if (item.Count < 0) item.Count = 0;
                return false;
            }

            //Debug.Log ("Building::UnitGenRemove decrease unitID:"+unitID.ToString()+" Not Found");
            return false;
        }

        // 
        public void UnitGenUpdate(float deltaTime)
        {

            for (int i = 0; i < queUnitGen.Count; ++i)
            {
                GenQueItem item = queUnitGen[i];
                if (item.Count == 0)
                {
                    //Debug.Log ("Building::UnitGenUpdate delete "+item.unitID.ToString());
                    UIDialogTraining.instance.ItemRemove(item.unitID);
                    queUnitGen.RemoveAt(i);
                    break;
                }
            }

            while (queUnitGen.Count > 0)
            {
                GenQueItem item = queUnitGen[0];
                deltaTime = item.Update(deltaTime);
                if (item.Count == 0)
                {
                    //Debug.Log ("Building::UnitGenUpdate delete "+item.unitID.ToString());
                    UIDialogTraining.instance.ItemRemove(item.unitID);
                    queUnitGen.RemoveAt(0);
                }

                if (deltaTime < 0.01f)
                    return;
            }
        }

        // a unit was created
        public void UnitCreated(int unitID)
        {
            // find army camp with space
            // create unit
            // set unit's base camp position

            GenUnitCount[unitID]++;
            //Debug.Log ("Building::UnitCreated "+unitID.ToString()+" UnitCount:"+GenUnitCount[unitID].ToString ());
        }

        // fill building info in building info dialog
        public void UIFillProgress(ProgressInfo progress, BDInfo type)
        {

            if (type == BDInfo.CapacityGold)
            {
                progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(PayType.Gold);
                progress.textInfo.text = "Capacity : " + Capacity[(int)PayType.Gold].ToString("#,##0") + "/" + def.Capacity[(int)PayType.Gold].ToString("#,##0");
                progress.imageMiddle.fillAmount = 0.0f;
                progress.imageFront.fillAmount = Capacity[(int)PayType.Gold] / (float)(def.Capacity[(int)PayType.Gold]);
            }
            else if (type == BDInfo.CapacityElixir)
            {
                progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(PayType.Elixir);
                progress.textInfo.text = "Capacity : " + Capacity[(int)PayType.Elixir].ToString("#,##0") + "/" + def.Capacity[(int)PayType.Elixir].ToString("#,##0");
                progress.imageMiddle.fillAmount = 0.0f;
                progress.imageFront.fillAmount = Capacity[(int)PayType.Elixir] / (float)(def.Capacity[(int)PayType.Elixir]);
            }
            else if (type == BDInfo.CapacitySulfur)
            {
                progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(PayType.Sulfur);
                progress.textInfo.text = "Capacity : " + Capacity[(int)PayType.Sulfur].ToString("#,##0") + "/" + def.Capacity[(int)PayType.Sulfur].ToString("#,##0");
                progress.imageMiddle.fillAmount = 0.0f;
                progress.imageFront.fillAmount = Capacity[(int)PayType.Sulfur] / (float)(def.Capacity[(int)PayType.Sulfur]);
            }
            else if (type == BDInfo.Capacity)
            {
                progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(def.eProductionType);
                progress.textInfo.text = "Capacity : " + Production.ToString("#,##0") + "/" + def.Capacity[(int)def.eProductionType].ToString("#,##0");
                progress.imageMiddle.fillAmount = 0.0f;
                progress.imageFront.fillAmount = Production / (float)(def.Capacity[(int)def.eProductionType]);
            }
            else if (type == BDInfo.ProductionRate)
            {
                progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(def.eProductionType);
                progress.textInfo.text = "ProductionRate : " + ((int)(def.ProductionRate * 3600.0f)).ToString("#,##0") + " per Hour";
                progress.imageMiddle.fillAmount = 0.0f;
                progress.imageFront.fillAmount = def.ProductionRate / defLast.ProductionRate;
            }
            else if (type == BDInfo.HitPoint)
            {
                progress.imageIcon.sprite = Resources.Load<Sprite>("Icons/Heart");
                progress.textInfo.text = "HitPoints : " + def.HitPoint.ToString("#,##0") + "/" + def.HitPoint.ToString("#,##0");
                progress.imageMiddle.fillAmount = 0.0f;
                progress.imageFront.fillAmount = (float)def.HitPoint / (float)def.HitPoint;
            }
            else if (type == BDInfo.StorageCapacity)
            {
                PayType payType = PayType.None;
                for (int i = 0; i < (int)PayType.Max; ++i)
                {
                    if (def.Capacity[i] != 0)
                    {
                        payType = (PayType)i;
                        break;
                    }
                }
                if (payType == PayType.None) return;

                progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(payType);
                progress.textInfo.text = "Storage Capacity : " + Capacity[(int)payType].ToString("#,##0") + "/" + def.Capacity[(int)payType].ToString("#,##0");
                progress.imageMiddle.fillAmount = 0.0f;
                progress.imageFront.fillAmount = Capacity[(int)payType] / (float)def.Capacity[(int)payType];
            }
            else { }
        }

        // fill building upgrading info in building upgrade ask dialog
        public void UIFillProgressWithNext(ProgressInfo progress, BDInfo type)
        {

            if (type == BDInfo.CapacityGold)
            {
                progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(PayType.Gold);
                progress.textInfo.text = "Capacity : " + def.Capacity[(int)PayType.Gold].ToString("#,##0") + "+" + (defNext.Capacity[(int)PayType.Gold] - def.Capacity[(int)PayType.Gold]).ToString("#,##0");
                progress.imageMiddle.fillAmount = (defNext == null) ? 0.0f : (float)defNext.Capacity[(int)PayType.Gold] / (float)defLast.Capacity[(int)PayType.Gold];
                progress.imageFront.fillAmount = (float)def.Capacity[(int)PayType.Gold] / (float)defLast.Capacity[(int)PayType.Gold];
            }
            else if (type == BDInfo.CapacityElixir)
            {
                progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(PayType.Elixir);
                progress.textInfo.text = "Capacity : " + def.Capacity[(int)PayType.Elixir].ToString("#,##0") + "+" + (defNext.Capacity[(int)PayType.Elixir] - def.Capacity[(int)PayType.Elixir]).ToString("#,##0");
                progress.imageMiddle.fillAmount = (defNext == null) ? 0.0f : (float)defNext.Capacity[(int)PayType.Elixir] / (float)defLast.Capacity[(int)PayType.Elixir];
                progress.imageFront.fillAmount = (float)def.Capacity[(int)PayType.Elixir] / (float)defLast.Capacity[(int)PayType.Elixir];
            }
            else if (type == BDInfo.CapacitySulfur)
            {
                progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(PayType.Sulfur);
                progress.textInfo.text = "Capacity : " + def.Capacity[(int)PayType.Sulfur].ToString("#,##0") + "+" + (defNext.Capacity[(int)PayType.Sulfur] - def.Capacity[(int)PayType.Elixir]).ToString("#,##0");
                progress.imageMiddle.fillAmount = (defNext == null) ? 0.0f : (float)defNext.Capacity[(int)PayType.Sulfur] / (float)defLast.Capacity[(int)PayType.Sulfur];
                progress.imageFront.fillAmount = (float)def.Capacity[(int)PayType.Sulfur] / (float)defLast.Capacity[(int)PayType.Sulfur];
            }
            else if (type == BDInfo.Capacity)
            {
                progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(def.eProductionType);
                progress.textInfo.text = "Capacity : " + def.Capacity[(int)def.eProductionType].ToString("#,##0") + "+" + (defNext.Capacity[(int)defNext.eProductionType] - def.Capacity[(int)def.eProductionType]).ToString("#,##0");
                progress.imageMiddle.fillAmount = (defNext == null) ? 0.0f : (float)defNext.Capacity[(int)defNext.eProductionType] / (float)defLast.Capacity[(int)defLast.eProductionType];
                progress.imageFront.fillAmount = (float)def.Capacity[(int)def.eProductionType] / (float)defLast.Capacity[(int)defLast.eProductionType];
            }
            else if (type == BDInfo.ProductionRate)
            {
                progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(def.eProductionType);
                progress.textInfo.text = "ProductionRate : " + ((int)(def.ProductionRate * 3600.0f)).ToString("#,##0") + "+" + ((int)((defNext.ProductionRate - def.ProductionRate) * 3600.0f)).ToString("#,##0") + " per Hour";
                progress.imageMiddle.fillAmount = (defNext == null) ? 0.0f : defNext.ProductionRate / defLast.ProductionRate;
                progress.imageFront.fillAmount = def.ProductionRate / defLast.ProductionRate;
            }
            else if (type == BDInfo.HitPoint)
            {
                progress.imageIcon.sprite = Resources.Load<Sprite>("Icons/Heart");
                progress.textInfo.text = "HitPoints : " + def.HitPoint.ToString("#,##0") + "+" + (defNext.HitPoint - def.HitPoint).ToString("#,##0");
                progress.imageMiddle.fillAmount = (defNext == null) ? 0.0f : (float)defNext.HitPoint / (float)defLast.HitPoint;
                progress.imageFront.fillAmount = (float)def.HitPoint / (float)defLast.HitPoint;
            }
            else if (type == BDInfo.StorageCapacity)
            {
                PayType payType = PayType.None;
                for (int i = 0; i < (int)PayType.Max; ++i)
                {
                    if (def.Capacity[i] != 0)
                    {
                        payType = (PayType)i;
                        break;
                    }
                }
                if (payType == PayType.None) return;

                progress.imageIcon.sprite = TBDatabase.GetPayTypeIcon(payType);
                progress.textInfo.text = "Storage Capacity : " + def.Capacity[(int)payType].ToString("#,##0") + "+" + (defNext.Capacity[(int)payType] - def.Capacity[(int)payType]).ToString("#,##0");
                progress.imageMiddle.fillAmount = defNext.Capacity[(int)payType] / (float)defLast.Capacity[(int)payType];
                progress.imageFront.fillAmount = def.Capacity[(int)payType] / (float)defLast.Capacity[(int)payType];
            }
            else { }
        }

    }

}