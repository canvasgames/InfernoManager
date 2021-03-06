﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          UIShop
///   Description:    class for shop dialog 
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE
{

    public enum ShopType
    {
        Normal = 0, // incase general buildings
        InApp = 1, // inapp item like gem
        House = 2, // house only (worker)
    };


    public class UIShop : MonoBehaviour
    {
        public static UIShop instance;

        public GameObject prefabShopItem;
        public GameObject prefabShopItemGem;
        public RectTransform rtDialog;
        public Toggle[] toggleButtons;
        public GameObject[] views;
        public GameObject[] contents;
        private ShopType eType = ShopType.Normal;

        private ShopDataManager shopDataManager;

        void Awake()
        {
            shopDataManager = new ShopDataManager();
            instance = this;
            gameObject.SetActive(false);
        }

        void Start()
        {
            if (GLOBALS.s.TUTORIAL_PHASE == 8)
            {
                CategorySelected(1);
                inactivateTabsTutorial();
            }
        }

        void Update()
        {
            if (!UIDialogMessage.IsShow() && Input.GetKeyDown(KeyCode.Escape))
            {
                Hide();
            }
        }

        // when category tab selected
        public void CategorySelected(int value)
        {

            Debug.Log("UIShop::CategorySelected" + value.ToString());

            if (GLOBALS.s.TUTORIAL_PHASE == 21 && value == 1)
            {
                TutorialController.s.pressBuildImpCasePressed();
            }

            ArrowsShop.s.recieveTab(value);

            for (int i = 0; i < toggleButtons.Length; ++i)
            {
                views[i].SetActive(toggleButtons[i].isOn ? true : false);
                contents[i].SetActive(toggleButtons[i].isOn ? true : false);
                if (GLOBALS.s.TUTORIAL_PHASE == 15 && value == 1 && i == 1)
                    views[i].GetComponent<ScrollRect>().horizontal = false;
                else
                    views[i].GetComponent<ScrollRect>().horizontal = true;
            }

        }

        public void Hide()
        {
            if (GLOBALS.s.TUTORIAL_PHASE != 8 && GLOBALS.s.TUTORIAL_PHASE != 15 && GLOBALS.s.TUTORIAL_PHASE != 21)
            {
                BETween.anchoredPosition(rtDialog.gameObject, 0.3f, new Vector3(0, -500)).method = BETweenMethod.easeOut;
                BETween.alpha(gameObject, 0.3f, 0.5f, 0.0f).method = BETweenMethod.easeOut;
                BETween.enable(gameObject, 0.01f, false).delay = 0.4f;
                //gameObject.SetActive(false);
                SceneTown.isModalShow = false;
            }

        }

        void _Show(ShopType type)
        {
            if (GLOBALS.s.TUTORIAL_PHASE == 21)
            {
                inactivateTabsTutorial2();
            }

            // if shop dialog called while new building is in creation,
            // delete new building
            if (SceneTown.buildingSelected != null)
            {
                if (!SceneTown.buildingSelected.OnceLanded)
                {
                    SceneTown.instance.BuildingDelete();
                }
            }

            gameObject.transform.localPosition = Vector3.zero;
            //Vector3 abc = new Vector3 (-600f, 0, 0);
            //gameObject.transform.localPosition = gameObject.transform.localPosition - abc;
            gameObject.SetActive(true);
            gameObject.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
            SceneTown.isModalShow = true;
            eType = type;

            // delete old items of each content
            for (int i = 0; i < contents.Length; ++i)
            {
                for (int j = contents[i].transform.childCount - 1; j >= 0; j--)
                {
                    Destroy(contents[i].transform.GetChild(j).gameObject);
                }
            }

            // create shop items of each contents
            if (eType == ShopType.Normal)
            {
                List<BuildingType> bt = new List<BuildingType>();

                //Popula shop com base na planilha buildings_in_shop

                bt.Clear();
                foreach (BuildingShopData bsd in shopDataManager.getShopBuildingsByCategory(BuildingShopData.CATEGORY_PUNISH))
                    bt.Add(TBDatabase.GetBuildingType(bsd.buildingId));

                FillContents(0, "Punish", bt);

                bt.Clear();
                foreach (BuildingShopData bsd in shopDataManager.getShopBuildingsByCategory(BuildingShopData.CATEGORY_DEVILS))
                    bt.Add(TBDatabase.GetBuildingType(bsd.buildingId));

                FillContents(1, "Resources", bt);

                bt.Clear();
                foreach (BuildingShopData bsd in shopDataManager.getShopBuildingsByCategory(BuildingShopData.CATEGORY_RESEARCH))
                    bt.Add(TBDatabase.GetBuildingType(bsd.buildingId));

                FillContents(2, "Research", bt);

                /******************************
                bt.Add(TBDatabase.GetBuildingType(11));
                bt.Add(TBDatabase.GetBuildingType(12));
                bt.Add(TBDatabase.GetBuildingType(13));
                bt.Add(TBDatabase.GetBuildingType(14));
                bt.Add(TBDatabase.GetBuildingType(16));
                bt.Add(TBDatabase.GetBuildingType(17));
                
                ********************************/


                // fill second tab

                //bt.Clear();
                //bt.Add(TBDatabase.GetBuildingType(3));
                /***********************
				
                
                bt.Add(TBDatabase.GetBuildingType(23));
                bt.Add(TBDatabase.GetBuildingType(22));
                **********************/

                //bt.Add(TBDatabase.GetBuildingType(24));
                //bt.Add(TBDatabase.GetBuildingType(5));
                // bt.Add(TBDatabase.GetBuildingType(6));
                // bt.Add (TBDatabase.GetBuildingType(2));
                //bt.Add (TBDatabase.GetBuildingType(7));
                //bt.Add (TBDatabase.GetBuildingType(8));
                //bt.Add (TBDatabase.GetBuildingType(9));
                //bt.Add (TBDatabase.GetBuildingType(10));

                /**************
                FillContents(1, "Resources", bt);
                ***************/

                // fill third tab
                //bt.Clear();
                /*******************
                
                bt.Add(TBDatabase.GetBuildingType(18));
                bt.Add(TBDatabase.GetBuildingType(21));
                bt.Add(TBDatabase.GetBuildingType(20));
                bt.Add(TBDatabase.GetBuildingType(19));
                *************************/
                //FillContents(2, "Research", bt);

                BETween.anchoredPosition(rtDialog.gameObject, 0.3f, new Vector3(0, -500), new Vector3(0, 0)).method = BETweenMethod.easeOut;
            }
            else if (eType == ShopType.InApp)
            {

                FillInApp(0, "InApp");

                // if shop has only one tab, then change position to hide tab
                BETween.anchoredPosition(rtDialog.gameObject, 0.3f, new Vector3(0, -500), new Vector3(0, -50)).method = BETweenMethod.easeOut;
            }
            else if (eType == ShopType.House)
            {

                List<BuildingType> bt = new List<BuildingType>();

                // add house building
                bt.Clear();
                bt.Add(TBDatabase.GetBuildingType(1));
                FillContents(0, "House", bt);

                // if shop has only one tab, then change position to hide tab
                BETween.anchoredPosition(rtDialog.gameObject, 0.3f, new Vector3(0, -500), new Vector3(0, -50)).method = BETweenMethod.easeOut;
            }
            else { }

            toggleButtons[0].isOn = true;
            CategorySelected(0);
            rtDialog.anchoredPosition = new Vector3(0, -500);
            BETween.alpha(gameObject, 0.3f, 0, 0.5f).method = BETweenMethod.easeOut;
        }

        // add shop item with position and animation
        public UIShopItem AddShopItem(GameObject prefab, int id, int i)
        {
            GameObject go = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);
            if (GLOBALS.s.TUTORIAL_PHASE == 21 && id == 1 && i == 0)
                go.transform.SetParent(contents[id].transform.parent);
            else
                go.transform.SetParent(contents[id].transform);
            go.transform.localScale = Vector3.one;
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(i * 320 + 150 + 10, 210 + 10);

            UIShopItem script = go.GetComponent<UIShopItem>();

            go.GetComponent<CanvasGroup>().alpha = 0;

            BETween bt1 = BETween.alpha(go, 0.1f, 0.0f, 1.0f);
            bt1.delay = 0.1f * (float)i + 0.2f;

            BETween bt2 = BETween.scale(go, 0.2f, Vector3.one, new Vector3(1.1f, 1.1f, 1.1f));
            bt2.delay = bt1.delay;
            bt2.loopStyle = BETweenLoop.pingpong;

            return script;
        }

        // incase shop item is building
        public void FillContents(int id, string TabName, List<BuildingType> list)
        {
            toggleButtons[id].transform.Find("Label").GetComponent<Text>().text = TabName;

            for (int i = 0; i < list.Count; ++i)
            {
                UIShopItem script = AddShopItem(prefabShopItem, id, i);
                script.Init(list[i]);
            }

            contents[id].GetComponent<RectTransform>().sizeDelta = new Vector3(320 * list.Count, 440);
        }

        // incase shop item is inapp
        public void FillInApp(int id, string TabName)
        {
            toggleButtons[id].transform.Find("Label").GetComponent<Text>().text = TabName;

            for (int i = 0; i < TBDatabase.GetInAppItemCount(); ++i)
            {
                UIShopItem script = AddShopItem(prefabShopItemGem, id, i);
                script.Init(TBDatabase.GetInAppItem(i));
            }

            contents[id].GetComponent<RectTransform>().sizeDelta = new Vector3(320 * TBDatabase.GetInAppItemCount(), 440);
        }

        public static void Show(ShopType type) { instance._Show(type); }

        void inactivateTabsTutorial()
        {

            for (int i = 0; i < toggleButtons.Length; ++i)
            {
                if (i > 0)
                    contents[i].SetActive(toggleButtons[i].interactable = false);
            }

        }

        public void inactivateTabsTutorial2()
        {

            for (int i = 0; i < toggleButtons.Length; ++i)
            {
                if (i != 1)
                    contents[i].SetActive(toggleButtons[i].interactable = false);
                else
                    contents[i].SetActive(toggleButtons[i].interactable = true);
            }

        }

        public void activateTabs()
        {

            for (int i = 0; i < toggleButtons.Length; ++i)
            {
                contents[i].SetActive(toggleButtons[i].interactable = true);
            }

        }
    }

}
