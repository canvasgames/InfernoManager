using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public enum ScrollType {
    smallScroll = 1, bigScroll = 2, bigScrollQuestion = 3, missionsDialog = 4
}
public class DialogsTexts : MonoBehaviour
{
    
    public ScrollType myScrollType = ScrollType.smallScroll;
    public GameObject upPart, downPart, myText;
    RectTransform myTextRect, myRect;
    float xPos, myTextHeight;
    string text_displaying = "";
    string text_final = "";
    int k = 0;
    string cur_color = "";
    string curTextState = "m";
    Text text_component;
    //Vector3 upperLeftPos = new Vector3(46,-162,0);
    Vector3 upperLeftPos = new Vector3(40,-32,0);
    Vector3 upperLeftPosBig = new Vector3(30,0,0);    //Vector3 middleCenterPos = new Vector3(14,-168,0);
    // Use this for initialization
    void Start()
    {
        myTextRect = myText.GetComponent<RectTransform>();
        myTextHeight = myTextRect.rect.height;
        myRect = GetComponent<RectTransform>();
        transform.localScale = new Vector3(1, 0, 1);
        ChangeTextFormat("l");
        Invoke("open", 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        upPart.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y , transform.localPosition.z);
        downPart.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - (myRect.rect.height * transform.localScale.y), transform.localPosition.z);
        myTextRect.sizeDelta = new Vector2 (myTextRect.sizeDelta.x, myTextHeight * transform.localScale.y);
    }

    public void open()
    {
        if(GLOBALS.s.TUTORIAL_OCCURING == true)
            changeText("",true);
        transform.DOScaleY(1f, 0.3f);

    }

    public void closeAndReopen(string textAlign = "")
    {
        transform.DOScaleY(0f, 0.3f).OnComplete(open);
        if (textAlign != "") ChangeTextFormat(textAlign);
    }

    public void closeAndDestroy()
    {
        transform.DOScaleY(0f, 0.3f).OnComplete(destroy);
        
    }

    void destroy()
    {
        MenusController.s.destroyMenu("", transform.parent.gameObject);
    }

    void ChangeTextFormat(string state) {
        if(curTextState != state && myScrollType != ScrollType.missionsDialog) {
            curTextState = state;
            if(state == "m") { // change the text to middle and center
                if(myScrollType == ScrollType.smallScroll) {
                    Debug.Log("CHANGING TEXT STATE TO MIDDLE");
                    myText.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
                    myText.GetComponentInChildren<Text>().transform.localPosition -= upperLeftPos;
                }
                else {
                    myText.GetComponentInChildren<Text>().alignment = TextAnchor.UpperCenter;
                    myText.GetComponentInChildren<Text>().transform.localPosition -= upperLeftPosBig;
                }
                
            }
            else if (state == "l") {
                if (myScrollType == ScrollType.smallScroll) {
                    Debug.Log("CHANGING TEXT STATE TO LEFT");
                    myText.GetComponentInChildren<Text>().alignment = TextAnchor.UpperLeft;
                    myText.GetComponentInChildren<Text>().transform.localPosition += upperLeftPos;
                }
                else {
                    myText.GetComponentInChildren<Text>().alignment = TextAnchor.UpperLeft;
                    myText.GetComponentInChildren<Text>().transform.localPosition += upperLeftPosBig;
                }
            }
        }
    }

    public void changeText(string dialogName = "", bool writeText = false)
    {
        string text_to_display = "";
        Debug.Log("CHANGE TEXT CALLED");
        //Debug.Log(dialogName);
        if(dialogName == "")
        {
            if (GLOBALS.s.TUTORIAL_PHASE == 1)
            {
                Debug.Log("LOCAL POSITION: " + myText.GetComponentInChildren<Text>().transform.localPosition);
                text_to_display = "That was quite  an entrance,\nright?";
                ChangeTextFormat("l");
            }

            //text_to_display = "Hi, I'm Satan!  \n You've been promoted to DEMON LORD!";

            else if (GLOBALS.s.TUTORIAL_PHASE == 2)
                text_to_display = "You're now in charge of\nthis area of the Hell.\nThat's your Palace.";
            else if (GLOBALS.s.TUTORIAL_PHASE == 3)
                text_to_display = "This is your personal\n<color=#fe2323>Hell's Gate</color>. It brings\ndead souls from earth.";
            else if (GLOBALS.s.TUTORIAL_PHASE == 4)
            {
                text_to_display = "Tap to Collect Souls.";
                //myText.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
            }
            else if (GLOBALS.s.TUTORIAL_PHASE == 5)
                text_to_display = "It seems we have nowhere\nto place the souls..\nWhat's your suggestion?";
            else if (GLOBALS.s.TUTORIAL_PHASE == 6)
                text_to_display = "For <color=#fe2323>Antichrist sake!</color>\nWhy did I promoted you?\nThey deserve <color=#fe2323>Punishment!</color>";
            //
            else if (GLOBALS.s.TUTORIAL_PHASE == 7)
                text_to_display = "Now lets punish this sinner souls.\nTap the <color=green>Build Button</color>.";
            else if (GLOBALS.s.TUTORIAL_PHASE == 8)
                text_to_display = "Select one of the\n<color=#fe2323>Punisher Buldings!</color>";
            // text_to_display = "Hold the finger over the building and drag to replace it and Confirm.";
            else if (GLOBALS.s.TUTORIAL_PHASE == 9)
            {
                text_to_display = "Place it and confirm";
                //myText.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
            }

            // text_to_display = "Tap to Collect Sadness.";
            else if (GLOBALS.s.TUTORIAL_PHASE == 10)
                text_to_display = "<color=#fe2323>Punisher Buildings</color> increases\nyour <color=#fe2323>Souls Capacity</color>";

            else if (GLOBALS.s.TUTORIAL_PHASE == -1)
            {
                text_to_display = "Now kick the chicken";
                //myText.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
            }

            else if (GLOBALS.s.TUTORIAL_PHASE == -2)
                text_to_display = "MUAHAHA! It's always fun\nto kick a chicken. Although\nthat has nothing to\ndo with the game";

            // text_to_display = "Great! Now your Hell Gate can Generate Souls! As higher your sadness meter, more souls will be sent to your Hell!";
            else if (GLOBALS.s.TUTORIAL_PHASE == 11)
            {
                text_to_display = "Tap to Collect Souls.";
                //myText.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
            }

            else if (GLOBALS.s.TUTORIAL_PHASE == 12)
                text_to_display = "Why the <color=blue>heaven</color> are you \ncelebrating?\nYou barely started!\nThere is still a long path\nto get to be someone other\nthan this <color=blue>bag of holiness</color>\nyou are!";
            //text_to_display = "Aquire more souls to Level Up and be respected.";
            else if (GLOBALS.s.TUTORIAL_PHASE == 14)
                text_to_display = "To build Punisher Buildings\nyou need <color=#fe2323>Hellfire!</color> Tap\nto build a Fire Mine.";
            else if (GLOBALS.s.TUTORIAL_PHASE == 15)
                text_to_display = "Now Select the <color=#fe2323>Fire Mine</color>";
            else if (GLOBALS.s.TUTORIAL_PHASE == 16)
            {
                text_to_display = "Place it and confirm";
                //myText.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
            }
            else if (GLOBALS.s.TUTORIAL_PHASE == 17)
                text_to_display = "Tap to collect Hellfire";
            else if (GLOBALS.s.TUTORIAL_PHASE == 18)
            {
                //myText.GetComponentInChildren<Text>().alignment = TextAnchor.UpperCenter;
                text_to_display = "Acceptable work, <color=blue>Mr Holiness</color>!\n\nKeep building and upgrading\nyour buildings!\nAcquire souls to Level Up\nand be respected.\n";
            }
            else if (GLOBALS.s.TUTORIAL_PHASE == 19)
                text_to_display = "If you want any missions, tap me.\nAdios! ";
            else if (GLOBALS.s.TUTORIAL_PHASE == 21)
            {
                text_to_display = "Select the Resources Tab";
                //myText.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
            }
            else if (GLOBALS.s.TUTORIAL_PHASE == 25)
                text_to_display = "Tap at your Rank to\ncheck the <color=red>Demons Rank</color> List";
            else if (GLOBALS.s.TUTORIAL_PHASE == 100)
                text_to_display = "Tap here to check the new catastrophes wheel!";
            else if (GLOBALS.s.TUTORIAL_PHASE == 102)
                text_to_display = "Tap to spin the wheel! \n Good luck!";
            else if (GLOBALS.s.TUTORIAL_PHASE == 104)
                text_to_display = "You can re-spin using gems or waiting. But this time is for free, spin again!";
            else if (GLOBALS.s.TUTORIAL_PHASE == 101)
            {
                text_to_display = "Back to business then.\nI'm promoting you to \n<color=#fe2323>Demon Lord!</color>";

                //myText.GetComponentInChildren<Text>().alignment = TextAnchor.UpperCenter;
            }
            else if (GLOBALS.s.TUTORIAL_PHASE == -13)
                text_to_display = "Tap the Demon Lord\nto check his Town";
            else if (GLOBALS.s.TUTORIAL_PHASE == -14)
                text_to_display = "Nice City, hm?";
        }
        else
        {
            Debug.Log("Calllllllllllllllllling");
            if (dialogName == "teste")
            {
                Debug.Log("foifofifiofifo ele");
                text_to_display = "Bacana issaê";
            }

            else if (string.Equals(dialogName, "MissionSpank"))
                text_to_display = "Spanking Hitler more than 1931415 times really bores you. You are a nice replacement";
            else if (string.Equals(dialogName, "MissionBuild"))
                text_to_display = "Seems you do have sense of humor on punishing people with exotic ways";
            else if (string.Equals(dialogName, "MissionCollect"))
                text_to_display = "Remember that you need Souls if you want to be respected";
            else if (string.Equals(dialogName, "HellDiplomacy"))
                text_to_display = "Hey Demon, \n\n Give me 50% of all your Hellfire, I need it for my next Heaven's raid  \n\n Best regards, Satan";
            else if (string.Equals(dialogName, "HellDiplomacy2"))
                text_to_display = "MUAAHHAHAHAHAHHAHA.  \n\n I don't think you understand your position. Give me 60% instead and I will tolerate you this time";
        }


        // PRINT THE TEXT
        if (curTextState == "l")
        {
            // myText.GetComponentInChildren<Text>().text = "Good job building yourself an Army! Now give me 50% of them to my next Heaven's Raid.";
            //letter by letter code
            k = 0;
            text_displaying = "";
            text_final = text_to_display;
            myText.GetComponentInChildren<Text>().text = "";
            cur_color = "";
            text_component = myText.GetComponentInChildren<Text>();
            //Invoke("display_text", 0.2f);
            TextWriter.s.write_text(text_component, text_final);

            ChangeTextFormat("l");

            //myText.GetComponentInChildren<Text>().alignment = TextAnchor.UpperLeft;
            //myText.GetComponentInChildren<Text>().transform.localPosition = upperLeftPos;
        }
        else
        {
            myText.GetComponentInChildren<Text>().text = text_to_display;
            ChangeTextFormat("m");

        }

    }

    


}