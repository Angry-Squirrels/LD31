using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]

public class HeroControl : MonoBehaviour {

	public float moveSpeed;
	public float gravity;
	public Animation childAnim;

	private CharacterController mCharacterController;
	private Vector3 mMovementVector;
	private CollisionFlags mCollisionFlags;

	// Use this for initialization
	void Start () {
		mCharacterController = (CharacterController)GetComponent("CharacterController");
		mMovementVector = new Vector3();
	}
	
	// Update is called once per frame
	void Update () {
	
		mMovementVector.x = -Input.GetAxis("Horizontal");
		mMovementVector.z = -Input.GetAxis("Vertical");

		float prevVitY = mMovementVector.y;
		mMovementVector.Normalize();
		mMovementVector *= moveSpeed;

		mMovementVector.y = prevVitY - gravity;

		mCollisionFlags = mCharacterController.Move(mMovementVector * Time.deltaTime);

		if(IsOnFloor())
			mMovementVector.y = 0;

		if(mMovementVector.magnitude != 0)
			mCharacterController.transform.rotation = Quaternion.LookRotation(new Vector3(mMovementVector.x, 0, mMovementVector.z));
	
		float tspeed = Mathf.Sqrt(mMovementVector.x * mMovementVector.x + mMovementVector.z * mMovementVector.z);

		if(!childAnim.IsPlaying("attack")){
			if(tspeed > 0.03 && !childAnim.IsPlaying("run")){
				childAnim.Play("run");
			}else if(tspeed < 0.03 && !childAnim.IsPlaying("idle")){
				childAnim.Play("idle");
			}
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
			closestSlot.BroadcastMessage("OnSelect");
		}
		// sinon on peut attaquer

		if(Input.GetButtonDown("Attack"))
			childAnim.Play("attack");

	}

	bool IsOnFloor(){
		return (mCollisionFlags & CollisionFlags.Below) != 0;
	}
	
}
