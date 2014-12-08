using UnityEngine;
using System.Collections;

public class Carrot : MonoBehaviour {

	int mLevel = 0;
	DayNightCycleManager mCycleManager;
	float mDayDuration = 0;
	float mGrowingTime = 0;
	int mAge = 0;
	Vector3 mGrowVector;

	public float growSpeed;

	// Use this for initialization
	void Start () {
		mCycleManager = DayNightCycleManager.instance;
		mDayDuration = mCycleManager.GetTotalDayDuration();
		mGrowVector = new Vector3(0.25f,1.0f,0.25f);
	}
	
	// Update is called once per frame
	void Update () {
		if(mCycleManager.IsDay()){
			mGrowingTime += Time.deltaTime;
			int newAge = (int)Mathf.Floor(mGrowingTime / mDayDuration);
			if(mAge != newAge){
				mAge = newAge;
				OnNewAge();
			}
			transform.Translate(Vector3.up * growSpeed * Time.deltaTime / mDayDuration);
			transform.localScale += mGrowVector * growSpeed * Time.deltaTime / mDayDuration;
		}
	} 

	void OnNewAge(){

	}

	public int GetLevel(){
		return mAge;
	}
}
