using UnityEngine;
using System.Collections;

public class RabbitSpawner : MonoBehaviour {

	public GameObject bunnyToSpawn;

	public int mBunniesToSpawnDurringDay = 3;
	public int mBunniesToSpawnDurringNight = 5;

	private float mDaySpawnRate;
	private float mNightSpawnRate;

	private float mSpawnCounter;

	private bool mStarted;

	private DayNightCycleManager mCycleManager;

	void OnNewDay(int day){
		mBunniesToSpawnDurringDay += (int)(day/1.5f);
		mBunniesToSpawnDurringNight += day;

		mDaySpawnRate = mCycleManager.GetDayDuration() / (mBunniesToSpawnDurringDay-1);
		mNightSpawnRate = mCycleManager.GetNightDuration() / (mBunniesToSpawnDurringNight-1);

		SpawnBunny();
	}

	void OnNewNight(){
		Debug.Log("new night");
		SpawnBunny();
	}

	// Use this for initialization
	void Start () {
		mSpawnCounter = 0;
		mStarted = false;

	}
	
	// Update is called once per frame
	void Update () {
		if(mCycleManager == null)
			mCycleManager = DayNightCycleManager.instance;

		mSpawnCounter += Time.deltaTime;
	
		if(mCycleManager.IsDay()){
			if(mSpawnCounter >= mDaySpawnRate)
			{
				SpawnBunny();
				mSpawnCounter = 0;
			}
		}else{
			if(mSpawnCounter >= mNightSpawnRate)
			{
				SpawnBunny();
				mSpawnCounter = 0;
			}
		}

	}

	void SpawnBunny ()
	{

		GameObject[] spawners = GameObject.FindGameObjectsWithTag("BunnySpawner");
		int spawnerIndex = (int)Random.Range(0, spawners.Length);
		GameObject spawner = spawners[spawnerIndex];

		if(mStarted == true){
			GameObject newBunny = (GameObject)Instantiate(bunnyToSpawn, spawner.transform.position, spawner.transform.rotation);
		}
	}

	void GameStart() {
		mStarted = true;
		OnNewDay(0);
	}
}
