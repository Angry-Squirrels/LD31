using UnityEngine;
using System.Collections;

public class DayNightCycleManager : MonoBehaviour {

	public static DayNightCycleManager instance; 

	// time in sec for each day phase
	public float aubeTime = 10.0f;
	public float dayTime = 50.0f;
	public float crepTime = 10.0f;
	public float nightTime = 50.0f;
	public float startingTimeOfDay = 25.0f;

	public float transitionTime = 2.0f;

	// camera an sun effect
	public Camera camera;
	public Light sun;
	public Light moon;

	public Vector3 sunAubeRotation;
	public Vector3 sunDayRotation;
	public Vector3 sunCrepRotation;
	public Vector3 sunNightRotation;

	public Vector3 moonAubeRotation;
	public Vector3 moonDayRotation;
	public Vector3 moonCrepRotation;
	public Vector3 moonNightRotation;

	// colors
	public Color daySkyColor;
	public Color crepSkyColor;
	public Color nightSkyColor;
	public Color aubeSkyColor;

	public Color daySunColor;
	public Color crepSunColor;
	public Color nightMoonColor;
	public Color aubeMoonColor;

	// 0 : 0h00 1 : 23h59m59s
	private float mCyclePosition;
	private float mTimeCounter;

	private Color mCurrentSkyColor;
	private Color mCurrentSunColor;
	private Color mCurrentMoonColor;

	private int mCurrentDayPart;
	private int mCurrentDay = 0;

	private float mCurrentTransitionTime;
	private bool mChangingDayPart;

	private bool mCycling;

	// Use this for initialization
	void Start () {
		instance = this;
		mCyclePosition = 0;
		mTimeCounter = startingTimeOfDay;
		mCurrentSkyColor = aubeSkyColor;
		mCurrentMoonColor = aubeMoonColor;
		mCurrentSunColor = daySunColor;
		mCurrentDayPart = 0;
		mCurrentTransitionTime = 0;
		mChangingDayPart = false;
		mCycling = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(mCycling)
			mTimeCounter += Time.deltaTime;

		if(mTimeCounter > aubeTime + dayTime + crepTime + nightTime){
			mTimeCounter = 0;
			mCurrentDay++;
			gameObject.SendMessage("OnNewDay", mCurrentDay);
		}

		if(mTimeCounter < aubeTime)
			ChangeDayPart(0);
		else if(mTimeCounter >= aubeTime && mTimeCounter < aubeTime + dayTime)
			ChangeDayPart(1);
		else if(mTimeCounter >= aubeTime + dayTime && mTimeCounter < aubeTime + dayTime + crepTime)
			ChangeDayPart(2);
		else if(mTimeCounter >= aubeTime + dayTime + crepTime && mTimeCounter < aubeTime + dayTime + crepTime + nightTime)
			ChangeDayPart(3);

		if(mChangingDayPart)
			UpdateTransition();
	}

	void ChangeDayPart(int part){
		if(mCurrentDayPart != part){
			mCurrentDayPart = part;
			mChangingDayPart = true; 
		}
	}
	
	void UpdateTransition ()
	{
		mCurrentTransitionTime += Time.deltaTime;
		float t = mCurrentTransitionTime / transitionTime;
		if(t >= 1){
			mCurrentTransitionTime = 0;
			mChangingDayPart = false;
			t = 1;
		}

		switch(mCurrentDayPart){
		case 0 :
			camera.backgroundColor = Color.Lerp(nightSkyColor, aubeSkyColor, t);
			moon.color = Color.Lerp(nightMoonColor, aubeMoonColor, t);
			moon.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(moonNightRotation), Quaternion.LookRotation(moonAubeRotation), t);
			sun.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(sunNightRotation), Quaternion.LookRotation(sunAubeRotation), t);
			break;
		case 1 :
			camera.backgroundColor = Color.Lerp(aubeSkyColor, daySkyColor, t);
			sun.color = Color.Lerp(aubeMoonColor, daySunColor, t);
			moon.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(moonAubeRotation), Quaternion.LookRotation(moonDayRotation), t);
			sun.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(sunAubeRotation), Quaternion.LookRotation(sunDayRotation), t);
			break;
		case 2 :
			camera.backgroundColor = Color.Lerp(daySkyColor, crepSkyColor, t);
			sun.color = Color.Lerp(daySunColor, crepSunColor, t);
			moon.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(moonDayRotation), Quaternion.LookRotation(moonCrepRotation), t);
			sun.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(sunDayRotation), Quaternion.LookRotation(sunCrepRotation), t);
			break;
		case 3 :
			camera.backgroundColor = Color.Lerp(crepSkyColor, nightSkyColor, t);
			moon.color = Color.Lerp(crepSunColor, nightMoonColor, t);
			moon.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(moonCrepRotation), Quaternion.LookRotation(moonNightRotation), t);
			sun.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(sunCrepRotation), Quaternion.LookRotation(sunNightRotation), t);
			break;
		}
	}

	public int GetTimerOfDay(){
		return mCurrentDayPart;
	}

	public float GetDayDuration(){
		return dayTime + crepTime;
	}

	public float GetNightDuration(){
		return nightTime + aubeTime;
	}

	public void StartCycles(){
		mCycling = true;
	}

	public void StopCycles(){
		mCycling = false;
	}

	public bool IsDay(){
		return GetTimerOfDay() == 1 || GetTimerOfDay() == 2;
	}
}
