using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class CameraTest : MonoBehaviour
{
    /*
    private static CameraTest _instance;
    public static CameraTest Instance { get { return _instance; } }

    #region Declaration

    // Debug buttons
    public GameObject[] debugButtons;

    // Sounds & Audio
    public AudioClip[] audioMatchClip = null;
    public AudioSource[] audioMatchSource = null;
    public AudioClip dialogEnter_sfx;
    public AudioSource dialogEnter_s;
    public AudioClip buttonPressed_sfx;
    public AudioSource buttonPressed_s;
    public AudioClip purchaseComplete_sfx;
    public AudioSource purchaseComplete_s;
    public AudioClip buyUpgrade_sfx;
    public AudioSource buyUpgrade_s;
    public AudioClip orderHelicopter_sfx;
    public AudioSource orderHelicopter_s;
    public AudioClip claimAchievement_sfx;
    public AudioSource claimAchievement_s;
    public AudioClip coinsEffect_sfx;
    public AudioSource coinsEffect_s;
    public AudioClip unlockLevel_sfx;
    public AudioSource unlockLevel_s;
    public AudioSource backgroundMusicSource = null;
    public string[] sounds = new string[] { "perc", "fx", "glass", "coo", "water", "cash" };

    // Map properties
    private float maxZoomIn = 110f;
    private float maxZoomOut = 850f;
    public GameObject bounds;
    private float frustumWidth;
    private float frustumHeight;
    public float boundFactor = 1.2f;
    public float dragResistFactor = 5f;
    public float bounceBackDuration = 1f;
    public float motionDuration = 0.5f;
    public float distanceForMapMotion = 50f;
    public float dragTimeForMapMotion = 0.5f;
    public float zoomSpeed = 10;
    public float cameraFocusSmoothnessOnZoom = 3f;
    public float scrollFactor = 0.8f;
    public float swipeMotionFactor = 10;
    public float blockInputAfterDialogOpenMilliseconds = 500f;

    // Internal States
    public bool waitingForLoadingScreen = true;

    private float lastZoomDistance = float.PositiveInfinity;
    private bool isBouncing = false;
    private bool inMotion = false;
    private bool isZooming = false;
    public RuntimePlatform platform;
    public bool mouseSimulation = false;
    private Vector3 zoomCenter = Vector3.zero;
    private int zoomDirection = 0;
    public GameObject mapBackground;

    public GameObject activeDialog;

    public bool firstDialogLoadingFinished = false;
    public bool startLoadingDialog = false;

    public GameObject mapHUDCoin;
    public SpriteRenderer mapHUDCoinSpriteRenderer;
    public Sprite coinNormal;
    public Sprite coinHit;

    public GameObject mapHUDPremium;
    public SpriteRenderer mapHUDPremiumSpriteRenderer;
    public Sprite PremiumNormal;
    public Sprite PremiumHit;

    //Touch states
    public bool touchStarted = false;

    //Mouse States - For Simulation Only
    private Vector3 m_touchStartPos;
    private Vector3 m_lastTouchPos;
    private Vector3 m_oneBeforeLastTouchPos;

    // Gui Elements
    public GUIStyle coinLabel;
    public Texture2D coinIcon;
    public GUIStyle cratesLabel;
    public GUIStyle energyTimerLabel;
    public Texture2D truckIcon;
    public Sprite[] objectivesIcons;
    public Sprite objectiveIcon_Ice;
    public Sprite objectiveIcon_Box;
    public Sprite objectiveIcon_Plate;
    public Sprite objectiveIcon_Happy;
    public Sprite objectiveIcon_Normal;
    public Sprite objectiveIcon_Angry;
    public Sprite objectiveIcon_Cars;
    public Sprite objectiveIcon_Louis;
    public Sprite objectiveIcon_SpecialSwiper;
    public Sprite objectiveIcon_SpecialBomb;

    public Sprite star_locked_1;
    public Sprite star_locked_2;
    public Sprite star_locked_3;
    public Sprite levelStart_Star_Locked;
    public Sprite level_locked;
    public Sprite level_unlocked;
    public Sprite level_Selected;
    public Sprite level_beforeUnlock;
    public Color level_Selected_Text_Color;

    public Avatar avatar;


    public TextMesh coins;
    public TextMesh premiumCurrency;
    public TextMesh energyTimer;
    public TextMesh energyFullText;
    public TextMesh energyAmount;

    public GameObject premiumCurrencyHUD;
    public GameObject levelReachedHUD;
    public TextMesh levelReachedTextHUD;
    public GameObject energyHUD;
    public GameObject left_HUDs;
    public GameObject bottom_left_HUDs;
    public GameObject right_HUDs;
    public GameObject supplies_HUDs_icon;

    //public TutorialItem[] tutorials;
    private GameObject[] tutorialHandTargets;

    public GameObject[] level_6_tutorial_steps_targets;


    public GameObject[] buyRestaurantTutorialsTargets;


    public GameObject orderSupplyReminderTarget;


    public GameObject achievementsReminderTarget;

    public SpriteRenderer[] areas;
    public Sprite[] areasLocked;
    public Sprite[] areasUnlocked;

    public GameObject level_unlock_animation;

    public bool blockAllButtons = true;

    public bool blockMapUpdate = false;

    public bool cameraInMotion = false;
    public float cameraMotionTime = 2f;
    private bool callTutorialCheckAfterMapMovementEnded = false;

    #endregion

    float screen_width = Screen.width;
    float screen_heigh = Screen.height;

    void Update()
    {

        if (cameraInMotion)
        {
            Debug.Log("Block update - waiting for loading screen: " + waitingForLoadingScreen + " camera in motion: " + cameraInMotion);
            return;
        }

        bool touchMove = false;

        bool touchBasedEventOccured = false;

        // Check tap / mouse input
        if ((Input.touchCount == 1 && mouseSimulation == false) || (Input.GetKey(KeyCode.Mouse0) && mouseSimulation == true))
        {
            isZooming = false;
            lastZoomDistance = float.PositiveInfinity;
            if (touchStarted == false)
            {
                touchMove = true;
                onTouchStart(Input.mousePosition);
            }
            else
            {
                touchMove = true;
                onTouchOccuring(Input.mousePosition);
            }
        }
        else if ((Input.touchCount <= 0 && mouseSimulation == false) || ((!Input.GetKey(KeyCode.Mouse0)) && mouseSimulation == true))
        {
            isZooming = false;
            lastZoomDistance = float.PositiveInfinity;
            if (touchStarted == true)
            {
                touchMove = true;
                onTouchEnd(Input.mousePosition);
            }
            else
            {
                touchStarted = false;
                zoomDirection = 0;
            }
        }

        if (Input.touchCount >= 2 && touchMove == false)
        {
            //Debug.Log("Zoom out - real touch");

            touchBasedEventOccured = true;

            if (touchStarted)
            {
                touchStarted = false;
            }

            if (inMotion || isBouncing)
            {
                isBouncing = false;
                inMotion = false;
            }

            Vector2 touch0, touch1;
            float distance;
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);

            touch0 = Input.GetTouch(0).position;
            touch1 = Input.GetTouch(1).position;

            if (isZooming == false)
            {
                //zoomCenter = (touch0 + touch1) / 2;
                zoomCenter = (touch0 - touch1) / 2;
                //zoomCenter += transform.position;
                zoomCenter = Camera.main.ScreenToWorldPoint(zoomCenter);
                zoomCenter.z = transform.position.z;
            }

            int maxZoomDirection = 3;

            distance = Vector2.Distance(touch1, touch0);
            //distance = Vector2.Distance(touch0, touch1);
            if (lastZoomDistance == float.PositiveInfinity)
            {
                lastZoomDistance = distance;
                zoomDirection = 0;
            }
            else
            {
                if (distance > lastZoomDistance && Camera.main.orthographicSize - zoomSpeed >= maxZoomIn)
                {

                    zoomDirection = Mathf.Max(zoomDirection - 1, -maxZoomDirection);
                    if (zoomDirection <= -maxZoomDirection)
                    {
                        float s = zoomSpeed * getZoomFactor();
                        Camera.main.orthographicSize = Camera.main.orthographicSize - s;
                        //transform.position = Vector3.Lerp(transform.position, zoomCenter, Time.deltaTime * cameraFocusSmoothnessOnZoom);
                    }
                }
                else if (distance < lastZoomDistance && Camera.main.orthographicSize + zoomSpeed <= maxZoomOut)
                {
                    zoomDirection = Mathf.Min(zoomDirection + 1, maxZoomDirection);
                    if (zoomDirection >= maxZoomDirection)
                    {
                        float s = zoomSpeed * getZoomFactor();
                        Camera.main.orthographicSize = Camera.main.orthographicSize + s;
                        //transform.position = Vector3.Lerp(transform.position, zoomCenter, Time.deltaTime * cameraFocusSmoothnessOnZoom);
                    }
                }
            }

            if (zoomDirection >= maxZoomDirection || zoomDirection <= -maxZoomDirection)
            {
                lastZoomDistance = distance;
            }

            isZooming = true;
            //HUDs.updateScaleRelativeToCamera();
        }

        if (mouseSimulation)
        {

            // Check pinch / mouse wheel for zoom in / out
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize + zoomSpeed <= maxZoomOut) // back
            {
                float s = zoomSpeed * getZoomFactor();
                Camera.main.orthographicSize = Camera.main.orthographicSize + s;
                touchBasedEventOccured = true;
                //Debug.Log("Camera size: " + Camera.main.orthographicSize);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize - zoomSpeed >= maxZoomIn) // forward
            {
                float s = zoomSpeed * getZoomFactor();
                Camera.main.orthographicSize = Camera.main.orthographicSize - s;
                touchBasedEventOccured = true;
                //Debug.Log("Camera size: " + Camera.main.orthographicSize);
                //Debug.Log ("Bounds " + bounds.renderer.bounds.size);
            }
        }

    }


    private void onTouchStart(Vector3 touch)
    {

        if (inMotion || isBouncing || isZooming)
        {
            //tweenSequence.Kill();
            isBouncing = false;
            inMotion = false;
            isZooming = false;
        }

        //Debug.Log ("Touch Started: " + touch.ToString());
        touchStarted = true;
        m_touchStartPos = touch;
        m_lastTouchPos = m_touchStartPos;
        m_oneBeforeLastTouchPos = m_lastTouchPos;
    }

    private float getZoomFactor()
    {
        float zoomFactor = 1;
        zoomFactor = (Camera.main.orthographicSize * 0.2f) / 100f;
        return zoomFactor;
    }


    private void onTouchOccuring(Vector3 touch)
    {

        if (isBouncing || isZooming || inMotion) return;

        Vector3 tmp = (m_lastTouchPos - touch);

        tmp *= getZoomFactor();

        Vector3 tmpOrig = tmp;

       
        float minX = transform.position.x - screen_width / 2;
        float maxX = transform.position.x + screen_width/ 2;
        float maxY = transform.position.y + screen_heigh / 2;
        float minY = transform.position.y - screen_heigh / 2;

        float minHardX = minX * boundFactor;
        float maxHardX = maxX * boundFactor;
        float minHardY = minY * boundFactor;
        float maxHardY = maxY * boundFactor;


        tmp = transform.position + tmp;

        float x = tmp.x;
        float y = tmp.y;

        if (tmp.x > maxHardX)
        {
            x = maxHardX;
        }
        else if (tmp.x < minHardX)
        {
            x = minHardX;
        }
        if (tmp.y > maxHardY)
        {
            y = maxHardY;
        }
        else if (tmp.y < minHardY)
        {
            y = minHardY;
        }

        if (tmp.x > maxX)
        {
            float currentResist = dragResistFactor * (tmp.x / maxHardX);
            tmp = tmpOrig;
            x = transform.position.x + (tmp.x / currentResist);
        }
        else if (tmp.x < minX)
        {
            float currentResist = dragResistFactor * (tmp.x / minHardX);
            tmp = tmpOrig;
            x = transform.position.x + (tmp.x / currentResist);
        }

        if (tmp.y > maxY)
        {
            float currentResist = dragResistFactor * (tmp.y / maxHardY);
            tmp = tmpOrig;
            y = transform.position.y + (tmp.y / currentResist);
        }
        else if (tmp.y < minY)
        {
            float currentResist = dragResistFactor * (tmp.y / minHardY);
            tmp = tmpOrig;
            y = transform.position.y + (tmp.y / currentResist);
        }

        tmp = new Vector3(x, y, tmp.z);

        transform.position = tmp;

        m_oneBeforeLastTouchPos = m_lastTouchPos;

        m_lastTouchPos = touch;
    }

    private bool closeRestaurantIfOpen = false;
    private void onTouchEnd(Vector3 touch)
    {

        if (isBouncing || isZooming || inMotion || activeDialog != null)
        {
            isZooming = false;
            touchStarted = false;
            return;
        }

        closeRestaurantIfOpen = true;

        //Debug.Log ("Touch Ended: " + touch.ToString());
        Vector3 tmp = transform.position;
        touchStarted = false;
        isZooming = false;

        float minX = transform.position.x - screen_width;
        float maxX = transform.position.x + screen_width;
        float maxY = transform.position.y + screen_heigh;
        float minY = transform.position.y - screen_heigh;

        bool bounce = false;

        if (tmp.x > maxX)
        {
            tmp.x = maxX;
            bounce = true;
        }
        else if (tmp.x < minX)
        {
            tmp.x = minX;
            bounce = true;
        }
        if (tmp.y > maxY)
        {
            tmp.y = maxY;
            bounce = true;
        }
        else if (tmp.y < minY)
        {
            tmp.y = minY;
            bounce = true;
        }

        if (bounce)
        {
            Debug.Log("IT SHOULD BE BOUNCING!! BOUNCING ALL NIGHT!");

            //TweenParms parms = new TweenParms().Prop("localPosition", tmp).Ease(EaseType.EaseOutElastic).OnComplete(repositionMapAfterBounce);
          //  Camera.main.transform.DOMove(, bounceBackDuration, parms);


            isBouncing = true;
            
        }
        else
        {

            //			float distance = Vector3.Distance(m_oneBeforeLastTouchPos, m_lastTouchPos);
            //
            //			//Vector3 target = ((m_touchStartPos - touch) * scrollFactor) * swipeMotionFactor * distance;
            //			Vector3 target = (((m_touchStartPos - m_oneBeforeLastTouchPos) / 2f) * scrollFactor * getZoomFactor()) * swipeMotionFactor;
            //
            //			target += transform.position;
            //
            //			if(target.x > maxX) {
            //				target.x = maxX;
            //			} else if(target.x < minX) {
            //				target.x = minX;
            //			}
            //			if(target.y > maxY) {
            //				target.y = maxY;
            //			} else if(target.y < minY) {
            //				target.y = minY;
            //			}
            //
            //			TweenParms parms = new TweenParms().Prop("localPosition", target).Ease(EaseType.Linear).OnComplete(repositionMapAfterBounce);
            //			tweenSequence = new Sequence();
            //			tweenSequence.Append (HOTween.To(camera.transform, 0.5f, parms ));
            //			tweenSequence.Play();
            //			//Debug.Log("Start map motion");
            //			inMotion = true;
        }

    }

    private void repositionMapAfterBounce()
    {

        Vector3 tmp = Camera.main.transform.position;

        float minX = transform.position.x - screen_width;
        float maxX = transform.position.x + screen_width / 2;
        float maxY = transform.position.y + screen_heigh / 2;
        float minY = transform.position.y - screen_heigh / 2;

        if (tmp.x > maxX)
        {
            tmp.x = maxX;
        }
        else if (tmp.x < minX)
        {
            tmp.x = minX;
        }
        if (tmp.y > maxY)
        {
            tmp.y = maxY;
        }
        else if (tmp.y < minY)
        {
            tmp.y = minY;
        }

        isBouncing = false;
        inMotion = false;
        isZooming = false;
        Camera.main.transform.position = tmp;
    }*/
}
