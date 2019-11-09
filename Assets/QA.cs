using UnityEngine;
using System.Collections;

public class QA : MonoBehaviour {

    public static QA s;

    public bool DontSave = false;
    public bool NoTutorial = false;
    public bool NoSatanEntering = false;
    public bool CameraNavigationOnRelease = false;
    public bool CrazyProduction = false;
    

    public int ProductionMultiplier = 1;

    public int first_game = 1;


    void Awake() {
        s = this;
        if (CrazyProduction) ProductionMultiplier = 20;
        else ProductionMultiplier = 1;

        first_game = PlayerPrefs.GetInt("first_game", 0);
        first_game = 0;
        if (first_game == 0) {
            PlayerPrefs.SetInt("first_game", 1);
            first_game = 1;
        }
        else
            first_game = 999;
    }
}

