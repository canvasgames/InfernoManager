﻿#region Using...

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using DG.Tweening;
using UnityEngine.SceneManagement;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          SceneTown
///   Description:    main class of town 
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
/// 
#endregion
namespace BE
{

    #region Variables Declaration
    public class SceneTown : MonoBehaviour
    {

        public static SceneTown instance;

        public GameObject bg;
        public GameObject camPos;

        public Text textLevel;
        public float minDistSlide = 0.01f;

        private bool bInTouch = false;
        private float ClickAfter = 0.0f;
        private bool bTemporarySelect = false;
        private Vector3 vCamRootPosOld = Vector3.zero;
        private Vector3 vCamPosOld = Vector3.zero;
        private Vector3 mousePosOld = Vector3.zero;
        private Vector3 mousePosLast = Vector3.zero;
        public GameObject goCamera = null;
        public GameObject goCameraRoot = null;
        public bool camPanningUse = true;
        public BEGround ground = null;
        private bool Dragged = false;
        private bool cameraStopping = false;

        public float cameraStopSpeed = 0.5f;
        public float cameraSpeed = 0.05f;
        private float avgx;
        private float avgy;
        private Vector3 avgDist;

        private float lastMoveTime = 0;

        /*private float 		zoomMax = 128;
		private float 		zoomMin = 16;
		private float 		zoomCurrent = 64.0f;
		*/
        private float zoomSpeed = 4;

        private float zoomMax = 19;
        private float zoomMin = 8;
        private float zoomCurrent = 9;

        private float perspectiveZoomSpeed = 0.3f;
        //public	float 		perspectiveZoomSpeed = 0.0001f;	// The rate of change of the field of view in perspective mode.
        public float orthoZoomSpeed = 1f;           // The rate of change of the orthographic size in orthographic mode.

        [HideInInspector]
        public Plane xzPlane;


        // when game started, camera zoomin 
        private bool InFade = true;
        private float FadeAge = 0.0f;

        private Building MouseClickedBuilding = null;
        private Text HouseInfo = null;

        public static bool isModalShow = false;
        public static Building buildingSelected = null;

        // TBDRESOURCES create new resources here
        public static BENumber Exp;
        public static BENumber Gold;
        public static BENumber Elixir;
        public static BENumber Gem;
        public static BENumber Shield;
        public static BENumber MaxElixir;

        public static BENumber Sulfur;
        public static BENumber Evilness;

        private static int Level = 0;
        private static int ExpTotal = 0;

        //MINE RESOURCES
        public static int SoulProductionIncTotal = 0;

        private float EvilnessTimer = 0;
        private float EvilnessSeconds = 0;

        #endregion

        #region Init Everything
        void Awake()
        {
            instance = this;

            // initialize BENumber class and set ui element 
            Exp = new BENumber(BENumber.IncType.VALUE, 0, 100000000, 0);
            Exp.AddUIImage(BEUtil.GetObject("PanelOverlay/LabelElixir/LabelExp/Fill").GetComponent<Image>());
            Exp.AddUIImageMax(BEUtil.GetObject("PanelOverlay/LabelElixir/LabelExp/FillLimit").GetComponent<Image>());

            Gold = new BENumber(BENumber.IncType.VALUE, 0, 200000, 100); // initial gold count is 1000
            Gold.AddUIText(BEUtil.GetObject("PanelOverlay/LabelGold/Text").GetComponent<Text>());
            Gold.AddUITextMax(BEUtil.GetObject("PanelOverlay/LabelGold/TextMax").GetComponent<Text>());
            Gold.AddUIImage(BEUtil.GetObject("PanelOverlay/LabelGold/Fill").GetComponent<Image>());


            Elixir = new BENumber(BENumber.IncType.VALUE, 0, 300000, 0, PayType.Elixir); // initial elixir count is 1000	
            Elixir.AddUIText(BEUtil.GetObject("PanelOverlay/LabelElixir/Text").GetComponent<Text>());
            Elixir.AddUITextMax(BEUtil.GetObject("PanelOverlay/LabelElixir/TextMax").GetComponent<Text>());
            Elixir.AddUIImage(BEUtil.GetObject("PanelOverlay/LabelElixir/Fill").GetComponent<Image>());

            //-----------

            //Sulfur
            Sulfur = new BENumber(BENumber.IncType.VALUE, 0, 999, 555, PayType.Sulfur); // initial Sulfur count is 1000
            Sulfur.AddUIText(BEUtil.GetObject("PanelOverlay/LabelSulfur/Text").GetComponent<Text>());
            Sulfur.AddUITextMax(BEUtil.GetObject("PanelOverlay/LabelSulfur/TextMax").GetComponent<Text>());
            Sulfur.AddUIImage(BEUtil.GetObject("PanelOverlay/LabelSulfur/Fill").GetComponent<Image>());

            //Evilness
            Evilness = new BENumber(BENumber.IncType.VALUE, 0, 999, 1999, PayType.Evilness); // initial Evilness count is 1000
            Evilness.AddUIText(BEUtil.GetObject("PanelOverlay/LabelEvilness/Text").GetComponent<Text>());
            Evilness.AddUITextMax(BEUtil.GetObject("PanelOverlay/LabelEvilness/TextMax").GetComponent<Text>());
            Evilness.AddUIImage(BEUtil.GetObject("PanelOverlay/LabelEvilness/Fill").GetComponent<Image>());

            //------------

            Gem = new BENumber(BENumber.IncType.VALUE, 0, 100000000, 50);   // initial gem count is 100	0	
            Gem.AddUIText(BEUtil.GetObject("PanelOverlay/LabelGem/Text").GetComponent<Text>());

            //TBDCURRENCIES SET THEIR VALUE HERE

            HouseInfo = BEUtil.GetObject("PanelOverlay/LabelHouse/Text").GetComponent<Text>();

            Shield = new BENumber(BENumber.IncType.TIME, 0, 100000000, 0);
            Shield.AddUIText(BEUtil.GetObject("PanelOverlay/LabelShield/Text").GetComponent<Text>());

            // For camera fade animation, set cameras initial positions
            goCameraRoot.transform.position = new Vector3(-5.5f, 0, 1);
            goCamera.transform.localPosition = new Vector3(0, 0, -128.0f);
            InFade = true;
            FadeAge = 0.0f;
        }

