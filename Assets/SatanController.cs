using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;


public class SatanController : MonoBehaviour {
    public static SatanController s;
    public Transform Canvas;
    private GameObject Satan;
    private GameObject CatExplosion;

    public Text introText;

    [Header("SatanStates")]
    public GameObject SatanIntro;
    public GameObject SatanIntro2;
    public GameObject SatanChallenging;
    public GameObject SatanExplaning;
    public GameObject SatanBragging;
    public GameObject SatanComplaining;

    void Awake() { s = this; }

	
    public void StartSatanIntro(float delay)
    {
        Invoke("IntroAnimation", delay);
    }

    void IntroAnimation()
    {
        SatanIntro.SetActive(true);
        StartCoroutine(SatanStartedTalk());
        introText.gameObject.SetActive(true);
    }

    IEnumerator SatanStartedTalk()
    {
        yield return new WaitForSeconds(1.5f);
        TextWriter.s.write_text(introText, "Behold the Great Satan!!\nRuler of the 9 Hells!", 0.02f, 0.1f);
        StartCoroutine(SatanFinishTalk());
    }

    IEnumerator SatanFinishTalk()
    {
        yield return new WaitForSeconds(3.5f);
        introText.text = "";
        introText.gameObject.SetActive(false);
    }
}
