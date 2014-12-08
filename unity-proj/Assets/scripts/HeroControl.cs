using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]

public class HeroControl : MonoBehaviour {

	public float moveSpeed;
	public float gravity;
	public Animation childAnim;
	public Collider shovel;
	public float SnowManDistFromCarrot;
	public float MinDistBetweenSnowMen;
	public int SnowmanCarrotCost;
	public GameObject SnowManPrefab;

	private CharacterController mCharacterController;
	private Vector3 mMovementVector;
	private CollisionFlags mCollisionFlags;

	private GameController mGameController;

	public AudioSource SFXSource;
	public AudioClip digSound;
	public AudioClip getCarrotSound;

	// Use this for initialization
	void Start () {
		mCharacterController = (CharacterController)GetComponent("CharacterController");
		mMovementVector = new Vector3();
		mGameController = GameController.instance;
	}
	
	// Update is called once per frame
	void Update () {
	
		mMovementVector.x = -Input.GetAxis("Horizontal");
		mMovementVector.z = -Input.GetAxis("Vertical");

		float prevVitY = mMovementVector.y;
		mMovementVector.Normalize();
		mMovementVector *= moveSpeed;

		mMovementVector.y = prevVitY - gravity;

		if(!childAnim.IsPlaying("catch") && !childAnim.IsPlaying("dig"))
			mCollisionFlags = mCharacterController.Move(mMovementVector * Time.deltaTime);

		if(IsOnFloor())
			mMovementVector.y = 0;
		 
		if(mMovementVector.magnitude != 0 && !childAnim.IsPlaying("catch") && !childAnim.IsPlaying("dig") && !childAnim.IsPlaying("idle"))
			mCharacterController.transform.rotation = Quaternion.LookRotation(new Vector3(mMovementVector.x, 0, mMovementVector.z));
	
		float tspeed = Mathf.Sqrt(mMovementVector.x * mMovementVector.x + mMovementVector.z * mMovementVector.z);

		if(!childAnim.IsPlaying("attack") && !childAnim.IsPlaying("catch") && !childAnim.IsPlaying("dig")){
			if(tspeed > 0.03 && !childAnim.IsPlaying("run")){
				childAnim.Play("run");
			}else if(tspeed < 0.03 && !childAnim.IsPlaying("idle")){
				childAnim.Play("idle");
			}
			shovel.enabled = false;
		}

		GameObject[] slots = GameObject.FindGameObjectsWithTag("CarrotSlot");
		GameObject closestSlot = null;
		float minDist = float.MaxValue;
		foreach(GameObject slot in slots){
			float dist = Vector3.Distance(slot.transform.position, transform.position + transform.forward*5);
			if(dist < minDist)
			{
				minDist = dist;
				closestSlot = slot;
			}
		}

		// on peut planter une carrotte
		if(minDist <= 10){
			CarrotSlot theSlot = closestSlot.GetComponent<CarrotSlot>();
			if(Input.GetButtonDown("Action")){
				if(!theSlot.HasCarrot() && mGameController.nbCarrot > 0)
				{
					childAnim.Play("catch");
					SFXSource.clip = digSound;
					SFXSource.volume = 1;
					SFXSource.Play();
				}
				else if(theSlot.HasGrownCarrot())
				{
					childAnim.Play("dig");
					SFXSource.clip = getCarrotSound;
					SFXSource.volume = 0.5f;
					SFXSource.Play();
				}
			}
			closestSlot.BroadcastMessage("OnSelect");
		}

		if(minDist >= SnowManDistFromCarrot)
		{
			MakeSnowMan();
		}

		if(Input.GetButtonDown("Attack"))
			childAnim.Play("attack");

	}

	void MakeSnowMan ()
	{
		// get snoman list
		GameObject[] snowmen = GameObject.FindGameObjectsWithTag("Snowman");
		float minDist = float.MaxValue;
		foreach(GameObject snowman in snowmen){
			Snowman scriptSnow = snowman.GetComponent<Snowman>();
			if(scriptSnow.IsDead()) continue;
			float dist = Vector3.Distance(snowman.transform.position, transform.position+transform.forward * 7);
			if(dist < minDist)
				minDist = dist;
		}
		//Debug.Log(minDist + "/ " + MinDistBetweenSnowMen);
		/*
		if(minDist >= MinDistBetweenSnowMen)
			Debug.Log("can make a snow man");
		else
			Debug.Log("another snowman is too close");
			*/

		if(minDist >= MinDistBetweenSnowMen && 
		   Input.GetButtonDown("Action"))
		{
			if(mGameController.TakeCarrot(SnowmanCarrotCost)){
				Instantiate(SnowManPrefab, transform.position + transform.forward*8 - transform.up*2, Quaternion.identity);
				childAnim.Play("catch");
				SFXSource.clip = digSound;
				SFXSource.volume = 1;
				SFXSource.Play();
			}
		}

	}

	public void PelleActive(){
		shovel.enabled = true;
	}

	public void PelleInactive(){
		shovel.enabled = false;
	}

	bool IsOnFloor(){
		return (mCollisionFlags & CollisionFlags.Below) != 0;
	}
	
}
