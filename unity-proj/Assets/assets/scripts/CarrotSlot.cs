using UnityEngine;
using System.Collections;

public class CarrotSlot : MonoBehaviour {

	public GameObject carrotToInstantiate;

	float mBaseY;
	bool mSelected;
	bool mHasCarrot;
	GameObject mCarrotte;
	
	// Use this for initialization
	void Start () {
		mBaseY = transform.position.y;
		mSelected = false;
		mHasCarrot = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(mSelected){
			gameObject.transform.Translate(Vector3.down*10);
			mSelected = false;
		}

		if(mHasCarrot)
		{
			mCarrotte.transform.SetParent(transform);
		}
	}

	void OnSelect(){
		mSelected = true;
		gameObject.transform.Translate(Vector3.up*10);

		if(Input.GetButtonDown("Action")){
			if(!mHasCarrot){
				mHasCarrot = true;
				GameObject carotte = (GameObject)Instantiate(carrotToInstantiate);
				mCarrotte = carotte;
				mCarrotte.transform.position.Set(transform.position.x, transform.position.y, transform.position.z);
			}
		}
	}

	void OnDeselect(){
		mSelected = false;
	}
}
