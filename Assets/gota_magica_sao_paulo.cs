using UnityEngine;
using System.Collections;
using DG.Tweening;

public class gota_magica_sao_paulo : MonoBehaviour {
    float original_y;
	// Use this for initialization
	void Start () {
        original_y = transform.position.y;
        move_down();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void move_down()
    {
        transform.DOMoveY(-70,1.7f);
        transform.DOScale(0.8f, 2).OnComplete(enrola_ai);
    }



    void enrola_ai()
    {
        transform.position = new Vector3(transform.position.x, original_y, transform.position.z);
        transform.DOScale(1, Random.Range(1,3)).OnComplete(move_down); 
    }
}
