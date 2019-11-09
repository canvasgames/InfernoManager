using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {
    public static SoundController s;

    public AudioClip ChickenHit;

    void Awake()
    {
        s = this;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