        void Start()
        {

            Time.timeScale = 1;
            isModalShow = false;
            xzPlane = new Plane(new Vector3(0f, 1f, 0f), 0f);

            // load game data from xml file
            Load();

            //FIRST TIME USER if user new to this game add initial building
            if (bFirstRun)
            {

                /*    
                    // add town hall 
                    {
                        Building script = BEGround.instance.BuildingAdd (0,1);
                        script.Move(Vector3.zero);
                        BuildingSelect(script);
                        BuildingLandUnselect();
                    }
                    // add hut
                    {
                        Building script = BEGround.instance.BuildingAdd (4,1);
                        script.Move(new Vector3(4,0,0));
                        BuildingSelect(script);
                        BuildingLandUnselect();
                    }*/
            }

            GainExp(0); // call this once to calculate level

            //set resource's capacity
            CapacityCheck();

            // create workers by hut count
            int HutCount = BEGround.instance.GetBuildingCount(1);
            BEWorkerManager.instance.CreateWorker(HutCount);
            BEGround.instance.SetWorkingBuildingWorker();
        }

        #endregion

        public void OnValueChanged(float a)
        {
            minDistSlide = a;
        }

        // result of quit messagebox
        public void MessageBoxResult(int result)
        {
            BEAudioManager.SoundPlay(6);
            if (result == 0)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
            }
        }

