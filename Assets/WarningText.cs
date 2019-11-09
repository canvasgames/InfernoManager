using UnityEngine;
using System.Collections;
using DG.Tweening;

public class WarningText : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init() {
        Invoke("StartFade", 1.5f);
    }

    void StartFade() {
        GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete(() => Destroy(this));
    }

}
