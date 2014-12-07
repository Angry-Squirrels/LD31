using UnityEngine;
using System.Collections;

public class RabbitSpawner : MonoBehaviour {

	public GameObject bunnyToSpawn;

	private int mBunniesToSpawnDurringDay = 1;
	private int mBunniesToSpawnDurringNight = 1;

	private float mDaySpawnRate;
	private float mNightSpawnRate;

	private float mSpawnCounter;

	private bool mStarted;

	private DayNightCycleManager mCycleManager;

	void OnNewDay(int day){
		mBunniesToSpawnDurringDay = 1 + day * 2;
		mBunniesToSpawnDurringNight = 1 + day * 4;

		mDaySpawnRate = mCycleManager.GetDayDuration() / (mBunniesToSpawnDurringDay-1);
		mNightSpawnRate = mCycleManager.GetNightDuration() / (mBunniesToSpawnDurringNight-1);

		SpawnBunny();
	}

	void OnNewNight(){
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