        #region UPDATE that handles Camera and building placement
        void Update()
        {
            #region Update GUI values
            // get delta time from BETime
            float deltaTime = BETime.deltaTime;

            // if user pressed escape key, show quit messagebox
            //if (!UIDialogMessage.IsShow("get keydown scape") && !isModalShow && Input.GetKeyDown(KeyCode.Escape)) {
            //  UIDialogMessage.Show("Do you want to quit this program?", "Yes,No", "Quit?", null, (result) => { MessageBoxResult(result); });
            //}

            // if in camera animation 
            InFade = false;
            /*if (InFade) {

                //camera zoom in
                FadeAge += Time.deltaTime * 0.7f;
                if (FadeAge > 1.0f) {
                    InFade = false;
                    FadeAge = 1.0f;
                    zoomCurrent = 24.0f;
                }

                goCameraRoot.transform.position = Vector3.Lerp(new Vector3(-5.5f, 0, -5), Vector3.zero, FadeAge);
                goCamera.transform.localPosition = Vector3.Lerp(new Vector3(0, 0, -128.0f), new Vector3(0, 0, -24.0f), FadeAge);
            }*/

            //TBDRESOUCES UPDATE YOUR RESOURCE BY TIME HERE
            if (Exp != null) Exp.Update(); else Debug.Log("EXP IS NULL....");
            Gold.Update();
            Elixir.Update();
            Sulfur.Update();
            Evilness.Update();
            Gem.Update();
            Shield.ChangeTo(Shield.Target() - (double)deltaTime);
            Shield.Update();
            HouseInfo.text = BEWorkerManager.instance.GetAvailableWorkerCount().ToString() + "/" + BEGround.instance.GetBuildingCount(1).ToString();

            if (UIDialogMessage.IsShow("scene town update") || isModalShow) return;
            //if(EventSystem.current.IsPointerOverGameObject()) return;

            #endregion
            if (!GLOBALS.s.SPANKING_OCURRING)
            {
                #region Camera Movement on Mouse button down
                if (Input.GetMouseButton(0))
                {

                    if (EventSystem.current.IsPointerOverGameObject() || GLOBALS.s.LOCK_CAMERA_TUTORIAL == true && GLOBALS.s.LOCK_CLICK_TUTORIAL == true || (GLOBALS.s.DIALOG_ALREADY_OPENED == true && GLOBALS.s.TUTORIAL_OCCURING == false))
                    {
                        return;
                    }

                    //Click MouseButton
                    if (!bInTouch && GLOBALS.s.LOCK_CLICK_TUTORIAL == false)
                    {
                        bInTouch = true;
                        ClickAfter = 0.0f;
                        bTemporarySelect = false;
                        Dragged = false;
                        mousePosOld = Input.mousePosition;
                        mousePosLast = Input.mousePosition;
                        avgx = 0;
                        avgy = 0;
                        avgDist = new Vector3(0, 0, 0);
                        vCamRootPosOld = goCameraRoot.transform.position;
                        vCamPosOld = goCamera.transform.localPosition;

                        //when a building was selected and user drag mouse on the map
                        //check mouse drag start is over selected building or not
                        //if not do not move selected building
                        Ray ray = Camera.main.ScreenPointToRay(mousePosOld);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject.tag == "Building"))
                        {
                            MouseClickedBuilding = BuildingFromObject(hit.collider.gameObject);
                        }
                        else
                        {
                            MouseClickedBuilding = null;
                        }

                        //Debug.Log ("Update buildingSelected:"+((buildingSelected != null) ? buildingSelected.name : "none"));

                    }
                    #endregion

                    #region Camera Movement holding button
                    else if (GLOBALS.s.LOCK_CAMERA_TUTORIAL == false)
                    {
                        //Mouse Button is in pressed 
                        //if mouse move certain diatance
                        float mDist = Vector3.Distance(Input.mousePosition, mousePosLast);
                        if (((mDist > 0.01f && !Application.isMobilePlatform) || (mDist > 3.5f && Application.isMobilePlatform)) && mDist < 150f
                            )
                        {
                            //if ( mDist > minDistSlide && mDist < 100f) {
                            // set drag flag on
                            if (!Dragged)
                            {
                                Dragged = true;

                                // show tile grid
                                if ((buildingSelected != null) && (MouseClickedBuilding == buildingSelected) && buildingSelected.Type != 0 && buildingSelected.Type != 4)
                                {
                                    BETween.alpha(ground.gameObject, 0.1f, 0.0f, 0.3f);
                                    //Debug.Log ("ground alpha to 0.1");
                                }
                            }

                            avgx = Math.Abs(Input.mousePosition.x - mousePosLast.x) / 2;
                            avgy = Math.Abs(Input.mousePosition.y - mousePosLast.y) / 2;
                            if (avgDist.x == 0 && avgDist.y == 0) avgDist = Input.mousePosition - mousePosLast;
                            else avgDist = (avgDist + Input.mousePosition - mousePosLast) / 2;
                            mousePosLast = Input.mousePosition;



                            // if selected building exist
                            if ((buildingSelected != null) && (MouseClickedBuilding == buildingSelected) && buildingSelected.Type != 0 && buildingSelected.Type != 4)
                            {

                                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                float enter;
                                xzPlane.Raycast(ray, out enter);
                                Vector3 vTarget = ray.GetPoint(enter);
                                // move selected building
                                buildingSelected.Move(vTarget);
                            }
                            // else camera panning
                            else
                            {
                                Vector3 vDelta = (Input.mousePosition - mousePosOld) * cameraSpeed;
                                //Debug.Log(" MOUSE POSITION: " + Input.mousePosition + "  OLD: " + mousePosOld + "  CCC CAM POS: " + goCamera.transform.localPosition);
                                //Debug.Log(" BG " + goCamera.GetComponent<Camera>().WorldToScreenPoint(bg.transform.localPosition));

                                //goCamera.transform.position = vCamPosOld - vDelta;
                                goCamera.transform.localPosition = new Vector3(vCamPosOld.x - vDelta.x, vCamPosOld.y - vDelta.y, goCamera.transform.localPosition.z);
                                if (QA.s.first_game == 1)
                                {
                                    // Debug.Log("i am here!");
                                    if (goCamera.transform.localPosition.x < -14.5f + 3.8f)
                                        goCamera.transform.localPosition = new Vector3(-14.5f + 3.8f, goCamera.transform.localPosition.y, goCamera.transform.localPosition.z);
                                    if (goCamera.transform.localPosition.x > 13.85f + 3.8)
                                        goCamera.transform.localPosition = new Vector3(13.85f + 3.8f, goCamera.transform.localPosition.y, goCamera.transform.localPosition.z);
                                    if (goCamera.transform.localPosition.y < -9.4f - 9)
                                        goCamera.transform.localPosition = new Vector3(goCamera.transform.localPosition.x, -9.4f - 9f, goCamera.transform.localPosition.z);
                                    if (goCamera.transform.localPosition.y > 15 - 9f)
                                        goCamera.transform.localPosition = new Vector3(goCamera.transform.localPosition.x, 15f - 9f, goCamera.transform.localPosition.z);
                                }
                                else
                                {
                                    if (goCamera.transform.localPosition.x < -14.5f)
                                        goCamera.transform.localPosition = new Vector3(-14.5f, goCamera.transform.localPosition.y, goCamera.transform.localPosition.z);
                                    if (goCamera.transform.localPosition.x > 13.85f)
                                        goCamera.transform.localPosition = new Vector3(13.85f, goCamera.transform.localPosition.y, goCamera.transform.localPosition.z);
                                    if (goCamera.transform.localPosition.y < -9.4f)
                                        goCamera.transform.localPosition = new Vector3(goCamera.transform.localPosition.x, -9.4f, goCamera.transform.localPosition.z);
                                    if (goCamera.transform.localPosition.y > 15f)
                                        goCamera.transform.localPosition = new Vector3(goCamera.transform.localPosition.x, 15f, goCamera.transform.localPosition.z);

                                }

                                /*
                                Vector3 vForward = goCameraRoot.transform.forward; vForward.y = 0.0f; vForward.Normalize();
                                Vector3 vRight = goCameraRoot.transform.right; vRight.y = 0.0f; vRight.Normalize();
                                Vector3 vMove = -vForward * vDelta.y + -vRight * vDelta.x;
                                goCameraRoot.transform.position = vCamRootPosOld + vMove;
                                //Debug.Log(" CAMERA LOCAL POSITION! X: " + goCameraRoot.transform.localPosition.x + " Y: " + goCameraRoot.transform.localPosition.y + " Z: " + goCameraRoot.transform.localPosition.z);
                                // Debug.Log(" BG POS X: " + bg.transform.position.x + " XL: " + bg.transform.localPosition.x + " Y: " + bg.transform.localPosition.y + " Z: " + bg.transform.localPosition.z);
                                //Debug.Log(" SCREEN POS X: "  + " XL: " + bg.transform.localPosition.x + " Y: " + bg.transform.localPosition.y + " Z: " + bg.transform.localPosition.z);
                                Camera cam = GetComponent<Camera>();
                                //Vector3 pos = goCamera.transform.position;  // get the game object position
                                */

                                lastMoveTime = Time.time;
                                // Debug.Log(" DRAGGIN! lastMoveTime: " + lastMoveTime + " mousePosLast: " + mousePosLast + " AVGX: " + avgx + " AVGY: " + avgy + " | AVGDIST: " + avgDist);

                            }
                        }
                        // Not Move
                        else
                        {
                            avgx = 0;
                            avgy = 0;
                            avgDist = new Vector3(0, 0, 0);

                            if (!Dragged)
                            {
                                ClickAfter += Time.deltaTime;
                                if (!bTemporarySelect && (ClickAfter > 0.5f))
                                {
                                    bTemporarySelect = true;
                                    Debug.Log("Update2 buildingSelected:" + ((buildingSelected != null) ? buildingSelected.name : "none"));
                                    Pick();
                                }
                            }
                        }

                    }
                }

