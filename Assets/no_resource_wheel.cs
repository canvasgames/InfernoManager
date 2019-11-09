using UnityEngine;
using System.Collections;

public class no_resource_wheel : MonoBehaviour {
    public GameObject daddy;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void click()
    {
        daddy.SetActive(false);
    }
}
