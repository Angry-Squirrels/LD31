using UnityEngine;
using System.Collections;

public class CarrotSlot : MonoBehaviour {

	public GameObject carrotToInstantiate;

	float mBaseY;
	bool mSelected;
	bool mHasCarrot;
	GameObject mCarrotte;
	GameController mGameController;
	
	// Use this for initialization
	void Start () {
		mBaseY = transform.position.y;
		mSelected = false;
		mHasCarrot = false;
		mGameController = GameController.instance;
	}
	
	// Update is called once per frame
	void Update () {
		if(mSelected){
			gameObject.transform.Translate(Vector3.down*10);
			mSelected = false;
		}
	}

	void OnSelect(){
		mSelected = true;
		gameObject.transform.Translate(Vector3.up*10);

		if(Input.GetButtonDown("Action")){
			if(!mHasCarrot && mGameController.TakeCarrot(1)){
				mHasCarrot = true;
				mCarrotte = (GameObject)Instantiate(carrotToInstantiate);
				mCarrotte.transform.SetParent(transform);
				mCarrotte.transform.position = transform.position;
				//mCarrotte.transform.Translate(Vector3.up * 2);
			}else{
				Carrot carrot = mCarrotte.GetComponent<Carrot>();
				if(carrot.GetLevel() > 0){
					DestroyImmediate(mCarrotte);
					mHasCarrot = false;
					int carrotTaken = (int)(carrot.GetLevel() * Mathf.Ceil(carrot.GetLevel() * 0.5f) + 1);
					mGameController.GetCarrot(carrotTaken);
				}
			}
		}
	}

	void OnDeselect(){
		mSelected = false;
	}

	public bool HasCarrot(){
		return mHasCarrot;
	}

	public bool HasGrownCarrot(){
		if(mCarrotte != null)
		{
			Carrot carrotScript = mCarrotte.GetComponent<Carrot>();
			return carrotScript.GetLevel() >= 1;
		}

		return false;
	}

	public void StealCarrot(){
		mHasCarrot = false;
		DestroyImmediate(mCarrotte);
	}
}