                #endregion

                #region Release Mouse Button
                else
                {

                    //Release MouseButton
                    if (bInTouch)
                    {
                        //Debug.Log("b in touch");
                        bInTouch = false;
                        // if in drag state
                        if (Dragged)
                        {
                            //Debug.Log("DRAGGED STATE");
                            // seleted building exist
                            if (buildingSelected != null)
                            {
                                // hide tile grid
                                if (MouseClickedBuilding == buildingSelected && buildingSelected.Type != 0 && buildingSelected.Type != 4)
                                    BETween.alpha(ground.gameObject, 0.1f, 0.3f, 0.0f);

                                if (buildingSelected.Landable && buildingSelected.OnceLanded)
                                {
                                    //BuildingLandUnselect(false);
                                    Debug.Log("BuildingLandUnselect chamado 3");
                                }

                            }

                            //camera was moving!! slowdown its movement
                            else if (QA.s.CameraNavigationOnRelease)
                            {
                                float timeDif = Time.time - lastMoveTime;
                                //Debug.Log("cameraStopping? " + cameraStopping + " timeDif " + timeDif);

                                if (!cameraStopping && timeDif < 1f && timeDif > 0)
                                {
                                    /*
                                    float dist = Vector3.Distance(Input.mousePosition, mousePosLast);
                                    float directionX, directionY;
                                    //float curVelocityX = Math.Abs(Input.mousePosition.x - mousePosLast.x) * timeDif;
                                    //float curVelocityX = Math.Abs(Input.mousePosition.x - mousePosLast.x) * timeDif;
                                    float curVelocityX = avgx;
                                    float curVelocityY = avgy;
                                    if (Input.mousePosition.x > mousePosLast.x) directionX = -1f; else directionX = 1f;
                                    if (Input.mousePosition.y > mousePosLast.y) directionY = -1f; else directionY = 1f;
                                    float newX = goCameraRoot.transform.position.x + curVelocityX * cameraStopSpeed * directionX;
                                    float newY = goCameraRoot.transform.position.z + curVelocityY * cameraStopSpeed * directionY;


                                    Debug.Log("avgx: " + avgx + " avgy " + avgy);
                                    Debug.Log("dist: " + dist + " Vx " + (curVelocityX) + " Vy: " + (curVelocityY) + " | xDif: " + (Input.mousePosition.x - mousePosLast.x) + " yDif: "+ (Input.mousePosition.y - mousePosLast.y));
                                    Debug.Log("dict x: " + directionX + " dict y " + directionY);
                                    Debug.Log("Camera x: " + goCameraRoot.transform.position.x + " x target: " + newX + " | Camera Y: " + goCameraRoot.transform.position.z + " target y: " + newY);
                                    cameraStopping = true;
                                    goCameraRoot.transform.DOMove(new Vector3(newX, 0, newY), 0.5f).SetEase(Ease.OutQuad).OnComplete(() => cameraStopping = false);

                                    avgx = 0;
                                    avgy = 0;
                                    // Debug.Log("CAMERA STOPPING START. time dif: " + (Time.deltaTime - lastMoveTime) + " vDelta: " + curVelocity);
                                    */

                                    if (avgDist.x == 0 && avgDist.y == 0) avgDist = Input.mousePosition - mousePosLast;
                                    else avgDist = (avgDist + Input.mousePosition - mousePosLast) / 2;
                                    //avgDist =  Input.mousePosition - mousePosLast;
                                    // Debug.Log("AVGDIST: " + avgDist + "| LastmouseX: " + mousePosLast.x + " lastMouseY : " + mousePosLast.y + " MouseX: " + Input.mousePosition.x + " MouseY: " + Input.mousePosition.y + " LastMousePosZ: " + mousePosLast.z + " MouseZ " + Input.mousePosition.z);

                                    Vector3 vDelta = avgDist * cameraStopSpeed;
                                    Vector3 vForward = goCameraRoot.transform.forward;
                                    vForward.y = 0.0f;
                                    vForward.Normalize();

                                    Vector3 vRight = goCameraRoot.transform.right;
                                    vRight.y = 0.0f;
                                    vRight.Normalize();

                                    Vector3 vMove = -vForward * vDelta.y + -vRight * vDelta.x;
                                    cameraStopping = true;
                                    goCameraRoot.transform.DOMove(goCameraRoot.transform.position + vMove, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => cameraStopping = false);
                                    // Debug.Log(" CamXold: " + vCamRootPosOld.x + " CamZold : " + vCamRootPosOld.z);
                                    // Debug.Log("CamTrueX: " + goCameraRoot.transform.position.x + " CamTruY: " + goCamera.transform.position.z);
                                    // Debug.Log(" VMoveX:  " + vMove.x + " VMoveZ:  " + vMove.z);

                                    //goCameraRoot.transform.position = vCamRootPosOld + vMove;

                                }
                            }
                        }
                        //unselect building
                        else
                        {

                            if (bTemporarySelect)
                            {
                                // land building
                                if ((buildingSelected != null) && (MouseClickedBuilding != buildingSelected) && buildingSelected.OnceLanded)
                                {

                                    Debug.Log("BuildingLandUnselect chamado 1");
                                    BuildingLandUnselect(false);
                                }

                            }
                            else
                            {
                                // land building
                                if ((buildingSelected != null) && (MouseClickedBuilding != buildingSelected) && buildingSelected.OnceLanded)
                                {
                                    Debug.Log("BuildingLandUnselect chamado 2");

                                    BuildingLandUnselect(true);
                                }

                                //Debug.Log ("Update3 buildingSelected:"+((buildingSelected != null) ? buildingSelected.name : "none"));
                                if (MouseClickedBuilding != buildingSelected || GLOBALS.s.TUTORIAL_OCCURING == true)
                                {
                                    Debug.Log(" PICK:");
                                    Pick();
                                }

                            }
                        }
                    }
                }

