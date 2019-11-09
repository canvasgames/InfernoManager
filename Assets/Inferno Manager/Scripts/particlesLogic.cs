using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class particlesLogic : MonoBehaviour {
    Transform finalPos, BigD;
    Vector3 initialPos;
    GameObject puft;
    float xPosLoc, yPosLoc;
    // Use this for initialization
    void Start () {
	
	}

    public void move(Transform daddy, Transform fPos, Vector3 iPos, int moveType, int numberOfParticles)
    {
        finalPos = fPos;
        BigD = daddy;
        initialPos = iPos;

        transform.position = new Vector3(initialPos.x, initialPos.y, daddy.transform.position.z);
        float angle;
        angle = (moveType * (360/ numberOfParticles)) *Mathf.Deg2Rad;
        angle = angle + (Random.Range(-10f, 10f));
        //Debug.Log(Camera.main.orthographicSize);
        //Debug.Log("Cos" + (-(Mathf.Cos(angle))) + "     Angle " + angledegrree);

        transform.DOMoveX((-(Mathf.Cos(angle)* Mathf.Rad2Deg) * (Random.Range(0.7f,1.3f)) * (6 / (Camera.main.orthographicSize)) ) + initialPos.x, 0.2f).OnComplete(waitForIt);
        transform.DOMoveY((-(Mathf.Sin(angle) * Mathf.Rad2Deg) * (Random.Range(0.7f, 1.3f)) * (6 / (Camera.main.orthographicSize)) ) + initialPos.y,0.2f);
        transform.DOScale(6 / (Camera.main.orthographicSize), 0f);
    }

    void waitForIt()
    {
        Invoke("moveToTop", 0.1f);
    }

    void moveToTop()
    {
        transform.DOMove(finalPos.transform.position, 0.7f).OnComplete(createsmoke);
        transform.DOScale(0.7f, 0.5f);

    }
    void createsmoke()
    {
        puft = (GameObject)Instantiate(Resources.Load("Prefabs/smoke"));
        puft.transform.SetParent(BigD, false);
        puft.transform.localPosition = transform.localPosition;
        puft.transform.localScale = new Vector3(0f, 0f, 0f);

        Invoke("smokeGrow", 0.1f);
    }

    void smokeGrow()
    {
        puft.transform.DOScale(1f, 0.2f).OnComplete(destroyObj);
    }

    void destroyObj()
    {
        Destroy(transform.gameObject);
        //puft.transform.DOScale(0.5f, 0.1f).OnComplete(destroySmoke);
        puft.GetComponent<Image>().DOFade(0f,0.4f).OnComplete(destroySmoke); ;
    }

    void destroySmoke()
    {
        Destroy(puft,0f);
    } 

// Update is called once per frame
    void Update ()
    {

    }

    public void moveCatrastofe()
    {
        GameObject fPos = GameObject.Find("ElixirIcon"); ;
         finalPos = fPos.transform;

        GameObject daddy = GameObject.Find("Canvas");
        BigD = daddy.transform;

        transform.DOMove(finalPos.transform.position, 1f).OnComplete(createsmoke); ;
        transform.DOScale(0.7f, 0.7f);
    }

    void createSmokeCat()
    {
        Debug.Log("Creating smoke");
        puft = (GameObject)Instantiate(Resources.Load("Prefabs/smoke"));

        puft.transform.SetParent(BigD, false);
        puft.transform.localPosition = transform.localPosition;
        puft.transform.localScale = new Vector3(0f, 0f, 0f);

        Invoke("smokeGrow", 0.1f);
    }
}
