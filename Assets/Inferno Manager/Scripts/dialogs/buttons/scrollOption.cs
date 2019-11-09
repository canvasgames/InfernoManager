using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class scrollOption : ButtonCap
{

    // Update is called once per frame
    void Update()
    {


    }

    public override void ActBT()
    {
        TutorialController.s.questionAnswered();
    }
}
