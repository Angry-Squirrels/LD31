using UnityEngine;
using System.Collections;

public class Carrot : MonoBehaviour {

	int mLevel = 0;
	DayNightCycleManager mCycleManager;
	float mDayDuration = 0;
	float mGrowingTime = 0;
	int mAge = 0;
	Vector3 mGrowVector;

	public float maxTranslate = 10;
	public Animation anim;

	float mTotalTranslate = 0;
	float mStartY = 0;
	float mEndY = 0;

	bool mReady = false;

	// Use this for initialization
	void Start () {
		mGrowingTime = 0;
		mStartY = transform.position.y;
		mEndY = mStartY + maxTranslate;
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

			float t = mGrowingTime / mDayDuration;
			float y = Mathf.Lerp(mStartY, mEndY, t);
			transform.position.Set(transform.position.x, y, transform.position.z);

			if(t >= 1){
				t = 1;
				if(!mReady){
					mReady = true;
					anim.Play("ready");
				}
			}

		}
	} 

	void OnNewAge(){

	}

	public int GetLevel(){
		return mAge;
	}
}
