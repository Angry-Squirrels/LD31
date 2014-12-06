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

		mCharacterController.transform.rotation = Quaternion.LookRotation(mMovementVector);

	}

	bool IsOnFloor(){
		return (mCollisionFlags & CollisionFlags.Below) != 0;
	}
}
