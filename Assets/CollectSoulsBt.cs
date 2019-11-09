using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;





public class CollectSoulsBt : ButtonCap {
    public GameObject allObjects, souls;

    [HideInInspector]public int quantityToCollect;
    public CollectSoulsType myBtType;
    bool alreadyClicked = false;
    // Use this for initialization
    void Start() {
        quantityToCollect = 100;
    }

    public override void ActBT() {
        if(alreadyClicked == false)
        {
            alreadyClicked = true;
            BE.SceneTown.instance.CapacityCheck();

            souls = (GameObject)Instantiate(Resources.Load("Prefabs/SoulsForCollect"));
            souls.transform.SetParent(MenusController.s.bigDaddy, false);

            souls.SetActive(true);
            particlesLogic[] particles;
            particles = souls.GetComponentsInChildren<particlesLogic>() as particlesLogic[];
            souls.transform.GetComponent<CanvasGroup>().DOFade(1f, 0.1f);
            foreach (particlesLogic part in particles)
            {
                part.moveCatrastofe();
            }

            if (myBtType == CollectSoulsType.MissionSpank || myBtType == CollectSoulsType.MissionBuild || myBtType == CollectSoulsType.MissionCollect)
            {
                transform.DOScale(0f, 0.6f);
                Debug.Log("Invoking");

                int CapacityTotal = BE.BEGround.instance.GetCapacityTotal(BE.PayType.Elixir);

                double actualvalue = BE.SceneTown.Elixir.ChangeDelta((double)quantityToCollect);

                if (CapacityTotal >= actualvalue + quantityToCollect)
                {
                    BE.SceneTown.instance.GainExp((int)quantityToCollect);
                    MissionsController.s.OnSoulsCollected(quantityToCollect);
                    distributeSouls(100);
                }
                else
                {

                    BE.SceneTown.instance.GainExp(CapacityTotal - (int)actualvalue);
                    MissionsController.s.OnSoulsCollected(CapacityTotal - (int)actualvalue);
                    distributeSouls(CapacityTotal - (int)actualvalue);
                }

                //allObjects.transform.DOMoveY(-450, 1f).OnComplete(() => M);
                Invoke("onAnimationEnd", 1.5f);
            }
        }

    }

    void onAnimationEnd() {
        Debug.Log("Invoked");

        MissionsController.s.OnSoulsCollected(myBtType);
    }



    void distributeSouls(float value)
    {
        BE.Building[] buildings;
        buildings = GameObject.FindObjectsOfType(typeof(BE.Building)) as BE.Building[];
        int lenght = buildings.Length;
        float discountEach = 1;
        int i = 0, cont = 0;
        float temp;
        while (value > 0)
        {
            for (i = 0; i < lenght; i++)
            {
                if (value >= 1)
                    temp = buildings[i].storeSouls(discountEach,666, 666, 666);
                else
                    temp = buildings[i].storeSouls(value, 666, 666, 666);

                value = value - temp;
                if (value == 0f)
                {
                    break;
                }

            }

            cont++;
            if (cont > 1000)
            {
                break;
            }
            i = 0;
        }
    }
}
