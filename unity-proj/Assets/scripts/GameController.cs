using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject menu;
	public GameObject hero;
	public GameObject gameUi;

	private DayNightCycleManager mDayManager;
	private int mCurrentDay = 0;

	public static GameController instance;

	public int nbCarrot = 3;
	private int nbRabbitKilled = 0;

	public MegaCarrot megaCarrot;

	public GameOverGUIScript gameOverUI;
	public Base_SoundManager musicManager;

	bool isGameOver = false;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		mDayManager = DayNightCycleManager.instance;
		Time.timeScale = 1;
		menu.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		if (megaCarrot.IsDead() && !isGameOver)
		{
			isGameOver = true;

			musicManager.StopAllMusics();
			musicManager.PlayMusic("gameover");

			Time.timeScale = 0;
			gameUi.SetActive(false);
			gameOverUI.updateText(mCurrentDay, nbCarrot, nbRabbitKilled);
			gameOverUI.gameObject.SetActive(true);
		}
	}

	public void StartGame(){
		menu.SetActive(false);
		gameUi.SetActive(true);
		Instantiate(hero, Vector3.up * 10, Quaternion.identity);
		mDayManager.StartCycles();
		megaCarrot.gameObject.SetActive (true);
		BroadcastMessage("GameStart");
	}

	public void resetGame()
	{
		Time.timeScale = 1;
		Application.LoadLevel("testA");
	}

	public void GetCarrot(int nb){
		nbCarrot += nb;
	}

	public bool TakeCarrot(int nb){
		if(nbCarrot >= nb){
			nbCarrot -= nb;
			return true;
		}
		return false;
	}

	public void onRabbitKilled()
	{
		nbRabbitKilled++;
	}

	public int getNbRabbitKilled()
	{
		return nbRabbitKilled;
	}
}
