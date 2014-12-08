using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]

public class BunnyAI : MonoBehaviour {

	public float gravity = 3;
	public float movementSpeed = 25;
	public float mEatRate = 2.0f;
	public float minDistToFlee = 10;
	public Animation animation;

	public SkinnedMeshRenderer renderer;
	public Texture mignon;
	public Texture mechant;

	public int maxLife = 10;
	public int life = 10;

	enum BunnyState {Normal, Transforming, Angry, Dead};

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
	private bool mFleeing = false;
	private float mAir = 0.0f;
	private Vector3 mFinalMoveVector;
	private bool mStartedTransform = false;

	private GameObject mCurrentTarget;

	private bool mEating = false;
	float mEatingTimer = 0;

	// Use this for initialization
	void Start () {
		mMovement = new Vector3();
		mFinalMoveVector = new Vector3();
		mCurrentState = BunnyState.Normal;
		mCharacterController = (CharacterController)GetComponent("CharacterController");
		mDest = new Vector3();
		ChangeRandomly ();
	}

	void ChangeRandomly ()
	{
		int moveMode = Random.Range(0,2);

		// idle
		if(moveMode == 0)
		{
			mNewDestTime = Random.Range(1.0f, 3.0f);
			mSpeed = 0;
			mTimeToRotate = 1.0f;
			if(!animation.IsPlaying("idle"))
				animation.Play("idle");
		}else{
			mNewDestTime = Random.Range (3.0f, 6.0f);
			mSpeed = movementSpeed;
			mTimeToRotate = Random.Range(0.5f,2.0f);
			if(!animation.IsPlaying("run"))
				animation.Play("run");
		}
		mNewDestCounter = 0;
		mDestRotation = Quaternion.Euler(0, Random.Range (0, 360), 0);
		mOriginRotation = gameObject.transform.rotation;
		mRotateTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		int timeOfDay = DayNightCycleManager.instance.GetTimerOfDay();

		if ((timeOfDay == 0 || timeOfDay == 3) && !mStartedTransform)
			ChangeState(BunnyState.Transforming);
		else if(timeOfDay == 1 || timeOfDay == 2){
			ChangeState(BunnyState.Normal);
			mStartedTransform = false;
		}

		if(mCurrentState == BunnyState.Normal)
			UpdateNormal();
		else if(mCurrentState == BunnyState.Angry)
			UpdateAngry();
		else if(mCurrentState == BunnyState.Dead)
			UpdateDead();
		else if(mCurrentState == BunnyState.Transforming)
			UpdateTransforming();

		mMovement.y -= gravity;

		mFinalMoveVector.x = mMovement.x * mAir;
		mFinalMoveVector.z = mMovement.z * mAir;
		mFinalMoveVector.y = mMovement.y;

		mCollisionFlags = mCharacterController.Move(mFinalMoveVector * Time.deltaTime);

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

		animation["run"].speed = 1.0f;

		//get closer to game center
		float fieldDist = Vector3.Distance(gameObject.transform.position, Vector3.zero);
		if(fieldDist > mDistFromCenter){
			float diffX = -gameObject.transform.position.x;
			float diffZ = -gameObject.transform.position.z;

			mDestRotation = Quaternion.LookRotation(new Vector3(diffX, 0, diffZ));
		}

		//flee from hero
		GameObject hero = GameObject.FindGameObjectWithTag("Player");
		float distFromPlayer = Vector3.Distance(hero.transform.position, transform.position);

		if(distFromPlayer < minDistToFlee){
			Vector3 fleeVector = transform.position - hero.transform.position;
			fleeVector.y = 0;
			mDestRotation =  Quaternion.LookRotation(fleeVector);
			mSpeed = movementSpeed * 2.0f;
			animation["run"].speed = 2.0f;
			if(!animation.IsPlaying("run"))
				animation.Play("run");
		}

		gameObject.transform.rotation = Quaternion.Lerp(mOriginRotation, mDestRotation, t);

		mMovement.x = gameObject.transform.forward.x * mSpeed;
		mMovement.z = gameObject.transform.forward.z * mSpeed;

		if(renderer.material.mainTexture != mignon)
			renderer.material.SetTexture(0, mignon);
	}

