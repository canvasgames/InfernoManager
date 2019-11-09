using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SoulsGroupBehaviour : MonoBehaviour {
	public GameObject allObjects, souls;
	bool not_clicked = true;
	// Use this for initialization
	void Start () {
		
		//GetComponent<SpriteRenderer> ().DOFade (1, 0.4f);
		move();
	}

	// Update is called once per frame
	void Update () {

	}

	void move()
	{
		float time;
		time = 10f;
		transform.DOLocalMoveX(transform.position.x - 50, time).SetEase(Ease.Linear).OnComplete(destroy);
	}
	void destroy()
	{
		//GetComponentInChildren<SoulsGroupAlha> ().fadeout ();
		BroadcastMessage ("fadeout");
		//Destroy(transform.gameObject);
		//Invoke ("destroy_for_real", 0.6f);
	}
	void destroy_for_real(){
		Destroy(transform.gameObject);
	}


	void OnClick(){
		if (not_clicked) {
			Debug.Log ("ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZOOOOOOOOOO");
			not_clicked = false;
			int CapacityTotal = BE.BEGround.instance.GetCapacityTotal (BE.PayType.Elixir);

			double actualvalue = BE.SceneTown.Elixir.ChangeDelta ((double)Random.Range (10, 20));

			if (CapacityTotal >= actualvalue + 100) {
				BE.SceneTown.instance.GainExp ((int)100);
				MissionsController.s.OnSoulsCollected (100);
				distributeSouls (100f);
			} else {
				BE.SceneTown.instance.GainExp (CapacityTotal - (int)actualvalue);
				MissionsController.s.OnSoulsCollected (CapacityTotal - (int)actualvalue);
				distributeSouls (CapacityTotal - (int)actualvalue);
			}



			allObjects.transform.DOMoveY (-450, 1f).OnComplete (destroy);
			souls.SetActive (true);
			particlesLogic[] particles;
			particles = souls.GetComponentsInChildren<particlesLogic> () as particlesLogic[];
			souls.transform.GetComponent<CanvasGroup> ().DOFade (1f, 0.1f);
			foreach (particlesLogic part in particles) {
				part.moveCatrastofe ();
			}
		}

	}

	void distributeSouls(float  value)
	{
		BE.Building[] buildings;
		buildings = GameObject.FindObjectsOfType(typeof(BE.Building)) as BE.Building[];
		int lenght = buildings.Length;

		float discountEach = 1;
		int i = 0, cont = 0;
		float temp;
		while (value > 0)
		{
			for (i = 0; i < lenght; i++)
			{
				if (value >= 1)
					temp = buildings[i].storeSouls(discountEach,666, 666, 666);
				else
					temp = buildings[i].storeSouls(value,666, 666, 666);

				value = value - temp;
				if (value == 0f)
				{
					break;
				}

			}

			cont++;
			if (cont > 1000)
			{
				break;
			}
			i = 0;
		}
	}
}
