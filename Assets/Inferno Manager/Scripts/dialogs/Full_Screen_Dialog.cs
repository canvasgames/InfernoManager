using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class Full_Screen_Dialog : MonoBehaviour
{
    public GameObject upPart, downPart, myText;
    float xPos, myTextHeight;
    // Use this for initialization
    void Start()
    {
        myTextHeight = myText.GetComponent<RectTransform>().rect.height;
        transform.localScale = new Vector3(1, 0, 1);
        Invoke("open", 0.3f);

    }

    // Update is called once per frame
    void Update()
    {
        upPart.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y , transform.localPosition.z);
        downPart.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - (GetComponent<RectTransform>().rect.height * transform.localScale.y), transform.localPosition.z);
        myText.GetComponent<RectTransform>().sizeDelta = new Vector2 (myText.GetComponent<RectTransform>().sizeDelta.x, myTextHeight * transform.localScale.y);
    }

    public void open()
    {
        changeText();
        transform.DOScaleY(1f, 0.3f);

    }

    public void closeAndReopen()
    {
        transform.DOScaleY(0f, 0.3f).OnComplete(open); 
    }

    public void closeAndDestroy()
    {
        transform.DOScaleY(0f, 0.3f).OnComplete(destroy);
        
    }
    void destroy()
    {
  
        MenusController.s.destroyMenu("", transform.parent.gameObject);
    }
    public void changeText()
    {
        if (GLOBALS.s.TUTORIAL_PHASE == 1)
            myText.GetComponentInChildren<Text>().text = "Hi, I'm <color=red>Satan!</color>  \n You've been promoted to a <color=red>DEMON LORD!</color>";
        else if (GLOBALS.s.TUTORIAL_PHASE == 2)
            myText.GetComponentInChildren<Text>().text = "You're now in charge of this area of Inferno.";
        else if (GLOBALS.s.TUTORIAL_PHASE == 3)
            myText.GetComponentInChildren<Text>().text = "This is your personal <color=red>HELL'S GATE</color>. It brings dead souls from earth.";
        else if (GLOBALS.s.TUTORIAL_PHASE == 4)
            myText.GetComponentInChildren<Text>().text = "Tap to Collect Souls.";
        else if (GLOBALS.s.TUTORIAL_PHASE == 5)
            myText.GetComponentInChildren<Text>().text = "Seem whe have no where to place the souls..\n What's your suggestion?\n";
        else if (GLOBALS.s.TUTORIAL_PHASE == 6)
            myText.GetComponentInChildren<Text>().text = "For the antichrist sake! \nWhy did I promoted you? \nThey deserve only ETERNAL PUNISHMENT!";
        //
        else if (GLOBALS.s.TUTORIAL_PHASE == 7)
            myText.GetComponentInChildren<Text>().text = "Now let's punish this sinner souls. Tap the BUILD Button.";
        else if (GLOBALS.s.TUTORIAL_PHASE == 8)
            myText.GetComponentInChildren<Text>().text = "Select a PUNISHER BUILDING.";
        // myText.GetComponentInChildren<Text>().text = "Hold the finger over the building and drag to replace it and Confirm.";
        else if (GLOBALS.s.TUTORIAL_PHASE == 9)
            myText.GetComponentInChildren<Text>().text = "Place it and confirm";
        // myText.GetComponentInChildren<Text>().text = "Tap to Collect Sadness.";
        else if (GLOBALS.s.TUTORIAL_PHASE == 10)
            myText.GetComponentInChildren<Text>().text = "Punisher Buildings increases your Souls Capacity";
        // myText.GetComponentInChildren<Text>().text = "Great! Now your Hell Gate can Generate Souls! As higher your sadness meter, more souls will be sent to your Hell!";
        else if (GLOBALS.s.TUTORIAL_PHASE == 11)
            myText.GetComponentInChildren<Text>().text = "Tap to Collect Souls.";
        else if (GLOBALS.s.TUTORIAL_PHASE == 12)
            myText.GetComponentInChildren<Text>().text = "Why the <color=blue>heaven</color> are you celebrating?\n You've just started!\n There is still a long path to get to be someone other than this <color=blue>bag of holiness</color> you are!";
        //myText.GetComponentInChildren<Text>().text = "Aquire more souls to Level Up and be respected.";
        else if (GLOBALS.s.TUTORIAL_PHASE == 14)
            myText.GetComponentInChildren<Text>().text = "To construct more Punisher Buildings you need <color=red>FIRE!</color> Tap to construct a Imp Pit.";
        else if (GLOBALS.s.TUTORIAL_PHASE == 15)
            myText.GetComponentInChildren<Text>().text = "Select the Fire Mine";
        else if (GLOBALS.s.TUTORIAL_PHASE == 16)
            myText.GetComponentInChildren<Text>().text = "Place it and confirm";
        else if (GLOBALS.s.TUTORIAL_PHASE == 17)
            myText.GetComponentInChildren<Text>().text = "Tap to collect fire";
        else if (GLOBALS.s.TUTORIAL_PHASE == 18)
            myText.GetComponentInChildren<Text>().text = "That's it! Now keep building and upgrading your buildings!";

        // myText.GetComponentInChildren<Text>().text = "Good job building yourself an Army! Now give me 50% of them to my next Heaven's Raid.";

        // myText.GetComponentInChildren<Text>().text = "I dont't think you understand who I Am! Just give me 60% instead 3=)";
    }


}