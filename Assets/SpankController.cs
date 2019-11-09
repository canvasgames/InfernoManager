using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;


public class SpankController : MonoBehaviour {

    #region Variables Declaration

    public static SpankController s;
    public Transform Canvas;

    List<GameObject> all = new List<GameObject>();

    GameObject body;

    GameObject handTutLeft;
    GameObject handTutRight;
    float handLeftX;
    float handRightX;
    GameObject arrowLeft;
    GameObject arrowRight;
    GameObject handSpank;

    GameObject slap;
    GameObject handSlapper = null;
    GameObject painFace;

    //texts
    GameObject textTitle;
    GameObject textSlap;
    GameObject textTime;

    //states and control variables
    bool touchActive = false;
    bool swiping = false;
    bool swiped = false;
    bool firstTime = false;

    float swipeXpos = 0;
    float centerXpos = 0;

    float shakeAmount = 8f;

    int nSlaps = 0;
    float timeLeft;
    float shakeTimeLeft;

    void Awake() { s = this; }

    public float tempo = 1f;
    public float forca = 5f;
    public int vibrato = 10;

    #endregion

    void Start() {
        //StartAnimation();
        centerXpos = Screen.width / 2 - 50;
    }

    void Update() {
        if (GLOBALS.s != null && GLOBALS.s.SPANKING_OCURRING && handSlapper != null) {
            if (firstTime)
                timeLeft -= Time.deltaTime;
            if (timeLeft > 0) {
                textTime.GetComponent<Text>().text = "Time Left: " + timeLeft.ToString("F2");
                

                // Shaking animation
                //body.transform.localPosition = Random.insideUnitCircle * shakeAmount;

                // Spanking hand
                if (Input.GetMouseButtonDown(0)) {
                    handSlapper.SetActive(true);
                    if (Input.mousePosition.x > centerXpos)
                        handSlapper.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    else {
                        handSlapper.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
                        //Debug.Log(" INVERTED CASE!" );
                    }
                }
                if (handSlapper.activeInHierarchy == true && Input.GetMouseButton(0)) {
                    handSlapper.transform.position = Input.mousePosition;
                    //Debug.Log(" MOUSE X: " + Input.mousePosition.x + " Screen center: " + Screen.width / 2);
                }
                if (Input.GetMouseButtonUp(0)) {
                    handSlapper.SetActive(false);
                    Debug.Log(" MOUSE UP !");
                }

                if (touchActive && !swiping && Input.GetMouseButtonDown(0)) {
                    swiping = true;
                    Debug.Log(" SWIPE START");
                    swipeXpos = Input.mousePosition.x;
                }

                else if (swiping && Input.GetMouseButton(0)) {
                    //Debug.Log(" swiiiiiiping!" );
                    if ((swipeXpos < centerXpos && Input.mousePosition.x >= swipeXpos + 100) || (swipeXpos > centerXpos && Input.mousePosition.x <= swipeXpos - 100)) {
                        SlapThatAss();
                        swipeXpos = 0;
                        //touchActive = false;
                        swiping = false;
                        //swiped = true;
                    }
                }
            }
            else {
                textTime.GetComponent<Text>().text = "Time Left: 0";
                EndEverthing();
            }
                // handSlapper.transform.position = Input.mousePosition;
            
        }
    }

    #region Animation Start
    public void StartAnimation() {
        GLOBALS.s.SPANKING_OCURRING = true;

        timeLeft = 10;
        nSlaps = 0;
        //display slaps here
        //texts
        #region Instantiations

        //body
        body = (GameObject)Instantiate(Resources.Load("Prefabs/Spank/Body"));
        body.transform.SetParent(Canvas, false);
        all.Add(body);

        //pain face
        painFace = (GameObject)Instantiate(Resources.Load("Prefabs/Spank/PainFace"));
        painFace.SetActive(false);
        painFace.transform.SetParent(Canvas, false);
        //all.Add(painFace);
        painFace.transform.parent = body.transform;


        // tutorial stuff
        arrowLeft = (GameObject)Instantiate(Resources.Load("Prefabs/Spank/ArrowSlapLeft"));
        arrowLeft.transform.SetParent(Canvas, false);
        arrowLeft.SetActive(true);
        arrowLeft.GetComponent<CanvasGroup>().alpha = 0;
        arrowLeft.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        all.Add(arrowLeft);


        handTutLeft = (GameObject)Instantiate(Resources.Load("Prefabs/Spank/handTutLeft"));
        handTutLeft.transform.SetParent(Canvas, false);
        handTutLeft.SetActive(true);
        handTutLeft.GetComponent<CanvasGroup>().alpha = 0;
        handTutLeft.GetComponent<CanvasGroup>().DOFade(1, 0.5f).OnComplete(() => hand_visible());
        handLeftX = handTutLeft.transform.localPosition.x;
        all.Add(handTutLeft);


        arrowRight = (GameObject)Instantiate(Resources.Load("Prefabs/Spank/ArrowSlapRight"));
        arrowRight.transform.SetParent(Canvas, false);
        arrowRight.SetActive(true);
        arrowRight.GetComponent<CanvasGroup>().alpha = 0;
        arrowRight.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        all.Add(arrowRight);

        handTutRight = (GameObject)Instantiate(Resources.Load("Prefabs/Spank/handTutRight"));
        handTutRight.transform.SetParent(Canvas, false);
        handTutRight.SetActive(false);
        /*handTutRight.SetActive(true);
        handTutRight.GetComponent<CanvasGroup>().alpha = 0;
        handTutRight.GetComponent<CanvasGroup>().DOFade(1, 0.9f);*/
        handRightX = handTutRight.transform.localPosition.x;
        all.Add(handTutRight);
        #endregion

         //texts
        textTitle = (GameObject)Instantiate(Resources.Load("Prefabs/Spank/TextTitle"));
        textTitle.SetActive(true);
        textTitle.transform.SetParent(Canvas, false);
        all.Add(textTitle);

        textTime = (GameObject)Instantiate(Resources.Load("Prefabs/Spank/TextTimeLeft"));
        textTime.transform.SetParent(Canvas, false);
        textTime.SetActive(false);
        all.Add(textTime);

        textSlap = (GameObject)Instantiate(Resources.Load("Prefabs/Spank/TextSlaps"));
        textSlap.transform.SetParent(Canvas, false);
        textSlap.GetComponent<Text>().text = "Slaps: " + nSlaps;
        textSlap.SetActive(false);
        all.Add(textSlap);

        //hand
        handSlapper = (GameObject)Instantiate(Resources.Load("Prefabs/Spank/Hand"));
        handSlapper.transform.SetParent(Canvas, false);
        handSlapper.SetActive(false);
        all.Add(handSlapper);

        touchActive = true;
        firstTime = false;

        Invoke("fade_in_right_hand", 0.5f);

    }

