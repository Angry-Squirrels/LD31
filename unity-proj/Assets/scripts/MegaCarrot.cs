using UnityEngine;
using System.Collections;

public class MegaCarrot : MonoBehaviour {

	public int maxLife = 20;
	int mLife;

	// Use this for initialization
	void Start () {
		mLife = maxLife;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TakeDammage(){
		mLife--;
	}
}
