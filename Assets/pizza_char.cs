using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class pizza_char : MonoBehaviour {

    [HideInInspector] public int number_of_catastrophes;

    public GameObject part1_obj, part2_obj, part3_obj, part4_obj, part5_obj;
    public GameObject arrow, no_sugar, button_spin;
    float part1_pct, part2_pct, part3_pct, part4_pct, part5_pct;
    // Use this for initialization
    void Start () {
        define_percentages(60, 40, 0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void define_percentages(float part1pct, float part2pct, float part3pct, float part4pct, float part5pct)
    {
        part1_pct = part1pct;
        part2_pct = part2pct;
        part3_pct = part3pct;
        part4_pct = part4pct;
        part5_pct = part5pct;

        if (part3_pct <= 0)
        {
            part3_obj.SetActive(false);
        }
        if (part4_pct <= 0)
        {
            part4_obj.SetActive(false);
        }
        if (part5_pct <= 0)
        {
            part5_obj.SetActive(false);
        }

        part1_obj.GetComponent<Image>().fillAmount = part1_pct * 0.01f ;

        part2_obj.GetComponent<Image>().fillAmount = part2_pct * 0.01f;
        part2_obj.transform.DORotate (new Vector3(part2_obj.GetComponent<RectTransform>().rotation.x, 
            part2_obj.GetComponent<RectTransform>().rotation.y, (-360 * 0.01f * part1_pct)),0);


        part3_obj.GetComponent<Image>().fillAmount = part3_pct * 0.01f;
       // part3_obj.GetComponent<RectTransform>().rotation = new Quaternion(part3_obj.GetComponent<RectTransform>().rotation.x, part3_obj.GetComponent<RectTransform>().rotation.y, -(360 * (part1_pct + part2_pct)), 0);
        part3_obj.transform.DORotate(new Vector3(part3_obj.GetComponent<RectTransform>().rotation.x,
            part3_obj.GetComponent<RectTransform>().rotation.y, (-360 * 0.01f * (part1_pct + part2_pct))), 0);


        part4_obj.GetComponent<Image>().fillAmount = part4_pct * 0.01f;
        //part4_obj.GetComponent<RectTransform>().rotation = new Quaternion(part4_obj.GetComponent<RectTransform>().rotation.x, part4_obj.GetComponent<RectTransform>().rotation.y, -(360 * (part1_pct + part2_pct + part3_pct)), 0);
        part4_obj.transform.DORotate(new Vector3(part4_obj.GetComponent<RectTransform>().rotation.x,
           part4_obj.GetComponent<RectTransform>().rotation.y, (-360 * 0.01f * (part1_pct + part2_pct + part3_pct))), 0);



        part5_obj.GetComponent<Image>().fillAmount = part5_pct * 0.01f;
        //part5_obj.GetComponent<RectTransform>().rotation = new Quaternion(part5_obj.GetComponent<RectTransform>().rotation.x, part5_obj.GetComponent<RectTransform>().rotation.y, -(360 * (part1_pct + part2_pct + part3_pct + part4_pct)), 0);
        part5_obj.transform.DORotate(new Vector3(part5_obj.GetComponent<RectTransform>().rotation.x,
    part5_obj.GetComponent<RectTransform>().rotation.y, (-360 * 0.01f * (part1_pct + part2_pct + part3_pct + part4_pct))), 0);

        
    }

    public void rotate()
    {
        float angle;
        if (GLOBALS.s.TUT_CAT_ALREADY_OCURRED == false)
        {
            GLOBALS.s.TUT_CAT_ALREADY_OCURRED = true;
            angle = -7300;
            TutorialController.s.endCatBT();
        }
        else
        {
            angle = Random.Range(-8000, -5000);
        }
         
        transform.DORotate(new Vector3(0, 0, angle), 3, RotateMode.LocalAxisAdd).OnComplete(print_angle);

        
    }

    void print_angle()
    {
        
        float angle_temp = transform.GetComponent<RectTransform>().eulerAngles.z;
        if (angle_temp >=0 && angle_temp <= (360 * part1_pct * 0.01f))
        {
           // Debug.Log("Caiu no 1");
            no_sugar.SetActive(true);
            Invoke("no_sugar_off", 2f);
       
        }
        else if(angle_temp <= ((360 * (part1_pct + part2_pct) * 0.01f))  )   
        {
            //Debug.Log("Caiu no 2");
            Invoke("cat_gatinho", 1.4f);
        }
        else if (angle_temp <= ( (360 * (part1_pct + part2_pct + part3_pct) * 0.01f)))
        {
           // Debug.Log("Caiu no 3");
        }
        else if (angle_temp <= ((360 * (part1_pct + part2_pct + part3_pct + part4_pct) * 0.01f)))
        {
           // Debug.Log("Caiu no 4");
        }
        else if (angle_temp <= ((360 * (part1_pct + part2_pct + part3_pct + part4_pct + part5_pct) * 0.01f)))
        {
            //Debug.Log("Caiu no 5");
        }
    }

    void no_sugar_off()
    {
        no_sugar.SetActive(false);
        button_spin.GetComponent<rotate_bt_roulette>().running_state_false();
    }

    void cat_gatinho()
    {
        button_spin.GetComponent<rotate_bt_roulette>().running_state_false();
        MenusController.s.createCatastrophe(0);
        MenusController.s.destroyMenu("CatastropheList", null);
    }
}
