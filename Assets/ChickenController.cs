using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;


public class ChickenController : MonoBehaviour {
    
    #region Variables Declaration

    public static ChickenController s;
    public Transform Canvas;
    GameObject chicken;
    GameObject handTut;
    GameObject arrow;
    GameObject boot;
    GameObject pow;

    bool touchActive = false;
    bool swiping = false;
    bool swiped = false;

    void Awake() { s = this; }

    #endregion

    void Start()
    {
        //start_animation();
    }

    void Update()
    {
        if (!swiped) { 
            if (touchActive && Input.GetMouseButtonDown(0) && Input.mousePosition.x <= arrow.transform.position.x)
                swiping = true;
            else if (swiping && Input.GetMouseButtonUp(0))
            {
                swiping = false;
                if (Input.mousePosition.x >= arrow.transform.position.x + arrow.GetComponent<RectTransform>().rect.width / 4)
                {
                    kick_the_baby();
                    touchActive = false;
                    swiped = true;
                }
            }
        }
    }

    #region Animation Start
    public void start_animation()
    {
        Debug.Log(" COCOOOOOOOOOOO");
        chicken = (GameObject)Instantiate(Resources.Load("Prefabs/Chicken/Chicken"));
        chicken.transform.SetParent(Canvas, false);

        arrow = (GameObject)Instantiate(Resources.Load("Prefabs/Chicken/Arrow"));
        arrow.transform.SetParent(Canvas, false);
        arrow.SetActive(true);
        arrow.GetComponent<CanvasGroup>().alpha = 0;
        arrow.GetComponent<CanvasGroup>().DOFade(1, 0.9f);
        //chicken.SetActive(true);

        handTut = (GameObject)Instantiate(Resources.Load("Prefabs/Chicken/handTut"));
        handTut.transform.SetParent(Canvas, false);
        handTut.SetActive(true);
        handTut.GetComponent<CanvasGroup>().alpha = 0;
        handTut.GetComponent<CanvasGroup>().DOFade(1, 0.9f).OnComplete(() => hand_visible());
        //arrow.get
        //chicken.SetActive(true);
    }

    public void hand_visible()
    {
        handTut.transform.localPosition = new Vector3(-435, handTut.transform.localPosition.y);
        Invoke("hand_start_moving", 0.3f);
    }

    void hand_start_moving()
    {
        handTut.transform.DOLocalMoveX(109, 0.7f).OnComplete(() => restart_hand());
    }
    public void restart_hand()
    {
        Invoke("hand_visible", 0.3f);
        touchActive = true;
    }

    #endregion

    #region Boot kicking animation
    public void kick_the_baby()
    {
        MenusController.s.destroyMenu("SmallScroll", null);


        handTut.SetActive(false);
        arrow.SetActive(false);

        Debug.Log("KICK THE BABY");
        boot = (GameObject)Instantiate(Resources.Load("Prefabs/Chicken/Boot"));
        boot.transform.SetParent(Canvas, false);
        boot.SetActive(true);

        boot.transform.DORotate(new Vector3(0, 0, 5), 0.7f).OnComplete(() =>
            boot.transform.DORotate(new Vector3(0, 0, -120), 0.5f).OnComplete(() =>
            start_kicking_for_real()));
    }

    void start_kicking_for_real()
    {
        //boot.transform.DORotate(new Vector3(0, 0, 50), 0.5f).OnComplete(() => boot_at_the_chicken());
        boot.transform.DORotate(new Vector3(0, 0, 490), 0.5f, RotateMode.FastBeyond360);
        Invoke("boot_at_the_chicken", 0.2f);
    }

    void boot_at_the_chicken()
    {
        //boot.transform.DORotate(new Vector3(0, 0, 100), 0.8f);
        
        pow = (GameObject)Instantiate(Resources.Load("Prefabs/Chicken/Pow"));
        pow.SetActive(true);
        pow.transform.SetParent(Canvas, false);
        pow.transform.DOScale(new Vector3(1.5f, 1.5f, 1), 0.5f);
        pow.GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete(() => pow.SetActive(false));

        //chicken.GetComponent<Animator>().SetTrigger("kick");
        chicken.transform.DOLocalMoveX(chicken.transform.position.x + 300, 0.55f).OnComplete(() => chicken_kicked_success());
        Vector3 abc = new Vector3(0, 0, 310);
        chicken.transform.DOLocalRotate(abc, 0.55f);

        BE.BEAudioManager.SoundPlay(11);
    }

    #endregion

    void chicken_kicked_success()
    {
        //Invoke("chicken_kicked_sucess_timer", 0.55f);
        boot.SetActive(false);
        chicken.SetActive(false);

        Invoke("call", 1.1f);


        // call tutorial back
    }

    void call()
    {
        TutorialController.s.after_chicken_kicked();
    }
        

}
