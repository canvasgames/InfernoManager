using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class HellHierarchyMenu : MonoBehaviour {

	public GameObject player;
	public float level_dist = 85f;
	public static HellHierarchyMenu s;

	// Use this for initialization
	void Start () {
		s = this;
		//advance_player ();
	
	}

	private void OnEnable()
	{
		Debug.Log(GLOBALS.s.TUTORIAL_PHASE);
		if (GLOBALS.s.TUTORIAL_PHASE == 13)
			GetComponentInChildren<ScrollRect>().vertical = false;
		else
			GetComponentInChildren<ScrollRect>().vertical = true;
	}

	public void advance_player(){
		player.transform.DOLocalMoveY(player.transform.localPosition.y - level_dist, 0.8f).SetEase (Ease.InOutCubic).OnComplete (advance_player_finished);
	}

	void advance_player_finished(){
		TutorialController.s.clickDemonLordToOpenCity ();
	}
}
