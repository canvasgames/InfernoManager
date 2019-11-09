using UnityEngine;
using System.Collections;

public class demonsCreator : MonoBehaviour {
    float createDemonTime,randpos;
    GameObject tempObject;
    // Use this for initialization
    void Start () {
        createDemonTime = 2;
		//createSoulsGroup ();
		Invoke("createSoulsGroup", Random.Range(15f,30f));

    }
	
	// Update is called once per frame
	void Update () {
        createDemonTime = createDemonTime - Time.deltaTime;
        if(createDemonTime <= 0)
        {
            createDemonTime = Random.Range(3f, 10f);
            createDemon();
        }

	}

    void createDemon()
    {
        tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/flyerDemon"));
        randpos = Random.Range(-20f, 20f);
        tempObject.transform.position = new Vector3(transform.position.x, transform.position.y, randpos);
    }

	void createSoulsGroup()
	{	
		if(GLOBALS.s.TUTORIAL_OCCURING == false) {
			tempObject = (GameObject)Instantiate(Resources.Load("Prefabs/RunningSoulsGroup"));
			//randpos = Random.Range(160f, 260f);
			randpos = Random.Range(-40f, 0f);
			tempObject.transform.position = new Vector3(transform.position.x, transform.position.y, randpos);

			//Invoke("createSoulsGroup", Random.Range(25f,50f));
		}
		Invoke("createSoulsGroup", Random.Range(15f,30f));
	}

}