    public void fade_in_right_hand() {
        handTutRight.SetActive(true);
        handTutRight.GetComponent<CanvasGroup>().alpha = 0;
        handTutRight.GetComponent<CanvasGroup>().DOFade(1, 0.5f).OnComplete(() => hand_visible_right());
    }

    public void hand_visible() {
        if (GLOBALS.s.SPANKING_OCURRING) {
            handTutLeft.transform.localPosition = new Vector3(handLeftX, handTutLeft.transform.localPosition.y);
            //handTutRight.transform.localPosition = new Vector3(handRightX, handTutRight.transform.localPosition.y);
            Invoke("hand_start_moving", 0.3f);
        }
    }
    public void hand_visible_right() {
        if (GLOBALS.s.SPANKING_OCURRING) {
           // handTutLeft.transform.localPosition = new Vector3(handLeftX, handTutLeft.transform.localPosition.y);
            handTutRight.transform.localPosition = new Vector3(handRightX, handTutRight.transform.localPosition.y);
            Invoke("hand_start_right_moving", 0.3f);
        }
    }

    void hand_start_moving() {
        if (GLOBALS.s.SPANKING_OCURRING) {
            //handTutRight.transform.DOLocalMoveX(handTutRight.transform.localPosition.x - 266, 0.5f).OnComplete(() => restart_hand());
            handTutLeft.transform.DOLocalMoveX(handTutLeft.transform.localPosition.x + 266, 0.5f).OnComplete(() => restart_hand());
        }
    }
    void hand_start_right_moving() {
        if (GLOBALS.s.SPANKING_OCURRING) {
            handTutRight.transform.DOLocalMoveX(handTutRight.transform.localPosition.x - 266, 0.5f).OnComplete(() => restart_hand_right());
           // handTutLeft.transform.DOLocalMoveX(handTutLeft.transform.localPosition.x + 266, 0.5f);
        }
    }

    public void restart_hand() {
        Invoke("hand_visible", 0.25f);
        
    }

    public void restart_hand_right() {
        Invoke("hand_visible_right", 0.25f);
        if (!firstTime) {
            firstTime = true;
            textTime.SetActive(true);
            textSlap.SetActive(true);
            textTitle.SetActive(false);
        }
    }

    #endregion

    #region Slap Animation Trigger
    public void SlapThatAss() {
        //MenusController.s.destroyMenu("SmallScroll", null);

        handTutLeft.SetActive(false);
        handTutRight.SetActive(false);
        arrowLeft.SetActive(false);
        arrowRight.SetActive(false);

        nSlaps++;
        textSlap.GetComponent<Text>().text = "Slaps: " + nSlaps;

        Debug.Log("\nSSSSSSSSSSSLAP THAT ASS");
        Debug.Log("SSSSSSSSSSSLAP THAT ASS");
        Debug.Log("SSSSSSSSSSSLAP THAT ASS\n");

        GameObject sslap = (GameObject)Instantiate(Resources.Load("Prefabs/Spank/Slap"));
        sslap.transform.localPosition = new Vector2(sslap.transform.localPosition.x + Random.Range(-100,100), sslap.transform.localPosition.y + Random.Range(-100, 100));
        sslap.SetActive(true);
        sslap.transform.SetParent(Canvas, false);
        sslap.transform.DOScale(new Vector3(1.5f, 1.5f, 1), 0.5f);
        sslap.GetComponent<CanvasGroup>().DOFade(0, 0.6f).OnComplete(() => Destroy(sslap)); //sslap.SetActive(false));

        painFace.SetActive(true);
        CancelInvoke("SlapEnd");
        Invoke("SlapEnd", 0.45f);

        body.transform.DOShakePosition(tempo,forca, vibrato);
        /*
        camera.transform.localPosition = Random.insideUnitCircle * shakeAmount;
        shake -= Time.deltaTime * decreaseFactor;
        reoganize -= Time.deltaTime * decreaseFactor;
        if (reoganize < 0) {
            wordCTRL[0].reorganize();
            reoganize = reorgInterval;
        }*/
    }

    void SlapEnd() {
        if (GLOBALS.s.SPANKING_OCURRING) 
            painFace.SetActive(false);
    }

    #endregion

    void EndEverthing() {
        Invoke("EndEverythingForReal", 0.5f);
        GLOBALS.s.SPANKING_OCURRING = false;
    }
    void EndEverythingForReal() {
        foreach (GameObject item in all)
            Destroy(item);

        MissionsController.s.RewardMisison(MissionType.Spank);
    }
}
