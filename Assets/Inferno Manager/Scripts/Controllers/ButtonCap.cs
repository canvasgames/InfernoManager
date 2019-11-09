using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonCap : MonoBehaviour {

    public bool deactivate = false;

    public void OnMouseUp()
    {
        if (deactivate == false)
            ActBT();
    }

    public void OnMouseEnter()
    {
        //if (deactivate == false)
           // GetComponent<Image>().color = Color.green;
    }

    public void OnMouseExit()
    {
        //if (deactivate == false)
            //GetComponent<Image>().color = Color.white;
    }

    public void DeactivateBt()
    {
        GetComponent<Image>().color = Color.gray;
        deactivate = true;
    }

    public void ActivateBt()
    {
        deactivate = false;
        GetComponent<Image>().color = Color.white;
    }
    public virtual void ActBT()
    {

    }
}
