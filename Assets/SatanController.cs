using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;


public class SatanController : MonoBehaviour {
    public static SatanController s;
    public Transform Canvas;
    private GameObject Satan;
    private GameObject CatExplosion;

    GameObject FireBg;

    void Awake() { s = this; }
	// Use this for initialization
	void Start () {

        //Invoke("satan_entering_animation", 1f);

        Satan = (GameObject)Instantiate(Resources.Load("Prefabs/Satan/SatanEntering"));
        Satan.transform.SetParent(Canvas, false);
        Satan.SetActive(false);

        //CatExplosion = (GameObject)Instantiate(Resources.Load("Prefabs/CatastropheExplosion"));
        //CatExplosion.transform.SetParent(Canvas, false);

        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void start_entering(float delay)
    {
        Invoke("satan_entering_animation", delay);
    }

    public void satan_entering_animation()
    {
        FireBg = (GameObject)Instantiate(Resources.Load("Prefabs/FireBg"));
        FireBg.SetActive(false);
        FireBg.transform.SetParent(Canvas, false);

        Satan.SetActive(true);
       

        //Invoke("satan_finished_appearing", 1f);
    }

    public void satan_started_talking()
    {
        TextWriter.s.write_text(Satan.GetComponentInChildren<Text>(), "Behold the Great Satan!!\nRuler of the 9 Hells!", 0.02f, 0.1f);
        Invoke("satan_finished_talking", 2.3f);
        //Satan.GetComponentInChildren<Text>().text = "Behold the Great Satan!!\nRuler of the 9 Hells!";
    }

    public void satan_finished_talking()
    {
        Debug.Log("FINISHED TALKING");
        Satan.GetComponentInChildren<Text>().text = "";
    }

    /*
    public void satan_finished_appearing()
    {
        //FireBg.SetActive(true);
        //FireBg.GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() => fire_finished_fading());
        //Satan.GetComponent<Animation>().Stop();
        //Invoke("satan_back_to_cape", 0.4f);
        
     }

    
    public void satan_back_to_cape()
    {
        //Debug.Log(" ASASASASASASAS");
        
        //CatExplosion.SetActive(true);
        //CatExplosion.transform.localPosition = new Vector2(0, 0);

        //Satan.GetComponent<Animation>().Stop();
        //Satan.GetComponent<CanvasGroup>().DOFade(0, 0.4f);
        Satan.SetActive(false);
        Invoke("explosion_finished", 0.4f);
    }

    public void explosion_finished()
    {
        //Satan.transform.localPosition = new Vector3(-236f, -129f, 0);
        //Satan.transform.localScale = new Vector3(0.5f, 0.5f, 0);

        Satan.SetActive(false);

        CatExplosion.SetActive(false);
    }



    public void fire_finished_fading()
    {
        Satan.transform.DOLocalMove(new Vector3(-236f, -129f, 0), 0.5f);
        Satan.transform.DOScale(new Vector3(0.5f, 0.6f, 0), 0.5f);
    } 
    */


    public void satan_vanished()
    {
        TutorialController.s.startTutorial();
        Destroy(Satan);

    }
}
