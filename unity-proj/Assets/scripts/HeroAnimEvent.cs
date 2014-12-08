using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroAnimEvent : MonoBehaviour {

	public HeroControl heroControl;

	public AudioSource stepSource;
	public List<AudioClip> stepSounds;

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

	public void playStepSound()
	{
		stepSource.clip = stepSounds [Random.Range (0, 2)];
		stepSource.Play ();
	}
}
