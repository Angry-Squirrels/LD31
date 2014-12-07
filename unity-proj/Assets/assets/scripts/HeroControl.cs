using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]

public class HeroControl : MonoBehaviour {

	public float moveSpeed;
	public float gravity;

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
	
		mMovementVector.x = -Input.GetAxis("Horizontal") * moveSpeed;
		mMovementVector.z = -Input.GetAxis("Vertical") * moveSpeed;
		mMovementVector.y -= gravity;

		mCollisionFlags = mCharacterController.Move(mMovementVector * Time.deltaTime);

		if(IsOnFloor())
			mMovementVector.y = 0;

		if(mMovementVector.magnitude != 0)
			mCharacterController.transform.rotation = Quaternion.LookRotation(mMovementVector);

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

		if(minDist <= 6){
			closestSlot.BroadcastMessage("OnSelect");
		}

	}

	bool IsOnFloor(){
		return (mCollisionFlags & CollisionFlags.Below) != 0;
	}
	
}
