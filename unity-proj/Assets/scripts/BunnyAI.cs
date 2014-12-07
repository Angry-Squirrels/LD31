using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]

public class BunnyAI : MonoBehaviour {

	public float gravity = 3;
	public float movementSpeed = 10;

	enum BunnyState {Normal, Angry};

	private BunnyState mCurrentState;
	private Vector3 mMovement;
	private CollisionFlags mCollisionFlags; 
	private CharacterController mCharacterController;
	
	private Vector3 mDest;
	private float mNewDestTime;
	private float mNewDestCounter;
	private float mSpeed;
	private Quaternion mDestRotation;
	private Quaternion mOriginRotation;
	private float mRotateTimer;
	private float mTimeToRotate;
	private float mDistFromCenter = 75;

	// Use this for initialization
	void Start () {
		mMovement = new Vector3();
		mCurrentState = BunnyState.Normal;
		mCharacterController = (CharacterController)GetComponent("CharacterController");

		mDest = new Vector3();
		ChangeRandomly ();
	}

	void ChangeRandomly ()
	{
		mNewDestTime = Random.Range (1.0f, 3.0f);
		mNewDestCounter = 0;
		mSpeed = Random.Range (0, movementSpeed);
		mDestRotation = Quaternion.Euler(0, Random.Range (0, 360), 0);
		mOriginRotation = gameObject.transform.rotation;
		mRotateTimer = 0;
		mTimeToRotate = Random.Range(0.5f,2.0f);
	}
	
	// Update is called once per frame
	void Update () {
		int timeOfDay = DayNightCycleManager.instance.GetTimerOfDay();

		if (timeOfDay == 0 || timeOfDay == 3)
			ChangeState(BunnyState.Angry);
		else
			ChangeState(BunnyState.Normal);

		//if(mCurrentState == BunnyState.Normal)
			UpdateNormal();
		//else
		//	UpdateAngry();

		mMovement.y -= gravity;

		mCollisionFlags = mCharacterController.Move(mMovement * Time.deltaTime);

		if(IsOnFloor())
			mMovement.y = 0;
	}

	void ChangeState(BunnyState state){
		if(mCurrentState != state){
			mCurrentState = state;
		}
	}
	
	// move arround randomly
	void UpdateNormal(){
		mNewDestCounter += Time.deltaTime;
		if(mNewDestCounter >= mNewDestTime)
			ChangeRandomly();
		mRotateTimer += Time.deltaTime;
		float t = mRotateTimer / mTimeToRotate;
		if(t > 1)
			t = 1;

		//get closer to game center
		float fieldDist = Vector3.Distance(gameObject.transform.position, Vector3.zero);
		if(fieldDist > mDistFromCenter){
			float diffX = -gameObject.transform.position.x;
			float diffZ = -gameObject.transform.position.z;

			mDestRotation = Quaternion.LookRotation(new Vector3(diffX, 0, diffZ));
		}

		gameObject.transform.rotation = Quaternion.Lerp(mOriginRotation, mDestRotation, t);

		mMovement.x = gameObject.transform.forward.x * mSpeed;
		mMovement.z = gameObject.transform.forward.z * mSpeed;
	}

	void UpdateAngry(){
	}

	bool IsOnFloor(){
		return (mCollisionFlags & CollisionFlags.Below) != 0;
	}
}
