using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class collectSouls : MonoBehaviour {
    public GameObject allObjects, souls;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//if(Input.getmo
	
	}

    public void clicked()
    {

        int CapacityTotal = BE.BEGround.instance.GetCapacityTotal(BE.PayType.Elixir);
     
        double actualvalue = BE.SceneTown.Elixir.ChangeDelta((double)100);

        if(CapacityTotal >= actualvalue + 100)
        {
            BE.SceneTown.instance.GainExp((int)100);
            MissionsController.s.OnSoulsCollected(100);
            distributeSouls(100f);
        }
        else
        {
            BE.SceneTown.instance.GainExp(CapacityTotal - (int)actualvalue);
            MissionsController.s.OnSoulsCollected(CapacityTotal - (int)actualvalue);
            distributeSouls(CapacityTotal - (int)actualvalue);
        }


        
        allObjects.transform.DOMoveY(-450,1f).OnComplete(destroy);
        souls.SetActive(true);
        particlesLogic[] particles;
        particles = souls.GetComponentsInChildren<particlesLogic>() as particlesLogic[];
        souls.transform.GetComponent<CanvasGroup>().DOFade(1f, 0.1f);
        foreach(particlesLogic part in particles)
        {
            part.moveCatrastofe();
        }
       // particles.moveCatrastofe();
        //souls.transform.DOMove(finalPos.transform.position, 1f);
    }

    void destroy()
    {

        Invoke("realDestroy", 1f);

            

        //GLOBALS.s.TUT_CAT_ALREADY_OCURRED = true;

        //TutorialController.s.catExplanation();

    }
    void realDestroy()
    {
        MenusController.s.destroyCat();
    }

    void distributeSouls(float  value)
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
                    temp = buildings[i].storeSouls(value,666, 666, 666);

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
