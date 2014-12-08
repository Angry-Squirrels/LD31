using UnityEngine;
using System.Collections;

public class HeroAnimEvent : MonoBehaviour {

	public HeroControl heroControl;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PellActive(){
		heroControl.PelleActive();
	}

	public void PelleInactive(){
		heroControl.PelleInactive();
	}
}
