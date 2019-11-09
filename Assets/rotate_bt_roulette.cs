using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class rotate_bt_roulette : MonoBehaviour {
    public GameObject pizza_chart, dialog_no_fire, fire_icon, gem_icon;
    bool running = false;
    bool re_spin = false;
    int re_spin_cost = 1;
    int spinned = 0;

    DateTime newDate, oldDate;
    string stringDate;
    // Use this for initialization
    void Start () {
        gem_icon.SetActive(false);
        spinned = PlayerPrefs.GetInt("Spinned", 0);
        //Debug.Log(spinned + "     SPINNNENENNENEN");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(spinned == 1 && re_spin == false)
        {
            day_passed();
        }
    }

    public void click()
    {
        if (GLOBALS.s.TUTORIAL_OCCURING == true && GLOBALS.s.TUTORIAL_PHASE == 104)
        {
            if (running == false )
            {
                running = true;
                PlayerPrefs.SetInt("Spinned", 1);
                pizza_chart.GetComponent<pizza_char>().rotate();
                TutorialController.s.end_tutorial_spin();
            }
        }
        else
        {
            if (running == false && spinned == 0)
            {
                running = true;
                PlayerPrefs.SetInt("Spinned", 1);
                PlayerPrefs.SetString("SpinTime", System.DateTime.Now.ToString());

                if (re_spin == false)
                {
                    if (BE.SceneTown.Gold.Target() >= 200 || GLOBALS.s.already_spinned == false)
                    {
                        if(GLOBALS.s.already_spinned == true) BE.SceneTown.Gold.ChangeDelta(-200);
                        pizza_chart.GetComponent<pizza_char>().rotate();
                        GLOBALS.s.already_spinned = true;

                    }
                    else
                    {
                        dialog_no_fire.SetActive(true);
                    }
                }
                else
                {
                    if (BE.SceneTown.Gem.Target() >= re_spin_cost)
                    {
                        BE.SceneTown.Gem.ChangeDelta(-re_spin_cost);
                        pizza_chart.GetComponent<pizza_char>().rotate();
                    }
                    else
                    {
                        dialog_no_fire.SetActive(true);
                    }
                }

            }
            else
            {
                if (BE.SceneTown.Gem.Target() >= 4)
                {
                    BE.SceneTown.Gem.ChangeDelta(-4);
                    pizza_chart.GetComponent<pizza_char>().rotate();
                }
                else
                {
                    dialog_no_fire.SetActive(true);
                }
            }
        }
    }

    public void running_state_false()
    {
        running = false;
        re_spin = true;

        re_spin_cost++;
        fire_icon.SetActive(false);
        gem_icon.SetActive(true);
        transform.GetComponentInChildren<Text>().text = "Re-Spin! \n"+ re_spin_cost+" ";

        
    }

    bool day_passed()
    {
        newDate = System.DateTime.Now;
        stringDate = PlayerPrefs.GetString("SpinTime");

        if (stringDate == "")
        {
            oldDate = newDate;
            PlayerPrefs.SetString("SpinTime", newDate.ToString());
        }
        else
        {
            oldDate = Convert.ToDateTime(stringDate);
        }

       // Debug.Log("LastDay: " + oldDate);
       // Debug.Log("CurrDay: " + newDate);

        TimeSpan difference = newDate.Subtract(oldDate);

       //  Debug.Log("Dif min: " + difference.Minutes);
        if (difference.Minutes >= 1)
        {
            PlayerPrefs.SetInt("Spinned", 0);
            spinned = 0;

            transform.GetComponentInChildren<Text>().text = "Spin! \n200 ";
            fire_icon.SetActive(true);
            gem_icon.SetActive(false);
            transform.GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 1);
            return true;

        }
        else if(difference.Minutes >= 0)
        {
            DateTime dif = oldDate.AddMinutes(1);
            TimeSpan diff = newDate.Subtract(dif);
            transform.GetComponentInChildren<Text>().text = "Wait " + (-diff.Minutes) +"m " + (-diff.Seconds) + "s "
               + "\n Or pay 4      " ;
            fire_icon.SetActive(false);
            gem_icon.SetActive(true);
            //transform.GetComponent<Image>().color = new Vector4(0.8f, 0.8f, 0.8f, 1);
        }
        else
        {
            PlayerPrefs.SetInt("Spinned", 0);
            spinned = 0;

            transform.GetComponentInChildren<Text>().text = "Spin the Wheel \nCost: 200  ";
            fire_icon.SetActive(true);
            gem_icon.SetActive(false);
        }

        return false;
    }
}
