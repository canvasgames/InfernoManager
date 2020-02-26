
#region Import
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
#endregion


///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          TBDatabase
///   Description:    
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

    // resource price to build buildings
    public enum PayType {
        None = -1,
        Gold = 0,
        Elixir = 1,
        Sulfur = 2,
        Evilness = 3,
        Gem = 4,
        Max = 5,
        
    }

    // definition of inapp purchase item
    [System.Serializable]
    public class InAppItem {

        public string Name; // item name (for example 'pack of gems')
        public int Gem; // gem count to get
        public string Price;    // price (for example 0.99$)

        public InAppItem(string _Name, int _Gem, string _Price) {
            Name = _Name;
            Gem = _Gem;
            Price = _Price;
        }
    }


    #region ------------- Army (not used) ------------------
    // definition of troop(army) unit
    [System.Serializable]
    public class ArmyDef {

        public GameObject prefab;
        public int DamagePerSecond;
        public int HitPoint;
        public int TrainingCost;
        public int ResearchCost;                // research cost to level up in laboratory
        public int LaboratoryLevelRequired; // Laboratory Level Required	
        public int ResearchTime;                // reserch time

        public ArmyDef(int _DamagePerSecond, int _HitPoint, int _TrainingCost, int _ResearchCost, int _LevelRequired, int _ResearchTime) {
            DamagePerSecond = _DamagePerSecond;
            HitPoint = _HitPoint;
            TrainingCost = _TrainingCost;
            ResearchCost = _ResearchCost;
            LaboratoryLevelRequired = _LevelRequired;
            ResearchTime = _ResearchTime;
        }

        // check if user has enough resource to training and set value to the text ui with color
        public bool PriceInfoCheck(Text _Price) {
            bool Available = false;
            _Price.text = TrainingCost.ToString("#,##0");
            if (SceneTown.Elixir.Target() >= TrainingCost) { _Price.color = Color.white; Available = true; }
            else { _Price.color = Color.red; Available = false; }

            return Available;
        }
    }

    // unit's preferred targets
    public enum PreffredTarget {
        None = 0,
        Resource = 1,
        Defense = 2,
    };

    // unit's attack type
    public enum AttackType {
        None = 0,
        Melee = 1,
    };

    // definition of army category type
    [System.Serializable]
    public class ArmyType {

        public string Name;
        public string Info;
        public int LevelMax;

        public PreffredTarget ePreferredTarget;
        public AttackType eAttackType;
        public int HousingSpace;            // need how many space per i unit
        public int TrainingTime = 10;
        public int MoveSpeed;
        public int AttackSpeed;
        public int BarrackLevelRequired;    //Barrack Level Required	
        public int AttackRange;

        public List<ArmyDef> Defs = new List<ArmyDef>();

        public ArmyType(string _Name, string _Info, int _LevelMax) {
            Name = _Name;
            Info = _Info;
            LevelMax = _LevelMax;
        }

        public void Add(ArmyDef def) {
            Defs.Add(def);
            def.prefab = Resources.Load("Prefabs/Army/" + Name + "_" + Defs.Count.ToString()) as GameObject;
        }

        public ArmyDef GetDefLast() {
            return Defs[Defs.Count - 1];
        }

        public ArmyDef GetDefine(int level) {
            return Defs[level - 1];
        }
    }

    public enum DamageType {
        None = 0,
        SingleTarget = 1,
        Splash = 2,
    };

    public enum TargetMove {
        None = 0,
        Ground = 1,
        Air = 2,
        GroundnAir = 3,
    };

    #endregion

    #region ========= BuildingDef Class ===========

    [System.Serializable]
    public class BuildingDef {

        public GameObject prefab;
        public int HitPoint;
        public int BuildGoldPrice;
        public int BuildElixirPrice;
        public int BuildGemPrice;
        public int BuildSulfurPrice;
        public int BuildTime;
        public int RewardExp;
        public int TownHallLevelRequired;		//Demon Palace Level Required	
        public int RankRequired;

        ////Gold Mine, Elixir Collector
        public PayType eProductionType = PayType.None;
        public float ProductionRate = 0;
        public int[] Capacity = new int[(int)PayType.Max];
        public int[] StorageCapacity = new int[(int)PayType.Max];
        public int SoulProdutionInc = 0;

        //Barracks
        public int TrainingQueueMax = 50;
        //public int []			TrainingEnable;

        //Army Camp
        public int TroopCapacity = 70;

        //Defense
        public float DamagePerSecond = 0.0f;
        public float DamagePerShot = 0.0f;
        public float Range = 0.0f;
        public float AttackSpeed = 0.0f;
        public DamageType eDamageType = DamageType.None;
        public TargetMove eTagetType = TargetMove.None;

        public BuildingDef(int _HitPoint, int _BuildGoldPrice, int _BuildElixirPrice, int _BuildGemPrice, int _BuildSulfurPrice, int _BuildTime, int _LevelRequired)
        {
            BuildingDefInit(_HitPoint, _BuildGoldPrice, _BuildElixirPrice, _BuildGemPrice, _BuildSulfurPrice, _BuildTime, _LevelRequired, 0, 0);
        }

        public BuildingDef(int _HitPoint, int _BuildGoldPrice, int _BuildElixirPrice, int _BuildGemPrice, int _BuildSulfurPrice, int _BuildTime, int _LevelRequired, int _RankRequired)
        {
            BuildingDefInit(_HitPoint, _BuildGoldPrice, _BuildElixirPrice, _BuildGemPrice, _BuildSulfurPrice, _BuildTime, _LevelRequired, _RankRequired, 0);
        }

        public BuildingDef(int _HitPoint, int _BuildGoldPrice, int _BuildElixirPrice, int _BuildGemPrice, int _BuildSulfurPrice, int _BuildTime, int _LevelRequired, int _RankRequired, int _SoulProductionInc)
        {
            BuildingDefInit(_HitPoint, _BuildGoldPrice, _BuildElixirPrice, _BuildGemPrice, _BuildSulfurPrice, _BuildTime, _LevelRequired, _RankRequired, _SoulProductionInc);
        }

        public void BuildingDefInit(int _HitPoint, int _BuildGoldPrice, int _BuildElixirPrice, int _BuildGemPrice, int _BuildSulfurPrice, int _BuildTime, int _LevelRequired, int _RankRequired, int _SoulProductionInc)
        {
            HitPoint = _HitPoint;
            BuildGoldPrice = _BuildGoldPrice;
            BuildElixirPrice = _BuildElixirPrice;
            BuildGemPrice = _BuildGemPrice;
            BuildSulfurPrice = _BuildSulfurPrice;

            BuildTime = _BuildTime;
            //RewardExp				= (int)Mathf.Sqrt(BuildTime);
            RewardExp = 0;
            TownHallLevelRequired = _LevelRequired;
            RankRequired = _RankRequired;
            SoulProdutionInc = _SoulProductionInc;

            for (int i = 0; i < (int)PayType.Max; ++i)
            {
                Capacity[i] = 0;
            }
        }

        public BuildingDef(XmlElement e)
        {

            HitPoint = int.Parse(e.GetAttribute("HitPoint"));
            string strPrice = e.GetAttribute("BuildPrice");
            string[] sitPriceSub = strPrice.Split(',');
            BuildGoldPrice = int.Parse(sitPriceSub[0]);
            BuildElixirPrice = int.Parse(sitPriceSub[1]);
            BuildSulfurPrice = int.Parse(sitPriceSub[2]);
            BuildGemPrice = int.Parse(sitPriceSub[4]);
            
            BuildTime = int.Parse(e.GetAttribute("BuildTime"));
            //RewardExp				= (int)Mathf.Sqrt(BuildTime);
            RewardExp = 0;
            TownHallLevelRequired = int.Parse(e.GetAttribute("TownHallLevelRequired"));

            for (int i = 0; i < (int)PayType.Max; ++i)
            {
                Capacity[i] = 0;
            }

            XmlNodeList list = e.ChildNodes;
            foreach (XmlElement ele in list)
            {
                if (ele.Name == "Capacity")
                {
                    Capacity[(int)PayType.Gold] = int.Parse(ele.GetAttribute("Gold"));
                    Capacity[(int)PayType.Elixir] = int.Parse(ele.GetAttribute("Elixir"));
                    Capacity[(int)PayType.Sulfur] = int.Parse(ele.GetAttribute("Sulfur"));
                }
                else if (ele.Name == "StorageCapacity")
                {
                    StorageCapacity[(int)PayType.Gold] = int.Parse(ele.GetAttribute("Gold"));
                    StorageCapacity[(int)PayType.Elixir] = int.Parse(ele.GetAttribute("Elixir"));
                    StorageCapacity[(int)PayType.Sulfur] = int.Parse(ele.GetAttribute("Sulfur"));
                }
                else if (ele.Name == "Production")
                {
                    string Type = ele.GetAttribute("Type");
                    for (int i = 0; i < (int)PayType.Max; ++i)
                    {
                        if (Type.Equals(((PayType)i).ToString()))
                        {
                            eProductionType = (PayType)i;
                            break;
                        }
                    }
                    ProductionRate = float.Parse(ele.GetAttribute("ProductionRate"));
                }
                else { }
            }
        }

        // set capacity related values
        public void SetCapacity(int gold, int elixir, int sulfur) {
            Capacity[(int)PayType.Gold] = gold;
            Capacity[(int)PayType.Elixir] = elixir;
            Capacity[(int)PayType.Sulfur] = sulfur;
        }

        // set storage capacity related values
        public void SetStorageCapacity(int gold, int elixir, int sulfur)
        {
            StorageCapacity[(int)PayType.Gold] = gold;
            StorageCapacity[(int)PayType.Elixir] = elixir;
            StorageCapacity[(int)PayType.Sulfur] = sulfur;
        }

        // set production related values
        public void SetProduction(PayType _eProductionType, int _ProductionRate) {
            eProductionType = _eProductionType;
            //ProductionRate 	= _ProductionRate / 3600.0f; //change time base hr -> sec
            ProductionRate = _ProductionRate / 60f; //change time base min -> sec
        }

        // set tower related values
        public void SetTower(float _DamagePerSecond, float _DamagePerShot, float _Range, float _AttackSpeed, DamageType _eDamageType, TargetMove _eTagetType) {
            DamagePerSecond = _DamagePerSecond;
            DamagePerShot = _DamagePerShot;
            Range = _Range;
            AttackSpeed = _AttackSpeed;
            eDamageType = _eDamageType;
            eTagetType = _eTagetType;
        }

        // set price icon and value
        public void PriceInfoApply(Image _PriceIcon, Text _Price) {

            if (BuildGoldPrice != 0) {
                _PriceIcon.sprite = TBDatabase.GetPayTypeIcon(PayType.Gold);
                _Price.text = BuildGoldPrice.ToString("#,##0");
            }
            else if (BuildElixirPrice != 0) {
                _PriceIcon.sprite = TBDatabase.GetPayTypeIcon(PayType.Elixir);
                _Price.text = BuildElixirPrice.ToString("#,##0");
            }
            else if (BuildSulfurPrice != 0){
                _PriceIcon.sprite = TBDatabase.GetPayTypeIcon(PayType.Sulfur);
                _Price.text = BuildSulfurPrice.ToString("#,##0");
            }
            else if (BuildGemPrice != 0) {
                _PriceIcon.sprite = TBDatabase.GetPayTypeIcon(PayType.Gem);
                _Price.text = BuildGemPrice.ToString("#,##0");
            }
            else { }
        }

        // check user has enough resource to build 
        public bool PriceInfoCheck(Text _Price) {
            bool Available = false;
            if (BuildGoldPrice != 0) {
                if (_Price != null) _Price.text = BuildGoldPrice.ToString("#,##0");
                if (SceneTown.Gold.Target() >= BuildGoldPrice) { if (_Price != null) _Price.color = Color.white; Available = true; }
                else { if (_Price != null) _Price.color = Color.red; Available = false; }
            }
            else if (BuildElixirPrice != 0) {
                if (_Price != null) _Price.text = BuildElixirPrice.ToString("#,##0");
                if (SceneTown.Elixir.Target() >= BuildElixirPrice) { if (_Price != null) _Price.color = Color.white; Available = true; }
                else { if (_Price != null) _Price.color = Color.red; Available = false; }
            }
            else if (BuildGemPrice != 0) {
                if (_Price != null) _Price.text = BuildGemPrice.ToString("#,##0");
                if (SceneTown.Gem.Target() >= BuildGemPrice) { if (_Price != null) _Price.color = Color.white; Available = true; }
                else { if (_Price != null) _Price.color = Color.red; Available = false; }
            }
            else if (BuildSulfurPrice != 0)
            {
                if (_Price != null) _Price.text = BuildSulfurPrice.ToString("#,##0");
                if (SceneTown.Sulfur.Target() >= BuildSulfurPrice) { if (_Price != null) _Price.color = Color.white; Available = true; }
                else { if (_Price != null) _Price.color = Color.red; Available = false; }
            }
            else { }

            return Available;
        }

        public void Save(XmlDocument d, XmlElement parent) {

            XmlElement ne = d.CreateElement("BuildingDef");
            ne.SetAttribute("HitPoint", HitPoint.ToString());
            string strPrice = BuildGoldPrice.ToString() + "," + BuildElixirPrice.ToString() + "," + BuildGemPrice.ToString();
            ne.SetAttribute("BuildPrice", strPrice);
            ne.SetAttribute("BuildTime", BuildTime.ToString());
            ne.SetAttribute("TownHallLevelRequired", TownHallLevelRequired.ToString());
            
            //capacity
            if ((Capacity[(int)PayType.Gold] != 0) || (Capacity[(int)PayType.Elixir] != 0) || (Capacity[(int)PayType.Sulfur] != 0)) {
                XmlElement ne2 = d.CreateElement("Capacity");
                ne2.SetAttribute("Gold", Capacity[(int)PayType.Gold].ToString());
                ne2.SetAttribute("Elixir", Capacity[(int)PayType.Elixir].ToString());
                ne2.SetAttribute("Sulfur", Capacity[(int)PayType.Sulfur].ToString());
                ne.AppendChild(ne2);
            }//capacity
            if ((StorageCapacity[(int)PayType.Gold] != 0) || (StorageCapacity[(int)PayType.Elixir] != 0) || (StorageCapacity[(int)PayType.Sulfur] != 0))
            {
                XmlElement ne2 = d.CreateElement("StorageCapacity");
                ne2.SetAttribute("Gold", StorageCapacity[(int)PayType.Gold].ToString());
                ne2.SetAttribute("Elixir", StorageCapacity[(int)PayType.Elixir].ToString());
                ne2.SetAttribute("Sulfur", StorageCapacity[(int)PayType.Sulfur].ToString());
                ne.AppendChild(ne2);
            }

            //production
            if (eProductionType != PayType.None) {
                XmlElement ne2 = d.CreateElement("Production");
                ne2.SetAttribute("Type", eProductionType.ToString());
                ne2.SetAttribute("ProductionRate", ProductionRate.ToString());
                ne.AppendChild(ne2);
            }

            parent.AppendChild(ne);
        }

    }

    #endregion

    #region ======== BuildingType Class ===========
    // class for building category type
    [System.Serializable]
    public class BuildingType {
        public int ID;
        public string Name;     // name og the building
        public string Info;     // description of the building
        public int TileX;       // needed tile size with
        public int TileZ;       // needed tile size height
        public int LevelMax;
        public int Category;
        public int[] MaxCount;//MaxCountByTownHallLevel
        public List<BuildingDef> Defs = new List<BuildingDef>();

        public BuildingType(int _ID, string _Name, string _Info, int _TileX, int _TileZ, int _LevelMax, int _Category, string _MaxCount) {
            ID = _ID;
            Name = _Name;
            Info = _Info;
            TileX = _TileX;
            TileZ = _TileZ;
            LevelMax = _LevelMax;
            Category = _Category;

            string[] Sub = _MaxCount.Split(',');
            if (Sub.Length > 0) {
                MaxCount = new int[Sub.Length];
                for (int i = 0; i < Sub.Length; ++i) {
                    MaxCount[i] = int.Parse(Sub[i]);
                }
            }
        }

        public BuildingType(XmlElement e) {

            ID = int.Parse(e.GetAttribute("ID"));
            Name = e.GetAttribute("Name");
            Info = e.GetAttribute("Info");
            TileX = int.Parse(e.GetAttribute("TileX"));
            TileZ = int.Parse(e.GetAttribute("TileZ"));
            LevelMax = int.Parse(e.GetAttribute("LevelMax"));
            Category = int.Parse(e.GetAttribute("Category"));
            string strMaxCount = e.GetAttribute("MaxCount");

            string[] Sub = strMaxCount.Split(',');
            if (Sub.Length > 0) {
                MaxCount = new int[Sub.Length];
                for (int i = 0; i < Sub.Length; ++i) {
                    MaxCount[i] = int.Parse(Sub[i]);
                }
            }

            Defs.Clear();
            XmlNodeList list = e.ChildNodes;
            foreach (XmlElement ele in list) {
                if (ele.Name == "BuildingDef")
                    Add(new BuildingDef(ele));
            }
        }


        public void Add(BuildingDef def) {
            Defs.Add(def);
            // set mesh prefab to each Building definition
            def.prefab = Resources.Load("Prefabs/Building/" + Name + "_" + Defs.Count.ToString()) as GameObject;
        }

        public BuildingDef GetDefLast() {
            return Defs[Defs.Count - 1];
        }

        public BuildingDef GetDefine(int level) {
            return ((level < 1) || (Defs.Count < level)) ? null : Defs[level - 1];
        }

        public int GetMoreBuildTownLevel(int CurrentCount) {
            for (int i = 0; i < MaxCount.Length; ++i) {
                if (MaxCount[i] > CurrentCount)
                    return i + 1;
            }
            return -1;
        }

        public void Save(XmlDocument d, XmlElement parent) {

            string strMaxCount = "";
            for (int i = 0; i < MaxCount.Length; ++i) {
                strMaxCount += MaxCount[i].ToString();
                if (i != MaxCount.Length - 1) strMaxCount += ",";
            }

            XmlElement ne = d.CreateElement("BuildingType");
            ne.SetAttribute("ID", ID.ToString());
            ne.SetAttribute("Name", Name);
            ne.SetAttribute("Info", Info);
            ne.SetAttribute("TileX", TileX.ToString());
            ne.SetAttribute("TileZ", TileZ.ToString());
            ne.SetAttribute("LevelMax", LevelMax.ToString());
            ne.SetAttribute("Category", Category.ToString());
            ne.SetAttribute("MaxCount", strMaxCount);

            parent.AppendChild(ne);

            for (int i = 0; i < Defs.Count; ++i) {
                Defs[i].Save(d, ne);
            }
        }

    }

    #endregion


    [System.Serializable]
    public class TBDatabase : MonoBehaviour {

        public static TBDatabase instance;

        private int ConfigVersion = 1;
        private string dbFilename = "Database.dat";
        public const int MAX_LEVEL = 230;

        public AudioClip[] audioClip;

        private List<InAppItem> InApps = new List<InAppItem>();
        private int[] LevelExp = new int[MAX_LEVEL + 1];
        private int[] LevelExpTotal = new int[MAX_LEVEL + 1];
        private List<BuildingType> Buildings = new List<BuildingType>();
        private List<ArmyType> Armies = new List<ArmyType>();




        void Awake() {
            instance = this;

            //add InApp purchase item
            InApps.Add(new InAppItem("Pile of Diamonds", 500, "$4.99"));
            InApps.Add(new InAppItem("Pouch of Diamonds", 1200, "$9.99"));
            InApps.Add(new InAppItem("Bag of Diamonds", 2500, "$19.99"));
            InApps.Add(new InAppItem("Box of Diamonds", 6500, "$49.99"));
            InApps.Add(new InAppItem("Crate of Diamonds", 14000, "$99.99"));

            // set experience values to each level OLD
            /* for(int Level=0 ; Level <= MAX_LEVEL ; ++Level) {
                 LevelExp[Level] = Level * 50 + Mathf.Max(0, (Level - 199) * 450);
                 LevelExpTotal[Level] = Level * (Level - 1) * 25;
                 Debug.Log ("Level "+Level.ToString ()+" - Exp: "+LevelExp[Level].ToString ()+" | ExpTotal: "+LevelExpTotal[Level].ToString ());
             }*/
            LevelExp[0] = 0;
            LevelExp[1] = 100;
            LevelExp[2] = 200;
            LevelExp[3] = 600;
            LevelExp[4] = 2000;
            LevelExp[5] = 6500;
            LevelExp[6] = 20000;
            LevelExp[7] = 42200;
            LevelExp[8] = 112500;
            LevelExp[9] = 216563;
            LevelExp[10] = 540000;
            LevelExp[11] = 1300250;
            LevelExp[12] = 3500000;

            LevelExpTotal[0] = 0;
            for (int Level = 1; Level <= MAX_LEVEL; ++Level)
            {
                LevelExpTotal[Level] = LevelExpTotal[Level - 1] + LevelExp[Level - 1];
                if(Level <12 ) Debug.Log("Level " + Level.ToString() + " - Exp: " + LevelExp[Level].ToString() + " | ExpTotal: " + LevelExpTotal[Level].ToString());
                if (Level > 12) LevelExp[Level] = 5000000;

            }

            //LevelExpTotal[1] = 100;
            //GOLD (DEMON OR HELLFIRE) GENERATOR
            /* {

                 BuildingType bt = new BuildingType(3, "Fire Mine", "", 3, 3, 12, 0, "1,2,3,4,5,6,6,6,6,7");
                 //for (int i = 0; i <= GLOBALS.s.BUILDING_MAX_LEVEL; i++) { 
                     { BuildingDef bd = new BuildingDef(400, 0, 150, 0, 10, 1); bd.SetCapacity(0, 0); bd.SetProduction(PayType.Gold, 100); bt.Add(bd); }
                     { BuildingDef bd = new BuildingDef(440, 0, 300, 0, 60, 1); bd.SetCapacity(0, 0); bd.SetProduction(PayType.Gold, 120); bt.Add(bd); }
                     { BuildingDef bd = new BuildingDef(480, 0, 700, 0, 900, 2); bd.SetCapacity(0, 0); bd.SetProduction(PayType.Gold, 180); bt.Add(bd); }
                     { BuildingDef bd = new BuildingDef(480, 0, 700, 0, 900, 2); bd.SetCapacity(0, 0); bd.SetProduction(PayType.Gold, 250); bt.Add(bd); }
                     { BuildingDef bd = new BuildingDef(480, 0, 700, 0, 900, 2); bd.SetCapacity(0, 0); bd.SetProduction(PayType.Gold, 550); bt.Add(bd); }
                     { BuildingDef bd = new BuildingDef(480, 0, 700, 0, 900, 2); bd.SetCapacity(0, 0); bd.SetProduction(PayType.Gold, 850); bt.Add(bd); }
                     { BuildingDef bd = new BuildingDef(480, 0, 700, 0, 900, 2); bd.SetCapacity(0, 0); bd.SetProduction(PayType.Gold, 1300); bt.Add(bd); }
                     { BuildingDef bd = new BuildingDef(480, 0, 700, 0, 900, 2); bd.SetCapacity(0, 0); bd.SetProduction(PayType.Gold, 3600); bt.Add(bd); }
                     { BuildingDef bd = new BuildingDef(480, 0, 700, 0, 900, 2); bd.SetCapacity(0, 0); bd.SetProduction(PayType.Gold, 9700); bt.Add(bd); }
                     { BuildingDef bd = new BuildingDef(480, 0, 700, 0, 900, 2); bd.SetCapacity(0, 0); bd.SetProduction(PayType.Gold, 26000); bt.Add(bd); }
                     { BuildingDef bd = new BuildingDef(480, 0, 700, 0, 900, 2); bd.SetCapacity(0, 0); bd.SetProduction(PayType.Gold, 71000); bt.Add(bd); }
                     { BuildingDef bd = new BuildingDef(480, 0, 700, 0, 900, 2); bd.SetCapacity(0, 0); bd.SetProduction(PayType.Gold, 191000); bt.Add(bd); }
                // }
            //{ BuildingDef bd = new BuildingDef ( 520,      0,    1400, 0,   3600, 2);	bd.SetCapacity(  2500,0);	bd.SetProduction(PayType.Gold,  800);	bt.Add(bd); }
            //{ BuildingDef bd = new BuildingDef ( 560,      0,    3000, 0,   7200, 3);	bd.SetCapacity( 10000,0);	bd.SetProduction(PayType.Gold, 1000);	bt.Add(bd); }
            //{ BuildingDef bd = new BuildingDef ( 600,      0,    7000, 0,  21600, 3);	bd.SetCapacity( 20000,0);	bd.SetProduction(PayType.Gold, 1300);	bt.Add(bd); }
            //{ BuildingDef bd = new BuildingDef ( 640,      0,   14000, 0,  43200, 4);	bd.SetCapacity( 30000,0);	bd.SetProduction(PayType.Gold, 1600);	bt.Add(bd); }
            //{ BuildingDef bd = new BuildingDef ( 680,      0,   28000, 0,  86400, 4);	bd.SetCapacity( 50000,0);	bd.SetProduction(PayType.Gold, 1900);	bt.Add(bd); }
            //{ BuildingDef bd = new BuildingDef ( 720,      0,   56000, 0, 172800, 5);	bd.SetCapacity( 75000,0);	bd.SetProduction(PayType.Gold, 2200);	bt.Add(bd); }
            //{ BuildingDef bd = new BuildingDef ( 780,      0,   84000, 0, 259200, 5);	bd.SetCapacity(100000,0);	bd.SetProduction(PayType.Gold, 2500);	bt.Add(bd); }
            //{ BuildingDef bd = new BuildingDef ( 860,      0,  168000, 0, 345600, 7);	bd.SetCapacity(150000,0);	bd.SetProduction(PayType.Gold, 3000);	bt.Add(bd); }
            //{ BuildingDef bd = new BuildingDef ( 960,      0,  336000, 0, 432000, 8);	bd.SetCapacity(200000,0);	bd.SetProduction(PayType.Gold, 3500);	bt.Add(bd); }
            Buildings.Add(bt);
             }*/



            //0-Demon Palace
            {
                BuildingType bt = new BuildingType(0, "Demon Palace", "Your personal Palace", 6, 6, GLOBALS.s.BUILDING_MAX_LEVEL, 0, "1,1,1,1,1,1,1,1,1,1");
                { BuildingDef bd = new BuildingDef(1500, 0, 0, 1, 0, 0, 1); bd.SetStorageCapacity(1000, 0, 0); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(1600, 100, 0, 1, 10, 1, 1); bd.SetStorageCapacity(4000, 0, 0); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(1850, 300, 0, 2, 30, 2, 2); bd.SetStorageCapacity(12000, 0, 0); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(2100, 600, 0, 3, 60, 3, 3); bd.SetStorageCapacity(27000, 0, 0); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(2400, 1200, 0, 10, 172800, 4, 4); bd.SetStorageCapacity(63000, 0, 0); bt.Add(bd); }
                Buildings.Add(bt);
            }

            #region Hut and Wall
            //1-Hut
            {
                BuildingType bt = new BuildingType(1, "Hut", "", 2, 2, 1, 0, "0,0,0,0,0,0,0,0,0,0");
                { BuildingDef bd = new BuildingDef(250, 0, 0, 0,0, 0, 0); bt.Add(bd); }
                Buildings.Add(bt);
            }

            //2-Wall
            {
                BuildingType bt = new BuildingType(2, "Wall", "", 1, 1, 11, 0, "0,25,50,75,100,125,175,225,250,250");
                { BuildingDef bd = new BuildingDef(300, 50, 0, 0, 0, 0, 2); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(500, 1000, 0, 0, 0, 0, 2); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(700, 5000, 0, 0, 0, 0, 3); bt.Add(bd); }
                //{ BuildingDef bd = new BuildingDef ( 900,  10000,       0, 0,      0, 4); bt.Add(bd); }
                //{ BuildingDef bd = new BuildingDef (1400,  30000,       0, 0,      0, 5); bt.Add(bd); }
                //{ BuildingDef bd = new BuildingDef (2000,  75000,       0, 0,      0, 6); bt.Add(bd); }
                //{ BuildingDef bd = new BuildingDef (2500, 200000,       0, 0,      0, 7); bt.Add(bd); }
                //{ BuildingDef bd = new BuildingDef (3000, 500000,       0, 0,      0, 8); bt.Add(bd); }
                //{ BuildingDef bd = new BuildingDef (4000,1000000, 1000000, 0,      0, 9); bt.Add(bd); }
                //{ BuildingDef bd = new BuildingDef (5500,3000000, 3000000, 0,      0, 9); bt.Add(bd); }
                //{ BuildingDef bd = new BuildingDef (7000,4000000, 4000000, 0,      0,10); bt.Add(bd); }
                Buildings.Add(bt);
            }
            #endregion

            //3 - Gold (DEMON OR HELLFIRE) GENERATOR
            {
                BuildingType bt = new BuildingType(3, "Fire Mine", "", 6, 6, GLOBALS.s.BUILDING_MAX_LEVEL, 0, GLOBALS.s.PUNISHER_COUNT_EVOLUTION);
                //for (int i = 0; i <= GLOBALS.s.BUILDING_MAX_LEVEL; i++) { 
                { BuildingDef bd = new BuildingDef(400, 100, 0, 1, 5, 0, 1); bd.SetCapacity(100 * 12, 0, 0); bd.SetProduction(PayType.Gold, 60); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(440, 200, 0, 1, 10, 0, 1); bd.SetCapacity(120 * 24, 0, 0); bd.SetProduction(PayType.Gold, 120); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 500, 0, 2, 30, 0, 2); bd.SetCapacity(180 * 36, 0, 0); bd.SetProduction(PayType.Gold, 180); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 1000, 0, 3, 60, 0,  2); bd.SetCapacity(250 * 48, 0, 0); bd.SetProduction(PayType.Gold, 250); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 1600, 0, 4, 2 * 60, 0,  2); bd.SetCapacity(550 * 60, 0, 0); bd.SetProduction(PayType.Gold, 550); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 4400, 0, 5, 5 * 60, 0, 3); bd.SetCapacity(850 * 80, 0, 0); bd.SetProduction(PayType.Gold, 850); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 11000, 0, 10, 10 * 60, 0, 3); bd.SetCapacity(1300 * 100, 0, 0); bd.SetProduction(PayType.Gold, 1300); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 31600, 0, 20, 15 * 60, 0, 4); bd.SetCapacity(3600 * 120, 0, 0); bd.SetProduction(PayType.Gold, 3600); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 84000, 0, 40, 20 * 60, 0, 4); bd.SetCapacity(9700 * 150, 0, 0); bd.SetProduction(PayType.Gold, 9700); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 224000, 0, 50, 30 * 60, 0, 5); bd.SetCapacity(26000 * 180, 0, 0); bd.SetProduction(PayType.Gold, 26000); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 600000, 0, 75, 45 * 60, 0, 5); bd.SetCapacity(71000 * 210, 0, 0); bd.SetProduction(PayType.Gold, 71000); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 1600200, 0, 100, 1 * 60 * 60, 0, 6); bd.SetCapacity(191000 * 240, 0, 0); bd.SetProduction(PayType.Gold, 191000); bt.Add(bd); }
                Buildings.Add(bt);
            }

            //4 - Hell's Gate (Old Elixir Collector)
            {
                BuildingType bt = new BuildingType(4, "Hells Gate", "", 6, 6, GLOBALS.s.BUILDING_MAX_LEVEL, 0, GLOBALS.s.PUNISHER_COUNT_EVOLUTION);
                //for (int i = 0; i <= GLOBALS.s.BUILDING_MAX_LEVEL; i++) { 
                { BuildingDef bd = new BuildingDef(400, 100, 0, 0, 5, 0, 1); bd.SetCapacity(0, 100 * 12, 0); bd.SetProduction(PayType.Elixir, 100); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(440, 300, 0, 1, 10, 0, 1); bd.SetCapacity(0, 150 * 24, 0); bd.SetProduction(PayType.Elixir, 150); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 600, 0, 2, 30, 0, 2); bd.SetCapacity(0, 8000, 0); bd.SetProduction(PayType.Elixir, 200); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 1100, 0, 3, 60, 0, 2); bd.SetCapacity(0, 14400, 0); bd.SetProduction(PayType.Elixir, 250); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 1600, 0, 4, 2 * 60, 0, 2); bd.SetCapacity(0, 25200, 0); bd.SetProduction(PayType.Elixir, 300); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 4400, 0, 5, 5 * 60, 0, 3); bd.SetCapacity(0, 50000, 0); bd.SetProduction(PayType.Elixir, 350); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 11000, 0, 6, 10 * 60, 0, 3); bd.SetCapacity(0, 85000, 0); bd.SetProduction(PayType.Elixir, 400); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 31600, 0, 7, 15 * 60, 0, 4); bd.SetCapacity(0, 121500, 0); bd.SetProduction(PayType.Elixir, 450); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 84000, 0, 8, 20 * 60, 0, 4); bd.SetCapacity(0, 180000, 0); bd.SetProduction(PayType.Elixir, 500); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 224000, 0, 9, 30 * 60, 0, 5); bd.SetCapacity(0, 1115750, 0); bd.SetProduction(PayType.Elixir, 600); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 600000, 0, 10, 45 * 60, 0, 5); bd.SetCapacity(0, 1500000, 0); bd.SetProduction(PayType.Elixir, 650); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 1600200, 0, 11, 1 * 60 * 60, 0, 6); bd.SetCapacity(0, 1850000, 0); bd.SetProduction(PayType.Elixir, 700); bt.Add(bd); }
                Buildings.Add(bt);
            }
            /*
            {
                BuildingType bt = new BuildingType(4, "Hells Gate", "", 3, 3, 12, 0, "1,2,3,4,5,6,6,6,6,7");
                { BuildingDef bd = new BuildingDef ( 400,    150, 0, 0,     10, 1);	bd.SetCapacity(0,   500);	bd.SetProduction(PayType.Elixir,  200);	bt.Add(bd); }
                { BuildingDef bd = new BuildingDef ( 440,    300, 0, 0,     60, 1);	bd.SetCapacity(0,  1000);	bd.SetProduction(PayType.Elixir,  400);	bt.Add(bd); }
                { BuildingDef bd = new BuildingDef ( 480,    700, 0, 0,    900, 2);	bd.SetCapacity(0,  1500);	bd.SetProduction(PayType.Elixir,  600);	bt.Add(bd); }
                Buildings.Add (bt);
            }*/

            #region ========== City Builder Buildings (not used) ==============
            //5-Gold Storage
            {
                BuildingType bt = new BuildingType(5, "Gold Storage", "", 3, 3, 11, 0, "1,1,2,2,2,2,2,3,4,4");
                { BuildingDef bd = new BuildingDef(400, 0, 300, 0,0,10, 1); bd.SetCapacity(1000, 0, 0); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(600, 0, 750, 0,0,1800, 2); bd.SetCapacity(3000, 0, 0); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(800, 0, 1500, 0,0,3600, 2); bd.SetCapacity(61000, 0, 0); bt.Add(bd); }
                Buildings.Add(bt);
            }

            //6-Elixir Storage
            {
                BuildingType bt = new BuildingType(6, "Elixir Storage", "", 3, 3, 11, 0, "1,1,2,2,2,2,2,3,4,4");
                { BuildingDef bd = new BuildingDef(400, 300, 0, 0,0, 10, 1); bd.SetCapacity(0, 1000, 0); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(600, 750, 0, 0,0, 1800, 2); bd.SetCapacity(0, 3000, 0); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(800, 1500, 0, 0,0, 3600, 2); bd.SetCapacity(0, 6000, 0); bt.Add(bd); }
                Buildings.Add(bt);
            }

            //7-Barracks
            {
                BuildingType bt = new BuildingType(7, "Barrack", "", 3, 3, 10, 0, "1,2,2,3,3,3,4,4,4,4");
                { BuildingDef bd = new BuildingDef(250, 0, 200, 0, 0,10, 1); bt.Add(bd); } //20
                Buildings.Add(bt);
            }

            //8-Army Camp
            {
                BuildingType bt = new BuildingType(8, "Army Camp", "", 4, 4, 8, 0, "1,1,2,2,3,3,4,4,4,4");
                { BuildingDef bd = new BuildingDef(250, 0, 250, 0, 0,300, 1); bt.Add(bd); } //20
                Buildings.Add(bt);
            }

            //9-Army Camp
            {
                BuildingType bt = new BuildingType(9, "Cannon", "", 4, 4, 8, 0, "1,1,2,2,3,3,4,4,4,4");
                { BuildingDef bd = new BuildingDef(250, 0, 250, 0, 0,300, 1); bt.Add(bd); } //20
                Buildings.Add(bt);
            }
            //10-Army Camp
            {
                BuildingType bt = new BuildingType(10, "Cannon2", "", 4, 4, 8, 0, "1,1,2,2,3,3,4,4,4,4");
                { BuildingDef bd = new BuildingDef(250, 0, 250, 0, 0,300, 1); bt.Add(bd); } //20
                Buildings.Add(bt);
            }
            #endregion

            #region ========== PUNISHER BUILDINGS ===============
            //11 - Bad Food Restaurant
            {
                BuildingType bt = new BuildingType(11, "Bad Food Restaurant", "", 3, 2, GLOBALS.s.BUILDING_MAX_LEVEL, 0, GLOBALS.s.PUNISHER_COUNT_EVOLUTION);
                { BuildingDef bd = new BuildingDef(250, 125, 0, 1,0, 5, 0, 1); bd.SetStorageCapacity(0, 175, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 250, 0, 2,0, 30, 0, 2); bd.SetStorageCapacity(0, 350, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 650, 0, 3,0, 60, 0, 2); bd.SetStorageCapacity(0, 600, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 1250, 0, 4,0, 2 * 60, 0, 3); bd.SetStorageCapacity(0, 750, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 3300, 0, 5,0, 3 * 60, 0, 4); bd.SetStorageCapacity(0, 1000, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 6,0, 5 * 60, 0, 5); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                Buildings.Add(bt);
            }

            //12 - Cauldron
            {
                BuildingType bt = new BuildingType(12, "Cauldron", "", 3, 3, GLOBALS.s.BUILDING_MAX_LEVEL, 0, GLOBALS.s.PUNISHER_COUNT_EVOLUTION);
                { BuildingDef bd = new BuildingDef(250, 150, 0, 1,0, 5, 0, 1); bd.SetStorageCapacity(0, 200, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 400, 0, 2,0, 30, 0, 2); bd.SetStorageCapacity(0, 450, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 900, 0, 3,0, 60, 0, 2); bd.SetStorageCapacity(0, 700, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 1250, 0, 4,0, 2 * 60, 0, 3); bd.SetStorageCapacity(0, 1000, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 3300, 0, 5,0, 3 * 60, 0, 4); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 6,0, 5 * 60, 0, 5); bd.SetStorageCapacity(0, 1500, 0); bt.Add(bd); } //20
                Buildings.Add(bt);
            }

            //13 - Sisyphus Hill
            {
                BuildingType bt = new BuildingType(13, "Sisyphus Hill", "", 4, 4, GLOBALS.s.BUILDING_MAX_LEVEL, 0, GLOBALS.s.PUNISHER_COUNT_EVOLUTION);
                { BuildingDef bd = new BuildingDef(250, 200, 0, 1, 0,10, 0, 3); bd.SetStorageCapacity(0, 250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 500, 0, 2, 0,40, 0, 3); bd.SetStorageCapacity(0, 500, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 1050, 0, 3,0, 60, 0, 3); bd.SetStorageCapacity(0, 800, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 1250, 0, 4,0, 2 * 60, 0, 3); bd.SetStorageCapacity(0, 750, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 3300, 0, 5,0, 3 * 60, 0, 4); bd.SetStorageCapacity(0, 1000, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 6,0, 5 * 60, 0, 5); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                Buildings.Add(bt);
            }
            //14 - Toaster Oven
            {
                BuildingType bt = new BuildingType(14, "Toaster Oven", "", 3, 2, GLOBALS.s.BUILDING_MAX_LEVEL, 0, GLOBALS.s.PUNISHER_COUNT_EVOLUTION);
                { BuildingDef bd = new BuildingDef(250, 100, 0, 1, 0,5, 0, 4); bd.SetStorageCapacity(0, 300, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 300, 0, 2, 0,30, 0, 4); bd.SetStorageCapacity(0, 600, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 750, 0, 3, 0,60, 0, 4); bd.SetStorageCapacity(0, 1000, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 1250, 0, 4,0, 2 * 60, 0, 4); bd.SetStorageCapacity(0, 1500, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 3300, 0, 5,0, 3 * 60, 0, 4); bd.SetStorageCapacity(0, 1000, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 6,0, 5 * 60, 0, 5); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                Buildings.Add(bt);
            }
            //15 - Justin Biba
            {
                BuildingType bt = new BuildingType(15, "Pop Music", "", 3, 2, GLOBALS.s.BUILDING_MAX_LEVEL, 0, GLOBALS.s.PUNISHER_COUNT_EVOLUTION);
                { BuildingDef bd = new BuildingDef(250, 100, 0, 0, 0,5, 0, 1); bd.SetStorageCapacity(0, 150, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 200, 0, 2, 0,30, 0, 2); bd.SetStorageCapacity(0, 300, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 750, 0, 3, 0,60, 0, 2); bd.SetStorageCapacity(0, 500, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 1250, 0, 4,0, 2 * 60, 0, 3); bd.SetStorageCapacity(0, 750, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 3300, 0, 5,0, 3 * 60, 0, 4); bd.SetStorageCapacity(0, 1000, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 6,0, 5 * 60, 0, 5); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                Buildings.Add(bt);
            }
            //16 - Fire Pool
            {
                BuildingType bt = new BuildingType(16, "Call Center", "", 4, 3, GLOBALS.s.BUILDING_MAX_LEVEL, 0, GLOBALS.s.PUNISHER_COUNT_EVOLUTION);
                { BuildingDef bd = new BuildingDef(250, 100, 0, 1, 0,5, 0, 5); bd.SetStorageCapacity(0, 100, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 300, 0, 2, 0,30, 0, 5); bd.SetStorageCapacity(0, 250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 750, 0, 3, 0,60, 0, 5); bd.SetStorageCapacity(0, 500, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 1250, 0, 4,0, 2 * 60, 0, 5); bd.SetStorageCapacity(0, 750, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 3300, 0, 5,0, 3 * 60, 0, 5); bd.SetStorageCapacity(0, 1000, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 6,0, 5 * 60, 0, 5); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                Buildings.Add(bt);
            }
            //17 - Office
            {
                BuildingType bt = new BuildingType(17, "Office", "", 4, 3, GLOBALS.s.BUILDING_MAX_LEVEL, 0, GLOBALS.s.PUNISHER_COUNT_EVOLUTION);
                { BuildingDef bd = new BuildingDef(250, 100, 0, 1, 0,5, 0, 6); bd.SetStorageCapacity(0, 100, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 300, 0, 2, 0,30, 0, 6); bd.SetStorageCapacity(0, 250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 750, 0, 3, 0,60, 0, 6); bd.SetStorageCapacity(0, 500, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 1250, 0, 4,0, 2 * 60, 0, 6); bd.SetStorageCapacity(0, 750, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 3300, 0, 5,0, 3 * 60, 0, 6); bd.SetStorageCapacity(0, 1000, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 6,0, 5 * 60, 0, 6); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                Buildings.Add(bt);
            }

            #endregion

            #region ================ RESEARCH BUILDINGS (inc soul production) =================
            //18 - Law School
            {
                BuildingType bt = new BuildingType(18, "Law School", "", 7, 6, 8, 0, "1,1,2,2,3,3,4,4,4,4");
                { BuildingDef bd = new BuildingDef(250, 100, 0, 0, 5, 1, 3, 5); bd.SetStorageCapacity(0, 100, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 300, 0, 0, 10, 1, 3, 10); bd.SetStorageCapacity(0, 250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 750, 0, 0, 30, 1, 3, 20); bd.SetStorageCapacity(0, 500, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 1250, 0, 0, 60, 1, 3, 40); bd.SetStorageCapacity(0, 750, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 3300, 0, 0, 2 * 60, 0, 4, 60); bd.SetStorageCapacity(0, 1000, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 0, 5 * 60, 0, 5, 80); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 0, 10 * 60, 0, 6, 100); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 0, 15 * 60, 0, 6, 125); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 0, 20 * 60, 0, 7, 150); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                Buildings.Add(bt);
            }
            //19 - Statue
            {
                BuildingType bt = new BuildingType(19, "Statue", "", 4, 4, 8, 0, "1,1,2,2,3,3,4,4,4,4");
                { BuildingDef bd = new BuildingDef(250, 100, 0, 0, 5, 1, 3, 5); bd.SetStorageCapacity(0, 100, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 300, 0, 0, 10, 1, 3, 10); bd.SetStorageCapacity(0, 250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 750, 0, 0, 30, 1, 3, 20); bd.SetStorageCapacity(0, 500, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 1250, 0, 0, 60, 1, 3, 40); bd.SetStorageCapacity(0, 750, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 3300, 0, 0, 2 * 60, 0, 4, 60); bd.SetStorageCapacity(0, 1000, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 0, 5 * 60, 0, 5, 80); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 0, 10 * 60, 0, 6, 100); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 0, 15 * 60, 0, 6, 125); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 0, 20 * 60, 0, 7, 150); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                Buildings.Add(bt);
            }
            //20 - Temple
            {
                BuildingType bt = new BuildingType(20, "Temple", "", 4, 4, 8, 0, "1,1,2,2,3,3,4,4,4,4");
                { BuildingDef bd = new BuildingDef(250, 100, 0, 0, 5, 1, 3, 5); bd.SetStorageCapacity(0, 100, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 300, 0, 0, 10, 1, 3, 10); bd.SetStorageCapacity(0, 250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 750, 0, 0, 30, 1, 3, 20); bd.SetStorageCapacity(0, 500, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 1250, 0, 0, 60, 1, 3, 40); bd.SetStorageCapacity(0, 750, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 3300, 0, 0, 2 * 60, 0, 4, 60); bd.SetStorageCapacity(0, 1000, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 0, 5 * 60, 0, 5, 80); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 0, 10 * 60, 0, 6, 100); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 0, 15 * 60, 0, 6, 125); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 0, 20 * 60, 0, 7, 150); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                Buildings.Add(bt);
            }
            //21 - Metal Bands
            {
                BuildingType bt = new BuildingType(21, "Metal Bands", "", 4, 4, 8, 0, "1,1,2,2,3,3,4,4,4,4");
                { BuildingDef bd = new BuildingDef(250, 100, 0, 0, 5, 1, 3, 5); bd.SetStorageCapacity(0, 100, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 300, 0, 0, 10, 1, 3, 10); bd.SetStorageCapacity(0, 250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 750, 0, 0, 30, 1, 3, 20); bd.SetStorageCapacity(0, 500, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 1250, 0, 0, 60, 1, 3, 40); bd.SetStorageCapacity(0, 750, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 3300, 0, 0, 2 * 60, 0, 4, 60); bd.SetStorageCapacity(0, 1000, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 0, 5 * 60, 0, 5, 80); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 0, 10 * 60, 0, 6, 100); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 0, 15 * 60, 0, 6, 125); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                { BuildingDef bd = new BuildingDef(250, 8900, 0, 0, 20 * 60, 0, 7, 150); bd.SetStorageCapacity(0, 1250, 0); bt.Add(bd); } //20
                Buildings.Add(bt);
            }



            //22 - FIRE) GENERATOR Refinery
            {
                BuildingType bt = new BuildingType(22, "Refinery", "", 3, 3, GLOBALS.s.BUILDING_MAX_LEVEL, 0, GLOBALS.s.PUNISHER_COUNT_EVOLUTION);
                //for (int i = 0; i <= GLOBALS.s.BUILDING_MAX_LEVEL; i++) { 
                { BuildingDef bd = new BuildingDef(400, 100, 0, 0, 5, 1, 6); bd.SetCapacity(100 * 12, 0, 0); bd.SetProduction(PayType.Gold, 100); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(440, 200, 0, 0, 10, 1,7); bd.SetCapacity(120 * 24, 0, 0); bd.SetProduction(PayType.Gold, 120); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 500, 0, 0, 30, 2, 8); bd.SetCapacity(180 * 36, 0, 0); bd.SetProduction(PayType.Gold, 180); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 1000, 1000, 0, 60, 2, 9); bd.SetCapacity(250 * 48, 0, 0); bd.SetProduction(PayType.Gold, 250); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 1600, 700, 0, 2 * 60, 2, 10); bd.SetCapacity(550 * 60, 0, 0); bd.SetProduction(PayType.Gold, 550); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 4400, 700, 0, 5 * 60, 3, 11); bd.SetCapacity(850 * 80, 0, 0); bd.SetProduction(PayType.Gold, 850); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 11000, 700, 0, 10 * 60, 3, 12); bd.SetCapacity(1300 * 100, 0, 0); bd.SetProduction(PayType.Gold, 1300); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 31600, 700, 0, 15 * 60, 4, 13); bd.SetCapacity(3600 * 120, 0, 0); bd.SetProduction(PayType.Gold, 3600); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 84000, 700, 0, 20 * 60, 4, 14); bd.SetCapacity(9700 * 150, 0, 0); bd.SetProduction(PayType.Gold, 9700); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 224000, 700, 0, 30 * 60, 5, 15); bd.SetCapacity(26000 * 180, 0, 0); bd.SetProduction(PayType.Gold, 26000); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 600000, 700, 0, 45 * 60, 5, 16); bd.SetCapacity(71000 * 210, 0, 0); bd.SetProduction(PayType.Gold, 71000); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 1600200, 700, 0, 1 * 60 * 60, 6, 17); bd.SetCapacity(191000 * 240, 0, 0); bd.SetProduction(PayType.Gold, 191000); bt.Add(bd); }
                Buildings.Add(bt);
            }
            //23 - Fire Factory
            {
                BuildingType bt = new BuildingType(23, "Factory", "", 3, 3, GLOBALS.s.BUILDING_MAX_LEVEL, 0, GLOBALS.s.PUNISHER_COUNT_EVOLUTION);
                //for (int i = 0; i <= GLOBALS.s.BUILDING_MAX_LEVEL; i++) { 
                { BuildingDef bd = new BuildingDef(400, 100, 0, 0, 5, 1,5); bd.SetCapacity(100 * 12, 0, 0); bd.SetProduction(PayType.Gold, 100); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(440, 200, 0, 0, 10, 1,6); bd.SetCapacity(120 * 24, 0, 0); bd.SetProduction(PayType.Gold, 120); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 500, 0, 0, 30, 2,6); bd.SetCapacity(180 * 36, 0, 0); bd.SetProduction(PayType.Gold, 180); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 1000, 1000, 0, 60, 2,7); bd.SetCapacity(250 * 48, 0, 0); bd.SetProduction(PayType.Gold, 250); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 1600, 700, 0, 2 * 60, 2,8); bd.SetCapacity(550 * 60, 0, 0); bd.SetProduction(PayType.Gold, 550); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 4400, 700, 0, 5 * 60, 3,9); bd.SetCapacity(850 * 80, 0, 0); bd.SetProduction(PayType.Gold, 850); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 11000, 700, 0, 10 * 60, 3,10); bd.SetCapacity(1300 * 100, 0, 0); bd.SetProduction(PayType.Gold, 1300); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 31600, 700, 0, 15 * 60, 4,11); bd.SetCapacity(3600 * 120, 0, 0); bd.SetProduction(PayType.Gold, 3600); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 84000, 700, 0, 20 * 60, 4,12); bd.SetCapacity(9700 * 150, 0, 0); bd.SetProduction(PayType.Gold, 9700); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 224000, 700, 0, 30 * 60, 5,13); bd.SetCapacity(26000 * 180, 0, 0); bd.SetProduction(PayType.Gold, 26000); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 600000, 700, 0, 45 * 60, 5,14); bd.SetCapacity(71000 * 210, 0, 0); bd.SetProduction(PayType.Gold, 71000); bt.Add(bd); }
                { BuildingDef bd = new BuildingDef(480, 1600200, 700, 0, 1 * 60 * 60, 6,15); bd.SetCapacity(191000 * 240, 0, 0); bd.SetProduction(PayType.Gold, 191000); bt.Add(bd); }
                Buildings.Add(bt);
            }
            // load building type and definition data from xml file
            // Load();

            #region Army Units (combat city builder)

            // unit definiron for training
            //0-Barbarian
            {
                ArmyType at = new ArmyType("Barbarian", "", 7); //None, Melee, 1, 20, 16, 1, 1, 0.4
                at.Add(new ArmyDef(8, 45, 25, 0, 0, 0));
                at.Add(new ArmyDef(11, 54, 40, 50000, 1, 21600));
                at.Add(new ArmyDef(14, 65, 60, 150000, 3, 86400));
                at.Add(new ArmyDef(18, 78, 100, 500000, 5, 259200));
                at.Add(new ArmyDef(23, 95, 150, 1500000, 6, 432000));
                at.Add(new ArmyDef(26, 110, 200, 4500000, 7, 864000));
                at.Add(new ArmyDef(30, 125, 250, 6000000, 8, 1209600));
                Armies.Add(at);
            }

            //1-Archer
            {
                ArmyType at = new ArmyType("Archer", "", 1);
                at.Add(new ArmyDef(8, 45, 25, 0, 0, 0));
                Armies.Add(at);
            }
            #endregion

        }

        void Start() {

        }



        #region ================= SAVE AND LOAD =======================

        public void Save() {

            string xmlFilePath = BEUtil.pathForDocumentsFile(dbFilename);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml("<Database><name>wrench</name></Database>");
            {
                xmlDocument.DocumentElement.RemoveAll();

                // Version
                { XmlElement ne = xmlDocument.CreateElement("ConfigVersion"); ne.SetAttribute("value", ConfigVersion.ToString()); xmlDocument.DocumentElement.AppendChild(ne); }

                XmlElement buildingRoot = xmlDocument.CreateElement("Building");
                xmlDocument.DocumentElement.AppendChild(buildingRoot);
                foreach (BuildingType bt in Buildings) {
                    bt.Save(xmlDocument, buildingRoot);
                }

                // ####### Encrypt the XML ####### // If you want to view the original xml file, turn of this piece of code and press play.
                if (xmlDocument.DocumentElement.ChildNodes.Count >= 1) {
                    xmlDocument.Save(xmlFilePath);
                }
                // ###############################
            }
        }

        public void Load() {

            //TextAsset textAsset = new TextAsset();
            //textAsset = (TextAsset)Resources.Load(dbFilename, typeof(TextAsset));

            TextAsset textAsset = (TextAsset)Resources.Load("Database");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(textAsset.text);

            //string xmlFilePath = BEUtil.pathForDocumentsFile(dbFilename);
            //XmlDocument xmlDocument = new XmlDocument();
            //xmlDocument.Load(xmlFilePath);

            if (xmlDocument != null) {
                XmlElement element = xmlDocument.DocumentElement;
                XmlNodeList list = element.ChildNodes;
                foreach (XmlElement ele in list) {
                    if (ele.Name == "ConfigVersion") { ConfigVersion = int.Parse(ele.GetAttribute("value")); }
                    else if (ele.Name == "Building") {
                        XmlNodeList list2 = ele.ChildNodes;
                        foreach (XmlElement ele2 in list2) {
                            if (ele2.Name == "BuildingType")
                                Buildings.Add(new BuildingType(ele2));
                        }
                    }
                    else { }
                }
            }
        }

        #endregion

        // get proper icon image by resource type
        public static Sprite GetPayTypeIcon(PayType _payType) {
            if (_payType == PayType.Gold) return Resources.Load<Sprite>("Icons/Gold");
            else if (_payType == PayType.Elixir) return Resources.Load<Sprite>("Icons/Elixir");
            else if (_payType == PayType.Gem) return Resources.Load<Sprite>("Icons/Gem");
            else if (_payType == PayType.Sulfur) return Resources.Load<Sprite>("Icons/Sulfur");
            else return null;
        }

        public static AudioClip GetAudio(int id) { return instance.audioClip[id]; }
        public static int GetInAppItemCount() { return instance.InApps.Count; }
        public static InAppItem GetInAppItem(int id) { return instance.InApps[id]; }
        public static int GetLevel(int expTotal) {
            for (int Level = 1; Level < MAX_LEVEL; ++Level) {
                if ((instance.LevelExpTotal[Level] <= expTotal) && (expTotal < instance.LevelExpTotal[Level + 1]))
                    return Level;
            }
            return -1;
        }
        public static int GetLevelExp(int level) { return instance.LevelExp[level]; }
        public static int GetLevelExpTotal(int level) { return instance.LevelExpTotal[level]; }
        public static string GetBuildingName(int type) { return instance.Buildings[type].Name; }
        public static BuildingType GetBuildingType(int type) { return instance.Buildings[type]; }
        public static BuildingDef GetBuildingDef(int type, int Level) { return (Level > 0) ? instance.Buildings[type].Defs[Level - 1] : null; }
        public static int GetArmyTypeCount() { return instance.Armies.Count; }
        public static ArmyType GetArmyType(int type) { return (type < instance.Armies.Count) ? instance.Armies[type] : null; }
        public static ArmyDef GetArmyDef(int type, int Level) { return (Level > 0) ? instance.Armies[type].Defs[Level - 1] : null; }
        public static int TownHallLevelRequired(int type, int Level) { return instance.Buildings[type].Defs[Level].RankRequired; }
    }


}
#endregion