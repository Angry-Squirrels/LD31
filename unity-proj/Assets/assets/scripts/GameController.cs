using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject menu;
	public GameObject hero;

	private DayNightCycleManager mDayManager;
	private int mCurrentDay = 0;

	// Use this for initialization
	void Start () {
		mDayManager = DayNightCycleManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame(){
		menu.SetActive(false);
		Instantiate(hero);
		mDayManager.StartCycles();
		BroadcastMessage("GameStart");
	}
}
