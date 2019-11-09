using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class CatastropheAnimationController : MonoBehaviour {

   public static CatastropheAnimationController s;

    public GameObject WarningAlert;

    public GameObject TvFrame;
    public GameObject TvStatic;

    public GameObject TvCityBG;
    public GameObject TvCityMask;

    public GameObject CatDestroyer;
    public GameObject[] Lasers;

    public GameObject Explosion;
    public GameObject Explosion2;
    public GameObject Explosion3;
    public GameObject Fire;
    public GameObject Fire2;
    public GameObject Fire3;

    public GameObject button;

    bool already_invoked = false;
    private int warning_count = 0;
    private int count = 0;

	// Use this for initialization
	void Start () {
        GLOBALS.s.DIALOG_ALREADY_OPENED = true;
        s = this;
        WarningAlert.SetActive(false);
        TvFrame.SetActive(false);
        TvStatic.SetActive(false);
        TvCityMask.SetActive(false);
        TvCityBG.SetActive(false);
        CatDestroyer.SetActive(false);
        Explosion.SetActive(false);
        button.SetActive(false);

        //display_warning();
        Invoke("display_warning", 0.5f);
	}
	

    #region =========== Warning ===============

    void display_warning()
    {
        WarningAlert.SetActive(true);
        //MenusController.s.moveMenu(MovementTypes.Left,WarningAlert, "WarningEnteringAnimation", 0, 0,"",false,true);

        Invoke("warning_animation", 0.7f);
    }

    void warning_animation()
    {
        if(warning_count < 7)
        {
            warning_count++;
            if(warning_count % 2 == 0)
                WarningAlert.SetActive(true);
            else
                WarningAlert.SetActive(false);
            Invoke("warning_animation", 0.2f);
        }
        else
        {
            tv_entering();
        }
    }

    #endregion

    #region ========== TV Entering =============
    void tv_entering()
    {
        Debug.Log("TV ENTERING");
        WarningAlert.SetActive(false);
        TvStatic.SetActive(true);
        TvFrame.SetActive(true);

       // MenusController.s.moveMenu(MovementTypes.Left, TvStatic, "TvStaticEnteringAnimation", 0, 0, "", false, true);
       // MenusController.s.moveMenu(MovementTypes.Left, TvFrame, "TvEnteringAnimation", 0, 0, "", false, true);
        Invoke("tv_entering_finished", 1f);
    }

    //make the cat appear
    void tv_entering_finished()
    {
        TvCityBG.SetActive(true);
        TvCityMask.SetActive(true);
        TvStatic.SetActive(false);
        Debug.Log(" entering finished!!");

        Invoke("go_go_cat_destroyer", 0.5f);
        // menu.transform.localPosition = new Vector3(xPos, yPos - Screen.height, 0f);
        
        //Move back to the screen and call punch at the end
        //menu.transform.DOLocalMoveY(yPos, 0.5f).OnComplete(() => punchDown(menu));
    }

    #endregion

    #region cat_entering
    void go_go_cat_destroyer()
    {
        CatDestroyer.SetActive(true);
        float ypos = CatDestroyer.transform.localPosition.y;
        CatDestroyer.transform.localPosition = new Vector2(CatDestroyer.transform.localPosition.x,
            CatDestroyer.transform.localPosition.y - CatDestroyer.GetComponent<RectTransform>().rect.height);
        CatDestroyer.transform.DOLocalMoveY(ypos, 0.5f).OnComplete(() => cat_destroyer_entering_finished());
    }

    void cat_destroyer_entering_finished()
    {
        count = 0;
        Lasers[0].SetActive(true);
        Lasers[1].SetActive(true);

        CatDestroyer.GetComponent<cat_animation_script>().start_shaking();
        
        Invoke("laser_blink_once", 0.10f);
    }

    void laser_blink_once()
    {
        if (count % 2 == 0)
        {
            Lasers[0].SetActive(false);
            Lasers[1].SetActive(false);
        }
        else
        {
            Lasers[0].SetActive(true);
            Lasers[1].SetActive(true);
        }

        count++;
        if (count < 3)
            Invoke("laser_blink_once", 0.10f);
        else {
            Invoke("laser_blink_once", 1.1f);
            count = -1;
            if (already_invoked == false)
            {
                Invoke("start_explosions", 0.15f);
                already_invoked = true;
            }
        }
    }

    #endregion

    #region =========== EXPLOSIONS ==============
    void start_explosions() {
        Explosion.SetActive(true);
        Invoke("start_fire1", 0.4f);

        Invoke("start_explosion2", 0.75f);
        Invoke("start_explosion3", 1.3f);

        Invoke("close_everything", 3.5f);
    }

    void start_explosion2()
    {
        Explosion2.SetActive(true);
        Invoke("start_fire2", 0.4f);
    }

    void start_explosion3(){
        Explosion3.SetActive(true);
        Invoke("start_fire3", 0.4f);
    }

    void start_fire1()
    {
        Fire.SetActive(true);
    }
    void start_fire2()
    {
        Fire2.SetActive(true);
    }
    void start_fire3()
    {
        Fire3.SetActive(true);
        
    }

    #endregion

    void close_everything(){
        // fazer o label ir pra cima, destruir tudo e criar um dialogo (scroll) pra coletar almas
        button.SetActive(true);
    }

}
