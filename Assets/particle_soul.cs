using UnityEngine;
using System.Collections;
using DG.Tweening;

public class particle_soul : MonoBehaviour {

    float pos_x, pos_y, pos_z;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void vaiviado(float pos_x, float pos_y, float pos_z)
    {
        this.pos_x = pos_x;
        this.pos_y = pos_y;
        this.pos_z = pos_z;
        Invoke("move_for_real", Random.Range(0.35f, 0.6f));
    }

    public void move_for_real() {
        transform.DOJump(new Vector3(pos_x, pos_y, pos_z), Random.Range(-7, 7), 1, Random.Range(0.6f, 2.0f)).OnComplete(suicide_solution);

    }

    void suicide_solution()
    {
        Destroy(gameObject);
    }
}
