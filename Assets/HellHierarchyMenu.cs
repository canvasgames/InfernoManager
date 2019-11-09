using UnityEngine;
using System.Collections;
using DG.Tweening;

public class HellHierarchyMenu : MonoBehaviour {

	public GameObject player;
	public float level_dist = 85f;
	public static HellHierarchyMenu s;

	// Use this for initialization
	void Start () {
		s = this;
		//advance_player ();
	
	}

	public void advance_player(){
		player.transform.DOLocalMoveY(player.transform.localPosition.y - level_dist, 0.8f).SetEase (Ease.InOutCubic).OnComplete (advance_player_finished);
	}

	void advance_player_finished(){
		TutorialController.s.clickDemonLordToOpenCity ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