                #endregion

                #region Zoom
                //zoom
                if (!InFade && !cameraStopping && (GLOBALS.s.DIALOG_ALREADY_OPENED == false && GLOBALS.s.TUTORIAL_OCCURING == false))
                {
                    zoomCurrent -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
                    zoomCurrent = Mathf.Clamp(zoomCurrent, zoomMin, zoomMax);
                    //goCamera.transform.localPosition = new Vector3(0,0,-zoomCurrent);
                    Camera camMain = goCamera.GetComponent<Camera>();
                    camMain.orthographicSize = zoomCurrent;
                }

                // pinch zoom for mobile touch input
                if (Input.touchCount == 2 && (GLOBALS.s.DIALOG_ALREADY_OPENED == false && GLOBALS.s.TUTORIAL_OCCURING == false))
                {
                    // Store both touches.
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    // Find the position in the previous frame of each touch.
                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    // Find the magnitude of the vector (the distance) between the touches in each frame.
                    float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    // Find the difference in the distances between each frame.
                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                    zoomCurrent += deltaMagnitudeDiff * perspectiveZoomSpeed;
                    zoomCurrent = Mathf.Clamp(zoomCurrent, zoomMin, zoomMax);
                    //goCamera.transform.localPosition = new Vector3(0,0,-zoomCurrent);
                    Camera camMain = goCamera.GetComponent<Camera>();
                    camMain.orthographicSize = zoomCurrent;
                }
                #endregion
            }
        }
        #endregion

        #region Exp and Capacity
        public void move_camera_to_building(Vector3 pos, float duration = 0.5f, float customZoom = 6f, float custom_x = 0, float custom_z = 0)
        {
            Debug.Log("MMMMMMMMMMMMM MOVING CAMERA TO BUILDING !!!! ");
            //pos = new Vector3(pos.x, pos.y, pos.z);
            //Vector3 newPos = new Vector3(pos.x - 1.5f, pos.y, pos.z - 1.5f);
            Vector3 newPos = new Vector3(pos.x - 1.5f + custom_x, pos.y, pos.z - 1.5f + custom_z);
            Camera cam = goCamera.GetComponent<Camera>();
            if (Math.Abs(cam.orthographicSize - customZoom) > 0.1f)
                cam.DOOrthoSize(customZoom, duration).OnComplete(() => zoomCurrent = customZoom);

            cameraStopping = true;
            goCameraRoot.transform.DOMove(newPos, duration).SetEase(Ease.OutQuad).OnComplete(() => cameraStopping = false);
        }

        //picking a building means that...
        public void Pick()
        {

            Debug.Log("Pick buildingSelected:" + ((buildingSelected != null) ? buildingSelected.name : "none"));
            //GameObject goSelectNew = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                //Debug.Log ("Pick"+hit.collider.gameObject.tag);
                if (hit.collider.gameObject.tag == "Ground")
                {
                    Debug.Log("Nao sei o q é isso");
                    BuildingSelect(null);
                    return;
                }
                else if (hit.collider.gameObject.tag == "Building")
                {
                    Debug.Log("Picked a Building");
                    Building buildingNew = BuildingFromObject(hit.collider.gameObject);
                    if (buildingNew.HasCompletedWork())
                        return;

                    //Dont select building when in tutorial
                    if (GLOBALS.s.TUTORIAL_OCCURING == false && GLOBALS.s.DIALOG_ALREADY_OPENED == false)
                        BuildingSelect(buildingNew);
                    return;
                }
                else
                {

                }
            }
            else
            {
                Debug.Log("Dont Picked Any Building");
            }

        }

        #region BUILDING functions
        // get building script
        // if child object was hitted, check parent 
        public Building BuildingFromObject(GameObject go)
        {
            Building buildingNew = go.GetComponent<Building>();
            if (buildingNew == null) buildingNew = go.transform.parent.gameObject.GetComponent<Building>();

            return buildingNew;
        }

        // select building
        public void BuildingSelect(Building buildingNew)
        {
            // if user select selected building again
            bool SelectSame = (buildingNew == buildingSelected) ? true : false;

            if (buildingSelected != null)
            {
                // if initialy created building, then pass
                if (!buildingSelected.OnceLanded) return;
                // building can't land, then pass 
                if (!buildingSelected.Landed && !buildingSelected.Landable) return;

                // land building
                Debug.Log("BuildingLandUnselect chamado 5");
                BuildingLandUnselect(false);
                UICommand.Hide();
                //

            }

            if (SelectSame)
                return;

            buildingSelected = buildingNew;

            if (buildingSelected != null)
            {
                //Debug.Log ("Building Selected:"+buildingNew.gameObject.name+" OnceLanded:"+buildingNew.OnceLanded.ToString ());
                // set scale animation to newly selected building
                BETween bt = BETween.scale(buildingSelected.gameObject, 0.1f, new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.4f, 1.4f, 1.4f));
                bt.loopStyle = BETweenLoop.pingpong;
                // se tbuilding state unland
                Debug.Log("Land call 1");
                buildingSelected.Land(false, true);

            }

        }

        public void BuildingLandUnselect(bool unselect, bool flagzita = false)
        {
            if (buildingSelected == null)
            {

                return;
            }
            Debug.Log("Land call 2");
            buildingSelected.Land(true, true, false);

            //Dont select building created by tut (TH e Gate) or unselect flag
            if ((unselect == true || GLOBALS.s.TUTORIAL_PHASE < 4 && GLOBALS.s.TUTORIAL_OCCURING == true))
            {
                buildingSelected = null;
            }

            Save();

            UICommand.Hide();

            //Eu que pus, para aparecer o HUD logo depois, tem q desaparecer e reaparecer para carregar os bts certos
            //O tempo do invoke parece n funcionar because of reasons
            if (GLOBALS.s.TUTORIAL_PHASE < 4 && GLOBALS.s.TUTORIAL_OCCURING == true && unselect == false)
                Invoke("UICommand.Show", 3);

            /*
            CODIGO ORIGINAL
                 public void BuildingLandUnselect() {
                    if(buildingSelected == null) return;

                    buildingSelected.Land(true, true);
                    buildingSelected = null;
                    Save ();

                    UICommand.Hide();
                }
            */
        }

        public void BuildingDelete()
        {
            if (buildingSelected == null) return;
            Debug.Log("Land call 3");
            buildingSelected.Land(false, false);
            BEGround.instance.BuildingRemove(buildingSelected);
            Destroy(buildingSelected.gameObject);
            buildingSelected = null;
            Save();
        }

        #endregion

        #region BUTTON basic fucntions
        //pause
        public void OnButtonAttack()
        {
            BEAudioManager.SoundPlay(6);
        }

        // user clicked shop button
        public void OnButtonShop()
        {
            if (GLOBALS.s.TUTORIAL_OCCURING == true)
            {
                Debug.Log("ON BUTTON SHOP - TUTORIAL OCURRING");

                if (GLOBALS.s.TUTORIAL_PHASE == 7)
                {

                    TutorialController.s.clickedBuildBt();
                    BEAudioManager.SoundPlay(6);
                    UIShop.Show(ShopType.Normal);
                }
                else if (GLOBALS.s.TUTORIAL_PHASE == 14)
                {
                    TutorialController.s.indicateTabFireMine();
                    BEAudioManager.SoundPlay(6);
                    UIShop.Show(ShopType.Normal);
                }

            }
            else
            {
                Debug.Log("ON BUTTON SHOP - TUTORIAL noooooooooot OCURRING");
                if (GLOBALS.s.DIALOG_ALREADY_OPENED == false)
                {
                    BEAudioManager.SoundPlay(6);
                    UIShop.Show(ShopType.Normal);
                }
            }
        }

        // user clicked gem button
        public void OnButtonGemShop()
        {
            if (GLOBALS.s.TUTORIAL_OCCURING == false && GLOBALS.s.DIALOG_ALREADY_OPENED == false)
            {
                BEAudioManager.SoundPlay(6);
                UIShop.Show(ShopType.InApp);
            }
        }

        // user clicked house button
        public void OnButtonHouse()
        {
            if (GLOBALS.s.TUTORIAL_OCCURING == false && GLOBALS.s.DIALOG_ALREADY_OPENED == false)
            {
                BEAudioManager.SoundPlay(6);
                UIShop.Show(ShopType.House);
            }
        }

        // user clicked option button
        public void OnButtonOption()
        {
            if (GLOBALS.s.TUTORIAL_OCCURING == false && GLOBALS.s.DIALOG_ALREADY_OPENED == false)
            {
                BEAudioManager.SoundPlay(6);
                UIOption.Show();
            }
        }


        #endregion

        public int CalculateTotalSoulsToNextLevel()
        {

            return 1;
        }
        // add exp
        public void GainExp(int exp)
        {
            Debug.Log("[GAIN EX] INIT: " + exp + " EXP TOTAL: " + ExpTotal);
            ExpTotal += exp;
            int NewLevel = TBDatabase.GetLevel(ExpTotal);
            int LevelExpToGet = TBDatabase.GetLevelExp(NewLevel);
            int LevelExpStart = TBDatabase.GetLevelExpTotal(NewLevel);

            SceneTown.Exp.MaxSet(LevelExpToGet);
            int ExpLeft = ExpTotal - LevelExpStart;
            SceneTown.Exp.ChangeTo(ExpLeft);

            Debug.Log("[GAIN EX] NEW LEVEL: " + NewLevel + " | LEVELXPTOGET: " + LevelExpToGet + " LevelExpStart " + LevelExpStart);

            // if level up occured
            if ((NewLevel > Level) && (Level != 0))
            {
                // show levelup notify here
                GLOBALS.s.USER_RANK = NewLevel;
                if (GLOBALS.s.USER_RANK != 2)
                {
                    MenusController.s.createLevelUp();
                }
            }
            Level = NewLevel;
            textLevel.text = NewLevel.ToString();

            // save game data
            Save();
        }


        //check max capacity and, if it is the maximum, set it to maximum.
        public void CapacityCheck()
        {
            int GoldCapacityTotal = BEGround.instance.GetCapacityTotal(PayType.Gold);
            int ElixirCapacityTotal = BEGround.instance.GetCapacityTotal(PayType.Elixir);
            int SulfurCapacityTotal = BEGround.instance.GetCapacityTotal(PayType.Sulfur);
            int EvilnessCapacityTotal = BEGround.instance.GetCapacityTotal(PayType.Evilness);

            Gold.MaxSet(GoldCapacityTotal);
            Elixir.MaxSet(ElixirCapacityTotal);
            Sulfur.MaxSet(SulfurCapacityTotal);
            Evilness.MaxSet(EvilnessCapacityTotal);

            if (Gold.Target() > GoldCapacityTotal)
                Gold.ChangeTo(GoldCapacityTotal);

            if (Elixir.Target() > ElixirCapacityTotal)
                Elixir.ChangeTo(ElixirCapacityTotal);

            if (Sulfur.Target() > SulfurCapacityTotal)
                Sulfur.ChangeTo(SulfurCapacityTotal);

            if (Evilness.Target() > EvilnessCapacityTotal)
                Evilness.ChangeTo(EvilnessCapacityTotal);

            int NewLevel = TBDatabase.GetLevel(ExpTotal);
            int LevelExpStart = TBDatabase.GetLevelExpTotal(NewLevel);
            Exp.CurrentMaxSet(ElixirCapacityTotal - LevelExpStart);

            BEGround.instance.DistributeByCapacity(PayType.Gold, (float)Gold.Target());
            BEGround.instance.DistributeByCapacity(PayType.Elixir, (float)Elixir.Target());
            BEGround.instance.DistributeByCapacity(PayType.Sulfur, (float)Sulfur.Target());
            //BEGround.instance.DistributeByCapacity(PayType.Evilness, (float)Evilness.Target());

            SoulProductionIncTotal = BEGround.instance.GetSoulProductionInc();
        }

        #endregion


        #region Save (and encrypt)
        // related to save and load gamedata with xml format
        bool UseEncryption = false;
        bool bFirstRun = false;
        string configFilename = "Config.dat";
        int ConfigVersion = 1;
        public bool InLoading = false;

        // Do not save town when script quit.
        // save when action is occured
        // (for example, when building created, when start upgrade, when colled product, when training start)
        public void Save()
        {
            if (QA.s.DontSave == false)
            {

                if (InLoading) return;

                string xmlFilePath = BEUtil.pathForDocumentsFile(configFilename);
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml("<item><name>wrench</name></item>");
                {
                    xmlDocument.DocumentElement.RemoveAll();

                    // Version
                    { XmlElement ne = xmlDocument.CreateElement("ConfigVersion"); ne.SetAttribute("value", ConfigVersion.ToString()); xmlDocument.DocumentElement.AppendChild(ne); }
                    { XmlElement ne = xmlDocument.CreateElement("Time"); ne.SetAttribute("value", DateTime.Now.ToString()); xmlDocument.DocumentElement.AppendChild(ne); }
                    { XmlElement ne = xmlDocument.CreateElement("ExpTotal"); ne.SetAttribute("value", ExpTotal.ToString()); xmlDocument.DocumentElement.AppendChild(ne); }
                    { XmlElement ne = xmlDocument.CreateElement("Gem"); ne.SetAttribute("value", Gem.Target().ToString()); xmlDocument.DocumentElement.AppendChild(ne); }
                    { XmlElement ne = xmlDocument.CreateElement("Gold"); ne.SetAttribute("value", Gold.Target().ToString()); xmlDocument.DocumentElement.AppendChild(ne); }
                    { XmlElement ne = xmlDocument.CreateElement("Elixir"); ne.SetAttribute("value", Elixir.Target().ToString()); xmlDocument.DocumentElement.AppendChild(ne); }
                    { XmlElement ne = xmlDocument.CreateElement("Sulfur"); ne.SetAttribute("value", Sulfur.Target().ToString()); xmlDocument.DocumentElement.AppendChild(ne); }
                    { XmlElement ne = xmlDocument.CreateElement("Evilness"); ne.SetAttribute("value", Evilness.Target().ToString()); xmlDocument.DocumentElement.AppendChild(ne); }
                    { XmlElement ne = xmlDocument.CreateElement("Shield"); ne.SetAttribute("value", Shield.Target().ToString()); xmlDocument.DocumentElement.AppendChild(ne); }

                    Transform trDecoRoot = BEGround.instance.trDecoRoot;
                    //List<GameObject> goTiles=new List<GameObject>();
                    foreach (Transform child in trDecoRoot)
                    {
                        Building script = child.gameObject.GetComponent<Building>();
                        if (script != null)
                        {
                            script.Save(xmlDocument);
                        }
                    }

                    // ####### Encrypt the XML ####### 
                    // If you want to view the original xml file, turn of this piece of code and press play.
                    if (xmlDocument.DocumentElement.ChildNodes.Count >= 1)
                    {
                        if (UseEncryption)
                        {
                            string data = BEUtil.Encrypt(xmlDocument.DocumentElement.InnerXml);
                            xmlDocument.DocumentElement.RemoveAll();
                            xmlDocument.DocumentElement.InnerText = data;
                        }
                        xmlDocument.Save(xmlFilePath);
                    }
                    // ###############################
                }
            }
        }

        #endregion

        #region Load
        public void Load()
        {

            if (QA.s.DontSave == false)
            {
                string xmlFilePath = BEUtil.pathForDocumentsFile(configFilename);
                if (!File.Exists(xmlFilePath))
                {
                    Save();
                    bFirstRun = true;
                }
                else
                {
                    bFirstRun = false;
                }

                InLoading = true;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlFilePath);

                // ####### Encrypt the XML ####### 
                // If the Xml is encrypted, so this piece of code decrypt it.
                if (xmlDocument.DocumentElement.ChildNodes.Count <= 1)
                {
                    if (UseEncryption)
                    {
                        string data = BEUtil.Decrypt(xmlDocument.DocumentElement.InnerText);
                        xmlDocument.DocumentElement.InnerXml = data;
                    }
                }
                //################################


                if (xmlDocument != null)
                {
                    XmlElement element = xmlDocument.DocumentElement;
                    XmlNodeList list = element.ChildNodes;
                    foreach (XmlElement ele in list)
                    {
                        if (ele.Name == "ConfigVersion") { ConfigVersion = int.Parse(ele.GetAttribute("value")); }
                        else if (ele.Name == "Time")
                        {
                            DateTime dtNow = DateTime.Now;
                            DateTime dtSaved = DateTime.Parse(ele.GetAttribute("value"));
                            //Debug.Log ("dtNow:"+dtNow.ToString());
                            //Debug.Log ("dtSaved:"+dtSaved.ToString());
                            TimeSpan timeDelta = dtNow.Subtract(dtSaved);
                            //Debug.Log ("TimeSpan:"+timeDelta.ToString());
                            BETime.timeAfterLastRun = timeDelta.TotalSeconds;
                        }
                        else if (ele.Name == "ExpTotal") { ExpTotal = int.Parse(ele.GetAttribute("value")); GLOBALS.s.USER_RANK = TBDatabase.GetLevel(ExpTotal); Debug.Log(" USER RANK SET! " + GLOBALS.s.USER_RANK); }
                        else if (ele.Name == "Gem") { Gem.ChangeTo(double.Parse(ele.GetAttribute("value"))); }
                        else if (ele.Name == "Gold") { Gold.ChangeTo(double.Parse(ele.GetAttribute("value"))); }
                        else if (ele.Name == "Elixir") { Elixir.ChangeTo(double.Parse(ele.GetAttribute("value"))); }
                        else if (ele.Name == "Sulfur") { Sulfur.ChangeTo(double.Parse(ele.GetAttribute("value"))); }
                        else if (ele.Name == "Evilness") { Evilness.ChangeTo(double.Parse(ele.GetAttribute("value"))); }
                        else if (ele.Name == "Shield") { Shield.ChangeTo(double.Parse(ele.GetAttribute("value"))); }
                        else if (ele.Name == "Building")
                        {
                            int Type = int.Parse(ele.GetAttribute("Type"));
                            int Level = int.Parse(ele.GetAttribute("Level"));

                            //Debug.Log ("Building Type:"+Type.ToString()+" Level:"+Level.ToString());
                            Building script = BEGround.instance.BuildingAdd(Type, Level);
                            script.Load(ele);
                        }
                        else { }
                    }
                }

                InLoading = false;
            }
        }

        #endregion

        public void createTownHownTutorial()
        {
            Building script = BEGround.instance.BuildingAdd(0, 1);
            Vector3 pos = new Vector3(7.1f, 0f, 16.8f);
            script.Move(pos);
            pos = new Vector3(0f, 0f, 13f);
            if (GLOBALS.s.TUTORIAL_OCCURING)
            {
                move_camera_to_building(pos, 0.5f, 14, 3f, 3f);
            }

            script.createExplosion();
            BuildingSelect(script);
            BuildingLandUnselect(true);
        }

        public void createHellGateTutorial()
        {
            Building script = BEGround.instance.BuildingAdd(4, 1);

            Vector3 pos = new Vector3(21f, 0f, 10f);
            script.Move(pos);

            Vector3 cameraPos = new Vector3(21f, 5f, 10f);

            if (GLOBALS.s.TUTORIAL_OCCURING)
            {
                move_camera_to_building(cameraPos, 0.5f, 11, -6f, -6f);
            }

            script.createExplosion();
            BuildingSelect(script);
            BuildingLandUnselect(false);
        }
    }



}
