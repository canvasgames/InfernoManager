using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CatastropheItem : MonoBehaviour {
    public GameObject cat;
    public GameObject locked;
    public GameObject Intensity, Chance,title,description, locked_cat;
    GameObject temp;
    // Use this for initialization
    void Start () {
        transform.localPosition = new Vector3(252.75f, transform.localPosition.y, transform.localPosition.z);
    }
	
	// Update is called once per frame
	void Update () {
   
	}

    public void initCatastrophe(int line)
    {
        if(line == 1)
        { 
            temp = (GameObject)Instantiate(cat, new Vector3(0, 0, 0), Quaternion.identity);
            locked_cat.SetActive(false);
            title.GetComponent<Text>().text = "Cat-astrophe";
            description.GetComponent<Text>().text = "A giant cat attacks, 40% chance to destroy a city \nReward: 100 souls";
        }
        else
        { 
            temp = (GameObject)Instantiate(locked, new Vector3(0, 0, 0), Quaternion.identity);
            Intensity.SetActive(false);
            Chance.SetActive(false);
            
            if(line == 2)
            {
                title.GetComponent<Text>().text = "Remove Pool's Stairs";
                description.GetComponent<Text>().text = "40% chance to drown people\nReward: 150 souls";
            }
            if (line == 3)
            {
                title.GetComponent<Text>().text = "Bacon Free";
                description.GetComponent<Text>().text = "Distribute Bacon, 50% chance of Infarts\nReward: 200 souls";
            }
            if (line == 4)
            {
                title.GetComponent<Text>().text = "Burn Nutella's";
                description.GetComponent<Text>().text = "Destroy Nutella's Factory, 40% of chance to raise the rate of suicides\nReward: 300 souls";
            }
            if (line == 5)
            {
                title.GetComponent<Text>().text = "Anvil Rain";
                description.GetComponent<Text>().text = "Sponsered by ACME, 60% chance of occurring rain of anvils\nReward: 500 souls";
            }

        }


        temp.transform.SetParent(gameObject.transform, false);
        temp.GetComponent<RectTransform>().localPosition = new Vector3(-180, 35, 0);
    }
}