	void UpdateTransforming(){
		if(!mStartedTransform){
			animation["wake"].speed = Random.Range(0.5f, 1.0f);
			animation.Play("wake");
			mStartedTransform = true;
			renderer.material.SetTexture(0, mechant);

			mMovement.x = 0;
			mMovement.z = 0;

			GameObject megaCarrot = GameObject.FindGameObjectWithTag("BigCarrot");
			float diffX = transform.position.x - megaCarrot.transform.position.x;
			float diffZ = transform.position.z - megaCarrot.transform.position.z;
			transform.rotation = Quaternion.LookRotation(new Vector3(-diffX, 0, -diffZ));
		}
	}

	public void TransformEnded(){
		ChangeState(BunnyState.Angry);
	}

	void UpdateAngry(){

		// getList of carrot slots
		GameObject[] slots = GameObject.FindGameObjectsWithTag("CarrotSlot");

		// gezt closest slot with a carrot
		GameObject closestsFullSlots = null;
		float minDist = float.MaxValue;

		foreach (GameObject slot in slots){
			CarrotSlot slotScript = slot.GetComponent<CarrotSlot>();
			float dist = Vector3.Distance(slot.transform.position, transform.position);
			if(slotScript.HasGrownCarrot() &&  dist < minDist){
				minDist = dist;
				closestsFullSlots = slot;
			}
		}

		animation["run"].speed = 2.0f;

		if(mMovement.magnitude > 1.0f && !animation.IsPlaying("run"))
			animation.Play("run");

		if(closestsFullSlots != null)
			GoTowardSlot(closestsFullSlots);
		else
			GotTowardMegaCarrot();
	}

	void GoTowardSlot (GameObject closestsFullSlots)
	{
		mCurrentTarget = closestsFullSlots;

		mSpeed = movementSpeed * 2.0f;

		float diffX = closestsFullSlots.transform.position.x - transform.position.x;
		float diffZ = closestsFullSlots.transform.position.z - transform.position.z;
		float dist = Mathf.Sqrt(diffX * diffX + diffZ * diffZ);

		transform.rotation = Quaternion.LookRotation(new Vector3(diffX, 0, diffZ));

		mMovement.x = gameObject.transform.forward.x * mSpeed;
		mMovement.z = gameObject.transform.forward.z * mSpeed;

		if(dist < 2){
			mMovement = Vector3.zero;
			if(!mEating){
				mEating = true;
				mEatingTimer = 0;
				animation.Play("eat");
			}else{
				mEatingTimer += Time.deltaTime;
				if(mEatingTimer > mEatRate){
					StealCarrot();
					mEating = false;
					mEatingTimer = 0;
				}
			}
		}else{
			mEating = false;
			mEatingTimer = 0;
		}
		
	}

	void StealCarrot(){
		CarrotSlot carrotSlotScript = mCurrentTarget.GetComponent<CarrotSlot>();
		carrotSlotScript.StealCarrot();
	}

	void UpdateDead(){
	}

	bool IsOnFloor(){
		return (mCollisionFlags & CollisionFlags.Below) != 0;
	}

	void GotTowardMegaCarrot ()
	{
		GameObject megaCarrot = GameObject.FindGameObjectWithTag("BigCarrot");
		if (megaCarrot != null)
		{
			MegaCarrot megaCarrotScript = megaCarrot.GetComponent<MegaCarrot>();

			mSpeed = movementSpeed * 2.0f;
			
			float diffX = megaCarrot.transform.position.x - transform.position.x;
			float diffZ = megaCarrot.transform.position.z - transform.position.z;
			float dist = Mathf.Sqrt(diffX * diffX + diffZ * diffZ);
			
			transform.rotation = Quaternion.LookRotation(new Vector3(diffX, 0, diffZ));
			
			mMovement.x = gameObject.transform.forward.x * mSpeed;
			mMovement.z = gameObject.transform.forward.z * mSpeed;

			if(dist < 6){
				mMovement = Vector3.zero;

				if(!mEating){
					mEating = true;
					mEatingTimer = 0;
					animation.Play("eat");
				}else{
					mEatingTimer += Time.deltaTime;
					if(mEatingTimer > mEatRate){
						mEatingTimer = 0;
						megaCarrotScript.TakeDammage();
					}
				}
			}
		}
	}

	public void TakeDammage(int dammage){
		life -= dammage;
		if(life <= 0){
			Destroy(gameObject);
		}
	}

	public void InAir(){
		mAir = 1.0f;
	}

	public void LandJump(){
		mAir = 0.0f;
	}

	public bool IsAngry(){
		return mCurrentState == BunnyState.Angry;
	}
	
}
