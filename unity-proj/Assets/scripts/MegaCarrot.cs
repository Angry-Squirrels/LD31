using UnityEngine;
using System.Collections;

public class MegaCarrot : MonoBehaviour {

	public int maxLife = 100;
	public int mLife;
	public float EndY;

	float mStartY; 
	bool mDead = false;

	// Use this for initialization
	void Start () {
		mStartY = transform.position.y;
		mLife = maxLife;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TakeDammage(){
		mLife--;
		if(mLife <= 0){
			mLife = 0;
			mDead = true;
		}
		float t = (float)mLife / (float)maxLife;

		float y = mStartY - (mStartY - EndY) * (1-t);

		transform.position = new Vector3(transform.position.x, y, transform.position.z);
	}

	public bool IsDead(){
		return mDead;
	}
}
