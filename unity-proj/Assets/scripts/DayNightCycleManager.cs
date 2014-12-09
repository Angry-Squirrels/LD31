using UnityEngine;
using System.Collections;

public delegate void newCycle(int _cycleName);

public class DayNightCycleManager : MonoBehaviour {

	public static DayNightCycleManager instance;

	public event newCycle onNewCycle;

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

	public GameObject sunObj;
	public GameObject moonObj;

	public Vector3 sunAubePos;
	public Vector3 sunDayPos;
	public Vector3 sunCrepPos;
	public Vector3 sunNightPos;

	public Vector3 moonAubePos;
	public Vector3 moonDayPos;
	public Vector3 moonCrepPos;
	public Vector3 moonNightPos;

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

	public MeshRenderer starsRenderer;

	// 0 : 0h00 1 : 23h59m59s
	private float mCyclePosition;
	private float mTimeCounter;

	private Color mCurrentSkyColor;
	private Color mCurrentSunColor;
	private Color mCurrentMoonColor;

	private int mCurrentDayPart;
	public int mCurrentDay = 0;

	private float mCurrentTransitionTime;
	private bool mChangingDayPart;

	private bool mCycling;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		mCyclePosition = 0;
		mTimeCounter = startingTimeOfDay;
		mCurrentSkyColor = aubeSkyColor;
		mCurrentMoonColor = aubeMoonColor;
		mCurrentSunColor = daySunColor;
		mCurrentDayPart = 0;
		mCurrentTransitionTime = 0;
		mChangingDayPart = false;
		mCycling = false;

		if (onNewCycle != null)
		{
			print ("start main theme");
			onNewCycle(4);
		}
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
			if (mCycling)
			{
				if(part == 3)
					gameObject.SendMessage("OnNewNight", mCurrentDay);
				if (onNewCycle != null)
				{
					onNewCycle(part);
				}
			}
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

		Color starsColor = starsRenderer.material.color;

		switch(mCurrentDayPart){
		case 0 :
			camera.backgroundColor = Color.Lerp(nightSkyColor, aubeSkyColor, t);
			moon.color = Color.Lerp(nightMoonColor, aubeMoonColor, t);
			moon.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(moonNightRotation), Quaternion.LookRotation(moonAubeRotation), t);
			sun.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(sunNightRotation), Quaternion.LookRotation(sunAubeRotation), t);
			starsColor.a = Mathf.Lerp(1.0f, 0.3f, t);
			starsRenderer.material.color = starsColor;
			sunObj.transform.position = Vector3.Lerp(sunNightPos, sunAubePos, t);
			moonObj.transform.position = Vector3.Lerp(moonNightPos, moonAubePos, t);
			break;
		case 1 :
			camera.backgroundColor = Color.Lerp(aubeSkyColor, daySkyColor, t);
			sun.color = Color.Lerp(aubeMoonColor, daySunColor, t);
			moon.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(moonAubeRotation), Quaternion.LookRotation(moonDayRotation), t);
			sun.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(sunAubeRotation), Quaternion.LookRotation(sunDayRotation), t);
			starsColor.a = Mathf.Lerp(0.3f, 0.0f, t);
			starsRenderer.material.color = starsColor;
			sunObj.transform.position = Vector3.Lerp(sunAubePos, sunDayPos, t);
			moonObj.transform.position = Vector3.Lerp(moonAubePos, moonDayPos, t);
			break;
		case 2 :
			camera.backgroundColor = Color.Lerp(daySkyColor, crepSkyColor, t);
			sun.color = Color.Lerp(daySunColor, crepSunColor, t);
			moon.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(moonDayRotation), Quaternion.LookRotation(moonCrepRotation), t);
			sun.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(sunDayRotation), Quaternion.LookRotation(sunCrepRotation), t);
			starsColor.a = Mathf.Lerp(0.0f, 0.3f, t);
			starsRenderer.material.color = starsColor;
			sunObj.transform.position = Vector3.Lerp(sunDayPos, sunCrepPos, t);
			moonObj.transform.position = Vector3.Lerp(moonDayPos, moonCrepPos, t);
			break;
		case 3 :
			camera.backgroundColor = Color.Lerp(crepSkyColor, nightSkyColor, t);
			moon.color = Color.Lerp(crepSunColor, nightMoonColor, t);
			moon.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(moonCrepRotation), Quaternion.LookRotation(moonNightRotation), t);
			sun.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(sunCrepRotation), Quaternion.LookRotation(sunNightRotation), t);
			starsColor.a = Mathf.Lerp(0.3f, 1.0f, t);
			starsRenderer.material.color = starsColor;
			sunObj.transform.position = Vector3.Lerp(sunCrepPos, sunNightPos, t);
			moonObj.transform.position = Vector3.Lerp(moonCrepPos, moonNightPos, t);
			break;
		}
	}

	public int GetTimerOfDay(){
		return mCurrentDayPart;
	}

	public float GetTotalDayDuration(){
		return dayTime + crepTime;
	}

	public float GetTotalNightDuration(){
		return nightTime + aubeTime;
	}

	public float GetAubeDuration(){
		return aubeTime;
	}

	public float GetCrepDuration(){
		return crepTime;
	}

	public float GetDayDuration(){
		return dayTime;
	}

	public float GetNightDuration(){
		return nightTime;
	}

	public void StartCycles(){
		mCycling = true;
		if (onNewCycle != null)
		{
			onNewCycle(1);
		}
	}

	public void StopCycles(){
		mCycling = false;
	}

	public bool IsDay(){
		return GetTimerOfDay() == 1 || GetTimerOfDay() == 2;
	}
}
